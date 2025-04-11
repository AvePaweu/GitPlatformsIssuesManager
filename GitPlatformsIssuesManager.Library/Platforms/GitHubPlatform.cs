using AutoMapper;
using GitPlatformsIssuesManager.Library.Dtos;
using GitPlatformsIssuesManager.Library.Models;
using GitPlatformsIssuesManager.Library.Models.PlatformConfig;
using System.Net;
using System.Net.Http.Json;

namespace GitPlatformsIssuesManager.Library.Platforms;

public class GitHubPlatform : IGitPlatform
{
    private readonly string _platformName;
    private readonly PlatformConfig _platformConfig;
    private readonly HttpClient _httpClient;
    private readonly IMapper _mapper;

    public GitHubPlatform() { }
    public GitHubPlatform(IMapper mapper) => _mapper = mapper;
    public GitHubPlatform(IMapper mapper, string platformName, PlatformConfig platformConfig, HttpClient httpClient) => (_mapper, _platformName, _platformConfig, _httpClient) = (mapper, platformName, platformConfig, httpClient);

    public PlatformConfig PlatformConfig => _platformConfig;

    public async Task<GitIssue> AddIssue(AddIssueDto issue, string owner, string repo)
    {
        //without a header below, returns status code 403 FORBIDDEN
        _httpClient.DefaultRequestHeaders.Add("User-Agent", _platformConfig.DefaultRepo);
        var gitHubIssue = _mapper.Map<GitHubIssue>(issue);
        var endpointUrl = GetApiEndpointByName("CreateAnIssue");
        string url = SetUrlParams(endpointUrl!.Url, owner, repo);
        var response = await _httpClient.PostAsJsonAsync(url, gitHubIssue);
        if (response.StatusCode == HttpStatusCode.Created)
        {
            var createdIssue = await response.Content.ReadFromJsonAsync<GitHubIssue>();
            return _mapper.Map<GitIssue>(createdIssue);
        }
        return new GitIssue() { Name = "Unsuccessful attempt to add new issue" };
    }

    public async Task<GitIssue> CloseIssue(string owner, string repo, int number)
    {
        var issueToClose = await GetIssue(owner, repo, number);
        if (issueToClose is null || issueToClose.State == "closed") throw new Exception("That issue is already closed!");
        issueToClose.State = "closed";
        var editIssueDto = _mapper.Map<EditIssueDto>(issueToClose);
        return await ModifyIssue(editIssueDto, owner, repo, number);
    }

    public async Task<GitIssue> GetIssue(string owner, string repo, int number)
    {
        //without a header below, returns status code 403 FORBIDDEN
        _httpClient.DefaultRequestHeaders.Add("User-Agent", _platformConfig.DefaultRepo);
        var endpointUrl = GetApiEndpointByName("GetAnIssue");
        string url = SetUrlParams(endpointUrl!.Url, owner, repo, number);
        var response = await _httpClient.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            var gitHubIssue = await response.Content.ReadFromJsonAsync<GitHubIssue>();
            return _mapper.Map<GitIssue>(gitHubIssue);
        }
        return new GitIssue() { Name = "An issue is not found" };
    }

    public async Task<List<GitIssue>> GetIssues(string owner, string repo)
    {
        //without a header below, returns status code 403 FORBIDDEN
        _httpClient.DefaultRequestHeaders.Add("User-Agent", _platformConfig.DefaultRepo);
        var endpointUrl = GetApiEndpointByName("ListRepositoryIssues");
        string url = SetUrlParams(endpointUrl!.Url, owner, repo);
        var response = await _httpClient.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            var issues = await response.Content.ReadFromJsonAsync<List<GitHubIssue>>();
            return _mapper.Map<List<GitIssue>>(issues);
        }
        return null;
    }

    public async Task<GitIssue> ModifyIssue(EditIssueDto issue, string owner, string repo, int number)
    {
        //without a header below, returns status code 403 FORBIDDEN
        _httpClient.DefaultRequestHeaders.Add("User-Agent", _platformConfig.DefaultRepo);
        var gitHubIssue = _mapper.Map<GitHubIssue>(issue);
        var endpointUrl = GetApiEndpointByName("ModifyAnIssue");
        string url = SetUrlParams(endpointUrl!.Url, owner, repo, number);
        var response = await _httpClient.PatchAsJsonAsync(url, gitHubIssue);
        if (response.IsSuccessStatusCode)
        {
            var modifiedIssue = await response.Content.ReadFromJsonAsync<GitHubIssue>();
            return _mapper.Map<GitIssue>(modifiedIssue);
        }
        return null;
    }

    /// <summary>
    /// Finds an ApiEndpoint object by its name
    /// </summary>
    /// <param name="name">Endpoint name</param>
    /// <returns>An ApiEndpoint object</returns>
    /// <exception cref="KeyNotFoundException">Thrown when a key for selected endpoint is not found</exception>
    private ApiEndpoint? GetApiEndpointByName(string name) => _platformConfig.EndpointsUrls.FirstOrDefault(p => p.Name == name) ?? throw new KeyNotFoundException("URL for specified endpoint not found");

    private static string SetUrlParams(string url, string owner, string repo) => url.Replace("{OWNER}", owner).Replace("{REPO}", repo);
    private static string SetUrlParams(string url, string owner, string repo, int number) => url.Replace("{OWNER}", owner).Replace("{REPO}", repo).Replace("{ISSUE_NUMBER}", number.ToString());
}