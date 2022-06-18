using data.Models;

namespace api.Models;

public class PipelineVersion
{
    public string Name { get; set; }
    public Guid Pipeline { get; set; }
    public Pipeline Code { get; set; }
    public Dictionary<string, string> Files { get; set; } = new();
}