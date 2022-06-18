namespace data.Models;

/// <summary>
/// This is the overall pipeline model used by the other systems.
/// </summary>
public class Pipeline
{
    public List<PipelineVariable> Variables { get; set; } = new();
    public List<PipelineStep> Steps { get; set; } = new();
    public List<PipelineFile> Files { get; set; } = new();
    public List<PipelineFlow> Flow { get; set; } = new();
}

public class PipelineFlow
{
    public string Step { get; set; }
    public List<string> DependsOn { get; set; } = new();
    public Dictionary<string, string> Locals { get; set; } = new();
}

public class PipelineFile
{
    public string Name { get; set; }
    public bool CanReplace { get; set; } = true;
    public bool IsBinary { get; set; }
    public string Location { get; set; }
}

public class PipelineStep
{
    public string Name { get; set; }
    public string Type { get; set; }
    public bool Persist { get; set; }
    public List<PipelineStepLocal> Locals { get; set; } = new();
    public Dictionary<string, string> Options { get; set; } = new();
    public List<PipelineStepAction> Actions { get; set; } = new();
}

public class PipelineStepAction
{
    public string Name { get; set; }
    public string Command { get; set; }
    
    /// <summary>
    /// Used by certain commands, not all
    /// </summary>
    public string Stage { get; set; }
}

public class PipelineStepLocal
{
    public string Name { get; set; }
    public string Type { get; set; }
}

public class PipelineVariable
{
    public string Name { get; set; }
    public string Type { get; set; }
    public string Scope { get; set; }
    public bool Secret { get; set; }
}
