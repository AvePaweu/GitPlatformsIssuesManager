using Newtonsoft.Json;

namespace GitPlatformsIssuesManager.Library.Models;

public class GitLabProject
{
    [JsonProperty("id")]
    public long Id { get; set; }
    [JsonProperty("name")]
    public string? Name { get; set; }
}