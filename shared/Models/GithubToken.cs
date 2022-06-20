using System.Text.Json.Serialization;

namespace shared.Models;

public class GithubToken
{
    [JsonPropertyName("token")]
    public string Token { get; set; }
}