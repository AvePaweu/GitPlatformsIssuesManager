using Newtonsoft.Json;

namespace GitPlatformsIssuesManager.Library.Models;

public class GitLabIssue
{
    [JsonProperty("id")]
    public long? Id { get; set; }
    [JsonProperty("iid")]
    public long? Iid { get; set; }
    [JsonProperty("title")]
    public string? Title { get; set; }
    [JsonProperty("description")]
    public string? Description { get; set; }
    [JsonProperty("state")]
    public string? State { get; set; } = "opened";

    public GitLabIssue() { }
    public GitLabIssue(string title, string description) => (Title, Description) = (title, description);
    public GitLabIssue(string title, string description, string state) => (Title, Description, State) = (title, description, state);
}