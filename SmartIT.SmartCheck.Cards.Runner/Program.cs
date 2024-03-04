using SmartIT.SmartCheck.Cards.Runner;

var builder = WebApplication.CreateBuilder(args);

builder.EnableDevelopmentDebugging();

var app = builder.Build();

app.MapGet("/sum", () =>
{
    int a = Random.Shared.Next();
    int b = Random.Shared.Next();
    long sum = a + b;
    return sum;
});

app.Run();
