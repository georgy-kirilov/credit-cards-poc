using CreditCards;
using CreditCards.Contracts;
using CreditCards.Data;
using CreditCards.Features;
using EpsAdapter;
using EpsAdapter.Data;
using EpsAdapter.Data.Models;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Runner;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();
builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddMassTransit(bus =>
{
    bus.SetKebabCaseEndpointNameFormatter();

    bus.AddConsumer<RequestCardStatusChangeToEpsConsumer>();
    bus.AddConsumer<CardStatusChangedConsumer>();

    bus.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq", "/", host =>
        {
            host.Username(builder.Configuration["RABBITMQ_USER"]);
            host.Password(builder.Configuration["RABBITMQ_PASSWORD"]);
        });

        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddScoped<RequestCardStatusChangeHandler>();
builder.Services.AddDatabase<CreditCardsDbContext>(builder.Configuration["CREDIT_CARDS_DATABASE"], CreditCardsDbContext.Schema);
builder.Services.AddKeyedTransient<ICardOperationPublisher, EpsCardOperationPublisher>(CardIssuer.Eps);

builder.Services.AddDatabase<EpsAdapterDbContext>(builder.Configuration["EPS_ADAPTER_DATABASE"], EpsAdapterDbContext.Schema);
builder.Services.AddHostedService<EpsWorker>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using var scope = app.Services.CreateAsyncScope();

    using var creditCardsDbContext = scope.ServiceProvider.GetRequiredService<CreditCardsDbContext>();
    creditCardsDbContext.Database.Migrate();

    if (!creditCardsDbContext.Cards.Any())
    {
        var card = Card.Create(CardIssuer.Eps);
        creditCardsDbContext.Cards.Add(card);
        creditCardsDbContext.SaveChanges();

        using var epsDbContext = scope.ServiceProvider.GetRequiredService<EpsAdapterDbContext>();
        epsDbContext.Database.Migrate();
        var epsCard = EpsCard.Create(Guid.NewGuid(), card.Id);
        epsDbContext.Cards.Add(epsCard);
        epsDbContext.SaveChanges();
    }
}

app.MapPost("api/request-card-status-change", RequestCardStatusChangeEndpoint.Map);

app.Run();
