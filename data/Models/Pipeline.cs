using System.Text.Json.Serialization;

namespace data.Models;

public class Pipeline
{
    [JsonPropertyName("id")]
    public Guid? ID { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("org")]
    public Guid? Organization { get; set; }
}