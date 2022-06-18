using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
    public Pipeline Code { get; set; }
    
    [Column("files", TypeName = "jsonb")]
    public Dictionary<string, string> Files { get; set; }
}
