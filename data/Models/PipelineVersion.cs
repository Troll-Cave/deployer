using System.Text.Json.Serialization;
using data.Models;

namespace api.Models;

public class PipelineVersion
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("pipeline")]
    public Guid Pipeline { get; set; }
    
    [JsonPropertyName("yaml")]
    public string YAML { get; set; }
    
    [JsonPropertyName("files")]
    public Dictionary<string, string> Files { get; set; } = new();
}