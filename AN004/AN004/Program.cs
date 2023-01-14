var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// https://learn.microsoft.com/zh-tw/aspnet/core/fundamentals/configuration/?view=aspnetcore-7.0
IConfiguration configuration = app.Configuration;
string from = configuration["from"]!;
string environment = app.Environment.EnvironmentName;

app.MapGet("/", () => $"Hello World! {from} in {environment}");

app.Run();
