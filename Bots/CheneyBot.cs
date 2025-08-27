using Coravel.Invocable;
using FishyFlip;
using Microsoft.Extensions.Logging;

namespace BskyBot.Bots;

public class CheneyBot(AtProtoSessionFactory atProto, PulseChecker pulseChecker, DeduplicatingFactProvider factProvider, ILogger<CheneyBot> logger) : IInvocable
{
    public async Task Invoke()
    {
        ATProtocol? session = null;

        try
        {
            logger.LogInformation("CheneyBot has woken up cranky as usual");

            var isAlive = await pulseChecker.IsAlive();
            logger.LogInformation("Alive? " + isAlive);

            session = await atProto.CreateSession();

            var postText = BuildPostText(isAlive);
            logger.LogInformation("Crafted new post: " + postText);

            var postResult = await session.Repo.CreatePostAsync(postText);

            postResult.Switch(
                success => { logger.LogInformation($"Success! Post: {success.Uri} {success.Cid}"); },
                error => throw new Exception($"Error: {error.StatusCode} {error.Detail}"));
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Could not post status");
        }
        finally
        {
            session?.Dispose();
            logger.LogInformation("CheneyBot snarls and goes back to sleep");
        }
    }

    private string BuildPostText(bool isAlive)
    {
        var momentOfTruth = isAlive ? "remains alive today" : "is burning in hell on this glorious day";

        var postText = $"Dick Cheney, {factProvider.GetFact()}, {momentOfTruth}, {DateTime.Today.ToLongDateString()}";

        if (postText.Length > 300)
        {
            postText = postText.Substring(0, 297) + "...";
        }

        return postText;
    }
}
