using System.ComponentModel;
using System.Text.Json.Serialization;

namespace data.Models;

/// <summary>
/// This is the overall pipeline model used by the other systems.
/// </summary>
public class Pipeline
{
    [JsonPropertyName("variables")]
    public List<PipelineVariable> Variables { get; set; } = new();
    
    [JsonPropertyName("steps")]
    public List<PipelineStep> Steps { get; set; } = new();
    
    [JsonPropertyName("files")]
    public List<PipelineFile> Files { get; set; } = new();
    
    [JsonPropertyName("flow")]
    public List<PipelineFlow> Flow { get; set; } = new();
}

public class PipelineFlow
{
    [JsonPropertyName("step")]
    public string Step { get; set; }
    
    [JsonPropertyName("dependsOn")]
    public List<string> DependsOn { get; set; } = new();
    
    [JsonPropertyName("locals")]
    public Dictionary<string, string> Locals { get; set; } = new();
}

public class PipelineFile
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("canReplace")]
    public bool CanReplace { get; set; } = true;
    
    [JsonPropertyName("isBinary")]
    public bool IsBinary { get; set; }
    
    [JsonPropertyName("location")]
    public string Location { get; set; }
}

public class PipelineStep
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("type")]
    public string Type { get; set; }
    
    [JsonPropertyName("persist")]
    public bool Persist { get; set; }
    
    [JsonPropertyName("locals")]
    public List<PipelineStepLocal> Locals { get; set; } = new();
    
    [JsonPropertyName("options")]
    public Dictionary<string, string> Options { get; set; } = new();
    
    [JsonPropertyName("actions")]
    public List<PipelineStepAction> Actions { get; set; } = new();
}

public class PipelineStepAction
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("command")]
    public string Command { get; set; }
    
    /// <summary>
    /// Used by certain commands, not all
    /// </summary>
    [JsonPropertyName("stage")]
    public string Stage { get; set; }
}

public class PipelineStepLocal
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("type")]
    public string Type { get; set; }
}

public class PipelineVariable
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("type")]
    public string Type { get; set; } = "string";

    [JsonPropertyName("scope")]
    public string Scope { get; set; } = "local";

    [JsonPropertyName("secret")]
    public bool Secret { get; set; }
}
