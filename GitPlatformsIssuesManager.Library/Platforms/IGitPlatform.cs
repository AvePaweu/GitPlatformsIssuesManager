using GitPlatformsIssuesManager.Library.Dtos;
using GitPlatformsIssuesManager.Library.Models;

namespace GitPlatformsIssuesManager.Library.Platforms;

public interface IGitPlatform
{
    Task<List<GitIssue>> GetIssues(string owner, string repo);
    Task<GitIssue> GetIssue(string owner, string repo, int number);
    Task<GitIssue> AddIssue(AddIssueDto issue, string owner, string repo);
    Task<GitIssue> ModifyIssue(EditIssueDto issue, string owner, string repo, int number);
    Task<GitIssue> CloseIssue(string owner, string repo, int number);
}
