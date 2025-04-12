using GitPlatformsIssuesManager.Library;
using GitPlatformsIssuesManager.Library.Models;
using GitPlatformsIssuesManager.Library.Models.PlatformConfig;
using AutoMapper;
using GitPlatformsIssuesManager.Library.Platforms;
using System.Net.Http.Headers;
using GitPlatformsIssuesManager.Library.Dtos;

namespace GitPlatformsIssuesManager.Tests;

public class GitLabPlatformTests
{
    private readonly string _platformName = "GitLab";
    private readonly string _defaultOwner = "pawelchatlas";
    private readonly string _defaultRepo = "test-project";
    private readonly long? _projectId = 68896883;
    private readonly int _issueId = 1;
    private readonly IMapper _mapper;

    [Fact]
    public void Get_PlatformConfig_CheckDefaultValues()
    {
        var platformConfig = new PlatformConfig(_platformName);
        Assert.Equal(platformConfig.DefaultOwner, _defaultOwner);
        Assert.Equal(platformConfig.DefaultRepo, _defaultRepo);
    }

    [Fact]
    public void Get_HttpClient_CheckIfContainsAuthorizationHeader()
    {
        var platformContext = new PlatformContext(_mapper, _platformName);
        var httpClient = platformContext.HttpClient;
        Assert.StartsWith(httpClient.DefaultRequestHeaders.Authorization!.Scheme, "Bearer");
    }

    [Fact]
    public async Task Get_Issues_CheckIfContainsResults()
    {
        var platformContext = new PlatformContext(_mapper, _platformName);
        var getIssues = await platformContext.GetIssues(_defaultOwner, _defaultRepo);
        Assert.NotEmpty(getIssues);
    }

    [Fact]
    public async Task Get_Issue_CheckIfHasItem()
    {
        var platformContext = new PlatformContext(_mapper, _platformName);
        var issue = await platformContext.GetIssue(_defaultOwner, _defaultRepo, _issueId);
        Assert.Equal(issue.IssueId, _issueId);
    }

    [Fact]
    public async Task Add_Issue_CheckIfIssueHasAdded()
    {
        var platformContext = new PlatformContext(_mapper, _platformName);
        var newIssue = new AddIssueDto { Title = "Test unit issue", Description = "Let's find it out!" };
        var addedIssue = await platformContext.AddIssue(newIssue, _defaultOwner, _defaultRepo);
        Assert.Equal(newIssue.Title, addedIssue.Name);
    }

    [Fact]
    public async Task Modify_Issue_CheckIfIssueHasModified()
    {
        var platformContext = new PlatformContext(_mapper, _platformName);
        var issueToModify = new EditIssueDto { Title = "Test modifying issue", Description = "Can we do it?" };
        var modifiedIssue = await platformContext.ModifyIssue(issueToModify, _defaultOwner, _defaultRepo, _issueId);
        Assert.Equal(issueToModify.Title, modifiedIssue.Name);
    }

    [Fact]
    public async Task Close_Issue_CheckIfIssueHasClosed()
    {
        var platformContext = new PlatformContext(_mapper, _platformName);
        var issueToClose = new EditIssueDto { State = "closed" };
        var closedIssue = await platformContext.ModifyIssue(issueToClose, _defaultOwner, _defaultRepo, _issueId);
        Assert.Equal(issueToClose.State, closedIssue.State);
    }
}
