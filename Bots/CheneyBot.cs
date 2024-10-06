using Coravel.Invocable;
using FishyFlip;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;

namespace BskyBot.Bots;

public class CheneyBot(IConfiguration config, ILogger<CheneyBot> logger) : IInvocable
{
    private List<string> _facts = [
        "who redefined torture as enhanced interrogation techniques",
        "champion of waterboarding",
        "who championed torture that a Senate investigation determined was not an effective way to gain intelligence from detainees",
        "who attempted to block the creation of the 9/11 commission that eventually determined he lied",
        "who repeatedly lied to the public about connections between Iraq and terrorists",
        "who said he has no problem with torture as long as it achieves objectives",
        "whose approval rating stood at a staggeringly low 13 percent at the end of his tenure as Vice President",
        "who proudly declared he'd authorize torture again in a minute",
        "who continues to insist that torture worked despite the contrary findings of a Senate investigation",
        "who insists that rectal rehydration and feeding of detainees was done for medical reasons despite medical experts saying otherwise",
        "who continues to argue that anything short of causing pain equal to that of organ failure is not torture",
        "who was unmoved by the revelation that 25% of the prisoners who were tortured were found innocent",
        "the architect of some of the most disastrous foreign and domestic policies of the early 21st century",
        "who pushed for a war based on known false premises of weapons of mass destruction and a link between Iraq and Al Qaeda",
        "who is responsible for an estimated half a million or more Iraqi civilian deaths",
        "who advocated strongly for the passage of the Patriot Act which granted the government sweeping surveillance powers over US citizens",
        "whose policies eroded civil liberties, violated human rights, destabilised entire regions, and left a legacy of fear, instability, and anger that continues to haunt the world today",
    ];

    public async Task Invoke()
    {
        logger.LogInformation("CheneyBot has woken up");
        var session = await CreateSession();

        var postText = BuildPostText();

        var postResult = await session.Repo.CreatePostAsync(postText);

        postResult.Switch(
            success =>
            {
                // Contains the ATUri and CID.
                Console.WriteLine($"Post: {success.Uri} {success.Cid}");
            },
            error => throw new Exception($"Error: {error.StatusCode} {error.Detail}"));

        logger.LogInformation("CheneyBot going back to sleep");
        session.Dispose();
    }

    private string BuildPostText()
    {
        var template = $"Dick Cheney, %fact%, remains alive today, {DateTime.Today.ToLongDateString()}";
        var filled = template.Replace("%fact%", GetRandomFact());

        if (filled.Length > 300)
        {
            filled = filled.Substring(0, 297) + "...";
        }

        return filled;
    }

    private string GetRandomFact()
    {
        var rand = new Random();
        var num = rand.Next(0, _facts.Count - 1);
        return _facts[num];
    }

    private async Task<ATProtocol?> CreateSession()
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