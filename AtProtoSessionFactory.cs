using FishyFlip;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;

namespace BskyBot;

public class AtProtoSessionFactory(IConfiguration config, ILogger<AtProtoSessionFactory> logger)
{
    public async Task<ATProtocol?> CreateSession()
    {
        var debugLog = new DebugLoggerProvider();
        var atProtocolBuilder = new ATProtocolBuilder()
            .EnableAutoRenewSession(false)
            .WithInstanceUrl(config.GetValue<Uri>("CheneyBot_PdsUri"))
            .WithLogger(debugLog.CreateLogger("CheneyBotDebug"));

        var atProtocol = atProtocolBuilder.Build();

        var result = await atProtocol.Server.CreateSessionAsync(config.GetValue<string>("CheneyBot_AppUser"),
            config.GetValue<string>("CheneyBot_AppPassword"), CancellationToken.None);

        result.Switch(
            success => logger.LogInformation($"Session: {success.Did}"),
            error => throw new Exception($"Error: {error.StatusCode} {error.Detail}")
        );

        return atProtocol;
    }
}
