using Coravel.Invocable;
using FishyFlip;
using Microsoft.Extensions.Logging.Debug;
using Microsoft.Extensions.Options;

namespace BskyBot.Bots;

public class CheneyBot(IOptions<CheneyBotOptions> config) : IInvocable
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
        "who has no problem with the revelation that 25% of the prisoners who were tortured were found innocent",
        "the architect of some of the most disastrous foreign and domestic policies of the early 21st century",
        "who pushed for a war based on known false premises of weapons of mass destruction and a link between Iraq and Al Qaeda",
        "who is responsible for an estimated half a million Iraqi civilian deaths",
        "who advocated strongly for the passage of the Patriot Act which granted the government sweeping surveillance powers over US citizens",
        "whose policies eroded civil liberties, violated human rights, destabilised entire regions, and left a legacy of fear, instability, and anger that continues to haunt the world today",
    ];

    public async Task Invoke()
    {
        var postText = BuildPostText();
        var session = await CreateSession();


        var postResult = await session.Repo.CreatePostAsync(postText);

        postResult.Switch(
            success =>
            {
                // Contains the ATUri and CID.
                Console.WriteLine($"Post: {success.Uri} {success.Cid}");
            },
            error => throw new Exception($"Error: {error.StatusCode} {error.Detail}"));

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
            .WithInstanceUrl(config.Value.PdsUri)
            .WithLogger(debugLog.CreateLogger("CheneyBotDebug"));

        var atProtocol = atProtocolBuilder.Build();

        var result = await atProtocol.Server.CreateSessionAsync(config.Value.AppUser, config.Value.AppPassword, CancellationToken.None);
        result.Switch(
            success => Console.WriteLine($"Session: {success.Did}"),
            error => throw new Exception($"Error: {error.StatusCode} {error.Detail}")
        );

        return atProtocol;
    }
}