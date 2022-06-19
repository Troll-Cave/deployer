using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using data.Models;

namespace data.DataModels;

[Table("job")]
public class JobDTO
{
    [Column("id"), Key()]
    public Guid ID { get; set; }
    
    /// <summary>
    /// An exact copy of the pipeline code
    /// </summary>
    [Column("code", TypeName = "jsonb")]
    public Pipeline Code { get; set; }

    /// <summary>
    /// KV for steps for data, not for use in POC.
    /// We'll save commiters for example here.
    /// </summary>
    [Column("metadata", TypeName = "jsonb")]
    public Dictionary<string, string> MetaData { get; set; } = new();

    /// <summary>
    /// KV mapping for step state
    /// </summary>
    [Column("step_state", TypeName = "jsonb")]
    public Dictionary<string, string> StepState { get; set; } = new();
    
    [Column("pipeline")]
    public Guid? PipelineVersionId { get; set; }
    
    [Column("application")]
    public Guid ApplicationId { get; set; }
    
    [Column("job_state")]
    public string JobState { get; set; }
    
    [Column("source_reference")]
    public string SourceReference { get; set; }
}