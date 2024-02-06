using CreditCards.Contracts;
using CreditCards.Data;
using CreditCards.Features;
using EpsAdapter;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<RequestCardStatusChangeHandler>();
builder.Services.AddKeyedTransient<ICardOperationPublisher, EpsCardOperationPublisher>(CardIssuer.Eps);
builder.Services.AddDbContext<CreditCardsDbContext>(options => options.UseSqlServer(builder.Configuration["Database"]));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using var scope = app.Services.CreateScope();
    using var dbContext = scope.ServiceProvider.GetRequiredService<CreditCardsDbContext>();

    dbContext.Database.EnsureDeleted();
    dbContext.Database.EnsureCreated();

    var card = Card.Create(CardIssuer.Eps);
    dbContext.Cards.Add(card);
    dbContext.SaveChanges();
}

app.UseHttpsRedirection();

app.MapPost("api/request-card-status-change", RequestCardStatusChangeEndpoint.Map);

//app.MapPost("api/eps/card-status-changed")

app.Run();
