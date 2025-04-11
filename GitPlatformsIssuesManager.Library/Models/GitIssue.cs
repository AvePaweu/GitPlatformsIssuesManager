namespace GitPlatformsIssuesManager.Library.Models;

public class GitIssue
{
    public long? Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? State { get; set; } = "open";

    public GitIssue() { }
    public GitIssue(string name, string description) => (Name, Description) = (name, description);
    public GitIssue(string name, string description, string state) => (Name, Description, State) = (name, description, state);
}
