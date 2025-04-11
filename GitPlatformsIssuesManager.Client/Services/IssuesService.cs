using AutoMapper;
using GitPlatformsIssuesManager.Library.Dtos;
using GitPlatformsIssuesManager.Library.Models;
using GitPlatformsIssuesManager.Library.Platforms;

namespace GitPlatformsIssuesManager.Client.Services;

public class IssuesService
{
    private readonly IMapper _mapper;

    public IssuesService(IMapper mapper) => _mapper = mapper;

    public async Task<List<GitIssue>> GetIssues(string platform, string? owner, string? repo)
    {
        var (ctx, repoOwner, repoName) = PrepareRequest(platform, owner, repo);
        var issues = await ctx.GetIssues(repoOwner, repoName);
        return issues;
    }

    public async Task<GitIssue> GetIssue(string platform, string? owner, string? repo, int number)
    {
        var (ctx, repoOwner, repoName) = PrepareRequest(platform, owner, repo);
        var issue = await ctx.GetIssue(repoOwner, repoName, number);
        return issue;
    }

    public async Task<GitIssue> CreateIssue(string platform, string? owner, string? repo, AddIssueDto issue)
    {
        var (ctx, repoOwner, repoName) = PrepareRequest(platform, owner, repo);
        var createdIssue = await ctx.AddIssue(issue, repoOwner, repoName);
        return createdIssue;
    }

    public async Task<GitIssue> ModifyIssue(string platform, string? owner, string? repo, int number, EditIssueDto issue)
    {
        var (ctx, repoOwner, repoName) = PrepareRequest(platform, owner, repo);
        var modifiedIssue = await ctx.ModifyIssue(issue, repoOwner, repoName, number);
        return modifiedIssue;
    }

    public async Task<GitIssue> CloseIssue(string platform, string? owner, string? repo, int number)
    {
        var (ctx, repoOwner, repoName) = PrepareRequest(platform, owner, repo);
        var closedIssue = await ctx.CloseIssue(repoOwner, repoName, number);
        return closedIssue;
    }

    private PlatformContext SetPlatformContext(string platform) => new PlatformContext(_mapper, platform);

    private static string SetRepoOwner(PlatformContext ctx, string? owner) => string.IsNullOrEmpty(owner) ? ctx.PlatformConfig.DefaultOwner : owner;
    private static string SetRepoName(PlatformContext ctx, string? repo) => string.IsNullOrEmpty(repo) ? ctx.PlatformConfig.DefaultRepo : repo;

    private (PlatformContext context, string repoOwner, string repoName) PrepareRequest(string platform, string? owner, string? repo)
    {
        var ctx = SetPlatformContext(platform);
        string o = SetRepoOwner(ctx, owner);
        string r = SetRepoName(ctx, repo);
        return new(ctx, o, r);
    }
}
