using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace data.DataModels;

[Table("application")]
public class ApplicationDTO
{
    [Column("id"), Key()]
    public Guid ID { get; set; }
    
    [Column("name")]
    public string Name { get; set; }
    
    [Column("pipeline")]
    public Guid? PipelineVersionId { get; set; }
    
    [Column("org")]
    public Guid? Organization { get; set; }

    [Column("variables", TypeName = "jsonb")]
    public Dictionary<string, string> Variables { get; set; } = new();
    
    [Column("source")]
    public string Source { get; set; }
}