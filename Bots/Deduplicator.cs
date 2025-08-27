namespace BskyBot.Bots;

public class DeduplicatingFactProvider(CheneyFactProvider factProvider)
{
    private readonly Dictionary<DateTime, string> _history = new();
    private readonly Func<KeyValuePair<DateTime, string>, bool> _expired = x => DateTime.Now > x.Key.AddDays(ExpirationDays);

    private const int ExpirationDays = 2;

    public string GetFact()
    {
        foreach (var item in _history.Where(_expired))
        {
            _history.Remove(item.Key);
        }

        var todaysFact = factProvider.GetFact();
        while (_history.ContainsValue(todaysFact))
        {
            todaysFact = factProvider.GetFact();
        }

        _history.Add(DateTime.Now, todaysFact);
        return todaysFact;
    }
}
