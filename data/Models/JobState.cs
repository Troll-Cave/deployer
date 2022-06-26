namespace data.Models;

public class JobState
{
    public string State { get; set; }
    public Dictionary<string, string> StepState { get; set; }
    public Dictionary<string, string> Metadata { get; set; }
}