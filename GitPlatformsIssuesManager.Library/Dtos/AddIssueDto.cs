using System.ComponentModel.DataAnnotations;

namespace GitPlatformsIssuesManager.Library.Dtos;

public class AddIssueDto
{
    [Required]
    public required string Title { get; set; }
    public string? Description { get; set; }
}
