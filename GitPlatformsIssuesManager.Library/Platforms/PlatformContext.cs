using AutoMapper;
using GitPlatformsIssuesManager.Library.Dtos;
using GitPlatformsIssuesManager.Library.Models;
using GitPlatformsIssuesManager.Library.Models.PlatformConfig;

namespace GitPlatformsIssuesManager.Library.Platforms;

/// <summary>
/// An attempt to implement the 'Strategy' behavioral design pattern
/// </summary>
public class PlatformContext
{
    private readonly IGitPlatform _gitPlatform;
    private readonly IMapper _mapper;
    private PlatformConfig _platformConfig;

    public PlatformConfig PlatformConfig => _platformConfig;
    public HttpClient HttpClient => EstablishHttpClient(_platformConfig);

    public PlatformContext() { }

    public PlatformContext(IMapper mapper, string platformName)
    {
        _mapper = mapper;
        _platformConfig = new PlatformConfig(platformName);
        _gitPlatform = platformName switch
        {
            "GitHub" => new GitHubPlatform(mapper, platformName, _platformConfig, EstablishHttpClient(_platformConfig)),
            "GitLab" => new GitLabPlatform(mapper, platformName, _platformConfig, EstablishHttpClient(_platformConfig)),
            _ => throw new NotImplementedException("Selected platform hasn't implemented yet!")
        };
        Console.WriteLine(_gitPlatform.GetType());
    }

    // proxying of IGitPlatform interface
    public Task<GitIssue> AddIssue(AddIssueDto issue, string owner, string repo) => _gitPlatform.AddIssue(issue, owner, repo);

    public Task<List<GitIssue>> GetIssues(string owner, string repo) => _gitPlatform.GetIssues(owner, repo);

    public Task<GitIssue> GetIssue(string owner, string repo, int number) => _gitPlatform.GetIssue(owner, repo, number);

    public Task<GitIssue> ModifyIssue(EditIssueDto issue, string owner, string repo, int number) => _gitPlatform.ModifyIssue(issue, owner, repo, number);

    public Task<GitIssue> CloseIssue(string owner, string repo, int number) => _gitPlatform.CloseIssue(owner, repo, number);

    /// <summary>
    /// Initialize the HttpClient instance by values defined for specified Platform
    /// </summary>
    /// <param name="platformConfig"></param>
    /// <returns>Object of HttpClient class with initialized BaseAddress and headers (except User-Agent)</returns>
    public HttpClient EstablishHttpClient(PlatformConfig platformConfig)
    {
        if (platformConfig is null) return new HttpClient();
        var httpClient = new HttpClient() { BaseAddress = new Uri(platformConfig.BaseUrl) };
        foreach (var header in platformConfig.RequestHeaders)
        {
            httpClient.DefaultRequestHeaders.Add(header.Name, header.Value);
        }
        return httpClient;
    }
}
