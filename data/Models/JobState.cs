namespace data.Models;

/// <summary>
/// State management for jobs
/// TODO: make this a finite state machine
/// </summary>
public class JobState
{
    public string State { get; set; }
    public Dictionary<string, string> StepState { get; set; }
    public Dictionary<string, string> Metadata { get; set; }
}