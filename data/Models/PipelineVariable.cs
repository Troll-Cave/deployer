namespace data.Models;

public class PipelineVariable
{
    public string Name { get; set; }
    public string Type { get; set; }
    public string Scope { get; set; }
    public bool Private { get; set; }
}