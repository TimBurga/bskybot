using Microsoft.Extensions.DependencyInjection;

namespace BskyBot.Bots;

public static class StartupExtensions
{
    public static void AddCheneyBot(this IServiceCollection services)
    {

        services.AddTransient<CheneyBot>();
        services.AddTransient<CheneyFactProvider>();
        services.AddTransient<AtProtoSessionFactory>();
        
        services.AddHttpClient<PulseChecker>(x => x.BaseAddress = new Uri("https://en.wikipedia.org/w/api.php"));
    }
}