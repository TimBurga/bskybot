using BskyBot;
using BskyBot.Bots;
using Coravel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder();

builder.Services.AddScheduler();

builder.Services.AddTransient<CheneyBot>();

_ = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", true)
    .AddEnvironmentVariables()
    .Build();

builder.Services.AddLogging(x => x.AddSimpleConsole().SetMinimumLevel(LogLevel.Information));

var app = builder.Build();

app.Services.UseScheduler(scheduler =>
    scheduler.Schedule<CheneyBot>()
        .DailyAtHour(16)
        .RunOnceAtStart());

try
{
    app.Run();
}
catch (Exception e)
{
    Console.WriteLine(e);
    await app.StopAsync();
}
