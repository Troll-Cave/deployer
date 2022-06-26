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
    public ApplicationVariables Variables { get; set; }
    
    [Column("source")]
    public string Source { get; set; }
}

public class ApplicationVariables
{
    public Dictionary<string, string> Variables { get; set; }

    public string? Get(string key)
    {
        if (this.Variables.ContainsKey(key))
        {
            return this.Variables[key];
        }
        else
        {
            return null;
        }
    }
}