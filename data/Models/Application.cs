using System.Text.Json.Serialization;

namespace data.Models;

public class Application
{
    [JsonPropertyName("id")]
    public Guid? ID { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("pipeline")]
    public Guid? PipelineVersionId { get; set; }
    
    [JsonPropertyName("org")]
    public Guid? Organization { get; set; }

    [JsonPropertyName("variables")]
    public Dictionary<string,string> Variables { get; set; }
    
    [JsonPropertyName("source")]
    public string Source { get; set; }
}