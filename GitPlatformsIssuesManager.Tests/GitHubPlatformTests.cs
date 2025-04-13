using AutoMapper;
using GitPlatformsIssuesManager.Library.Dtos;
using GitPlatformsIssuesManager.Library.Models.PlatformConfig;
using GitPlatformsIssuesManager.Library.Platforms;
using GitPlatformsIssuesManager.Library.Profiles;
using RichardSzalay.MockHttp;

namespace GitPlatformsIssuesManager.Tests;

public class GitHubPlatformTests
{
    private readonly string _platformName = "GitHub";
    private readonly string _defaultOwner = "AvePaweu";
    private readonly string _defaultRepo = "cv-pawel-chatlas";
    private readonly int _issueId = 1;
    private readonly IMapper _mapper;
    private PlatformConfig _platformConfig;

    public GitHubPlatformTests()
    {
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new IssuesProfile())));
        _platformConfig = new PlatformConfig(_platformName);
    }

    [Fact]
    public void Get_PlatformConfig_CheckDefaultValues()
    {
        Assert.Equal(_platformConfig.DefaultOwner, _defaultOwner);
        Assert.Equal(_platformConfig.DefaultRepo, _defaultRepo);
    }

    [Fact]
    public void Get_HttpClient_CheckIfContainsAuthorizationHeader()
    {
        var platformContext = new PlatformContext(_mapper, _platformName);
        var mockHttp = new MockHttpMessageHandler();
        platformContext.HttpClient = SetupClient(mockHttp, _platformConfig);
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
        var issueToClose = new EditIssueDto { Title = "Test modifying issue", State = "closed" };
        var closedIssue = await platformContext.ModifyIssue(issueToClose, _defaultOwner, _defaultRepo, _issueId);
        Assert.Equal(issueToClose.State, closedIssue.State);
    }

    private HttpClient SetupClient(MockHttpMessageHandler mockHttpMessageHandler, PlatformConfig platformConfig)
    {
        var client = new HttpClient(mockHttpMessageHandler);
        client.BaseAddress = new Uri(platformConfig.BaseUrl);
        foreach (var header in platformConfig.RequestHeaders)
        {
            client.DefaultRequestHeaders.Add(header.Name, header.Value);
        }
        return client;
    }
}
