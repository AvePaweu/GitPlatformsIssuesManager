using AutoMapper;
using GitPlatformsIssuesManager.Library.Dtos;
using GitPlatformsIssuesManager.Library.Models;
using GitPlatformsIssuesManager.Library.Models.PlatformConfig;
using System.Net.Http.Json;

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

    public async Task<GitIssue> AddIssue(AddIssueDto issue, string owner, string repo)
    {
        var projectId = await GetProjectIdByName(owner, repo);
        var gitLabIssue = _mapper.Map<GitLabIssue>(issue);
        var endpointUrl = GetApiEndpointByName("CreateAnIssue");
        string url = SetUrlParams(endpointUrl!.Url, owner, repo, projectId.Id, null);
        var response = await _httpClient.PostAsJsonAsync(url, gitLabIssue);
        if (response.IsSuccessStatusCode)
        {
            var createdIssue = await response.Content.ReadFromJsonAsync<GitHubIssue>();
            return _mapper.Map<GitIssue>(createdIssue);
        }
        return new GitIssue() { Name = "Unsuccessful attempt to add new issue" };
    }

    public async Task<GitIssue> CloseIssue(string owner, string repo, int number)
    {
        var issueToClose = await GetIssue(owner, repo, number);
        if (issueToClose is null || issueToClose.State == "closed") return new GitIssue { Name = "That issue is already closed!" };
        issueToClose.State = "closed";
        var editIssueDto = _mapper.Map<EditIssueDto>(issueToClose);
        return await ModifyIssue(editIssueDto, owner, repo, number);
    }

    public async Task<GitIssue> GetIssue(string owner, string repo, int number)
    {
        var projectId = await GetProjectIdByName(owner, repo);
        var endpointUrl = GetApiEndpointByName("GetAnIssue");
        string url = SetUrlParams(endpointUrl!.Url, owner, repo, projectId.Id, number);
        var response = await _httpClient.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            var gitHubIssue = await response.Content.ReadFromJsonAsync<GitLabIssue>();
            return _mapper.Map<GitIssue>(gitHubIssue);
        }
        return new GitIssue() { Name = "Issue not found" };
    }

    public async Task<List<GitIssue>> GetIssues(string owner, string repo)
    {
        var projectId = await GetProjectIdByName(owner, repo);
        var endpointUrl = GetApiEndpointByName("ListRepositoryIssues");
        string url = SetUrlParams(endpointUrl!.Url, "", "", projectId.Id, null);
        var response = await _httpClient.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            var issues = await response.Content.ReadFromJsonAsync<List<GitLabIssue>>();
            return _mapper.Map<List<GitIssue>>(issues);
        }
        return [];
    }

    public async Task<GitIssue> ModifyIssue(EditIssueDto issue, string owner, string repo, int number)
    {
        var projectId = await GetProjectIdByName(owner, repo);
        var gitLabIssue = _mapper.Map<GitLabIssue>(issue);
        var endpointUrl = GetApiEndpointByName("ModifyAnIssue");
        string url = SetUrlParams(endpointUrl!.Url, owner, repo, projectId.Id, number);
        if (gitLabIssue.State == "closed") url += "?state_event=close";
        var response = await _httpClient.PutAsJsonAsync(url, gitLabIssue);
        if (response.IsSuccessStatusCode)
        {
            var modifiedIssue = await response.Content.ReadFromJsonAsync<GitLabIssue>();
            return _mapper.Map<GitIssue>(modifiedIssue);
        }
        return new GitIssue { Name = "Unsuccessful attempt to modify an issue" };
    }

    public async Task<GitLabProject> GetProjectIdByName(string owner, string repo)
    {
        var endpointUrl = GetApiEndpointByName("FindRepositoryId");
        string url = SetUrlParams(endpointUrl!.Url, owner, repo, null, null);
        var response = await _httpClient.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            var project = await response.Content.ReadFromJsonAsync<List<GitLabProject>>();
            return project?.FirstOrDefault() ?? new GitLabProject();
        }
        return new GitLabProject() { Name = "Project not found" };
    }

    private ApiEndpoint? GetApiEndpointByName(string name) => _platformConfig.EndpointsUrls.FirstOrDefault(p => p.Name == name) ?? throw new KeyNotFoundException("URL for specified endpoint not found");

    private static string SetUrlParams(string url, string owner, string repo, long? projectId, long? number) =>
        url.Replace("{OWNER}", owner)
            .Replace("{PROJECT_ID}", projectId?.ToString() ?? "")
            .Replace("{REPO}", repo)
            .Replace("{ISSUE_NUMBER}", number?.ToString() ?? "");
}
