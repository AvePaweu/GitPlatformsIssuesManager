using System.Text.Json.Serialization;

namespace GitPlatformsIssuesManager.Library.Dtos;

public class EditIssueDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    [JsonIgnore]
    public string State { get; set; } = "open";
}
