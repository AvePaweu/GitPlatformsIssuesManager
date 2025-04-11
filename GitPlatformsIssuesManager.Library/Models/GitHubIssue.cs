using Newtonsoft.Json;

namespace GitPlatformsIssuesManager.Library.Models;

public class GitHubIssue
{
    [JsonProperty("id")]
    public long? Id { get; set; }
    [JsonProperty("number")]
    public long? Number { get; set; }
    [JsonProperty("title")]
    public string? Title { get; set; }
    [JsonProperty("body")]
    public string? Body { get; set; }
    [JsonProperty("state")]
    public string? State { get; set; } = "open";

    public GitHubIssue() { }
    public GitHubIssue(string title, string body) => (Title, Body) = (title, body);
    public GitHubIssue(string title, string body, string state) => (Title, Body, State) = (title, body, state);

}
