using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace BskyBot.Bots;

public class PulseChecker(HttpClient httpClient)
{
    public async Task<bool> IsAlive()
    {
        var result = await httpClient.GetAsync("?action=query&prop=extracts&origin=*&format=json&pageids=5058628&exintro&explaintext");

        result.EnsureSuccessStatusCode();

        var response = await result.Content.ReadFromJsonAsync<PulseCheckResponse>();

        return !response.Query.Pages.Details.Extract.Contains("died", StringComparison.InvariantCultureIgnoreCase);
    }
}

public class PulseCheckResponse
{
    public string batchcomplete { get; set; }
    [JsonPropertyName("query")]
    public Query Query { get; set; }
}

public class Query
{
    [JsonPropertyName("pages")]
    public Pages Pages { get; set; }
}

public class Pages
{
    [JsonPropertyName("5058628")]
    public Details Details{ get; set; }
}

public class Details
{
    public int pageid { get; set; }
    public int ns { get; set; }
    public string title { get; set; }
    [JsonPropertyName("extract")]
    public string Extract { get; set; }
}
