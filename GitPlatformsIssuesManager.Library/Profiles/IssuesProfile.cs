using AutoMapper;
using GitPlatformsIssuesManager.Library.Dtos;
using GitPlatformsIssuesManager.Library.Models;

namespace GitPlatformsIssuesManager.Library.Profiles;

public class IssuesProfile : Profile
{
    public IssuesProfile()
    {
        // mapping for GitHub issue
        CreateMap<GitIssue, GitHubIssue>()
            .ForMember(dest => dest.Number, opts => opts.MapFrom(src => src.Id))
            .ForMember(dest => dest.Title, opts => opts.MapFrom(src => src.Name))
            .ForMember(dest => dest.Body, opts => opts.MapFrom(src => src.Description));

        CreateMap<GitHubIssue, GitIssue>()
            .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Number))
            .ForMember(dest => dest.Name, opts => opts.MapFrom(src => src.Title))
            .ForMember(dest => dest.Description, opts => opts.MapFrom(src => src.Body));

        CreateMap<AddIssueDto, GitHubIssue>()
            .ForMember(dest => dest.Body, opts => opts.MapFrom(src => src.Description));

        CreateMap<EditIssueDto, GitHubIssue>()
            .ForMember(dest => dest.Body, opts => opts.MapFrom(src => src.Description));

        CreateMap<GitIssue, EditIssueDto>()
            .ForMember(dest => dest.Title, opts => opts.MapFrom(src => src.Name));

        //mapping for GitLab issue
    }
}
