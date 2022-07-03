using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using data.Models;

namespace data.DataModels;

[Table("pipeline_version")]
public class PipelineVersionDTO
{
    [Column("id"), Key()]
    public Guid ID { get; set; }
    
    [Column("name")]
    public string Name { get; set; }
    
    [Column("pipeline")]
    public Guid PipelineId { get; set; }
    
    [Column("code", TypeName = "jsonb")]
    public PipelineCode Code { get; set; }
    
    [Column("yaml")]
    public string YAML { get; set; }
    
    [Column("files", TypeName = "jsonb")]
    public PipelineVersionFiles Files { get; set; }
}

public class PipelineVersionFiles
{
    [JsonPropertyName("files")]
    public Dictionary<string, string> Files { get; set; }
}
