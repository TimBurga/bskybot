using Coravel.Invocable;
using Microsoft.Extensions.Logging;

namespace BskyBot.Bots;

public class CheneyBot(AtProtoSessionFactory sessionFactory, PulseChecker pulseChecker, CheneyFactProvider factProvider, ILogger<CheneyBot> logger) : IInvocable
{
    public async Task Invoke()
    {
        logger.LogInformation("CheneyBot has woken up");

        var isAlive = await pulseChecker.IsAlive();
        logger.LogInformation("Alive? " + isAlive);

        var session = await sessionFactory.CreateSession();

        var postText = BuildPostText(isAlive);
        logger.LogInformation("Crafted new post: " + postText);

        var postResult = await session.Repo.CreatePostAsync(postText);

        postResult.Switch(
            success =>
            {
                logger.LogInformation($"Success! Post: {success.Uri} {success.Cid}");
            },
            error => throw new Exception($"Error: {error.StatusCode} {error.Detail}"));

        logger.LogInformation("CheneyBot snarls and goes back to sleep");
        session.Dispose();
    }

    private string BuildPostText(bool isAlive)
    {
        var spinMeRightRoundBaby = isAlive ? "remains alive today" : "is burning in hell today";

        var postText = $"Dick Cheney, {factProvider.Random()}, {spinMeRightRoundBaby}, {DateTime.Today.ToLongDateString()}";

        if (postText.Length > 300)
        {
            postText = postText.Substring(0, 297) + "...";
        }

        return postText;
    }
}
