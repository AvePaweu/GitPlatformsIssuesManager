using AutoMapper;
using GitPlatformsIssuesManager.Library.Dtos;
using GitPlatformsIssuesManager.Library.Models;
using GitPlatformsIssuesManager.Library.Models.PlatformConfig;

namespace GitPlatformsIssuesManager.Library.Platforms;

public class GitLabPlatform : IGitPlatform
{

    private readonly string _platformName;
    private readonly IMapper _mapper;
    private readonly PlatformConfig _platformConfig;
    private readonly HttpClient _httpClient;

    public GitLabPlatform() { }
    public GitLabPlatform(IMapper mapper) => _mapper = mapper;
    public GitLabPlatform(IMapper mapper, string platformName, PlatformConfig platformConfig, HttpClient httpClient) => (_mapper, _platformName, _platformConfig, _httpClient) = (mapper, platformName, platformConfig, httpClient);

    public Task<GitIssue> AddIssue(AddIssueDto issue, string owner, string repo)
    {
        throw new NotImplementedException();
    }

    public Task<GitIssue> CloseIssue(string owner, string repo, int number)
    {
        throw new NotImplementedException();
    }

    public Task<GitIssue> GetIssue(string owner, string repo, int number)
    {
        throw new NotImplementedException();
    }

    public Task<List<GitIssue>> GetIssues(string owner, string repo)
    {
        throw new NotImplementedException();
    }

    public Task<GitIssue> ModifyIssue(EditIssueDto issue, string owner, string repo, int number)
    {
        throw new NotImplementedException();
    }
}
