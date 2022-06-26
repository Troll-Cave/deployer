using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Nodes;
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

    [Column("pipeline")]
    public Guid? PipelineVersionId { get; set; }
    
    [Column("application")]
    public Guid ApplicationId { get; set; }
    
    [Column("job_state", TypeName = "jsonb")]
    public JobState JobState { get; set; }
    
    [Column("source_reference")]
    public string SourceReference { get; set; }
}