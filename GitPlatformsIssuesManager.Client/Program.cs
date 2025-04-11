using AutoMapper;
using GitPlatformsIssuesManager.Client.Services;
using GitPlatformsIssuesManager.Library.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Buffers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(opts => opts.SwaggerEndpoint("/openapi/v1.json", "OpenAPI v1"));
}

app.UseHttpsRedirection();

var mapper = app.Services.GetService<IMapper>();

// in the future: find any way to 'intelligent' pass IssuesService to DI container - this below does not work
//var issuesService = new IssuesService(mapper!);
//builder.Services.AddSingleton<IssuesService>(issuesService);


/*
 * ENDPOINTS
 */
app.MapGet("/{platform}/issues",
    async ([FromRoute] string platform, [FromQuery] string? owner, [FromQuery] string? repo) =>
    await new IssuesService(mapper!).GetIssues(platform, owner, repo))
.WithName("GetAllIssues");

app.MapGet("/{platform}/issues/{number}",
    async ([FromRoute] string platform, [FromRoute] int number, [FromQuery] string? owner, [FromQuery] string? repo) =>
    await new IssuesService(mapper!).GetIssue(platform, owner, repo, number))
.WithName("GetIssue");

app.MapPost("/{platform}/issue", 
    async ([FromRoute] string platform, [FromQuery] string? owner, [FromQuery] string? repo, [FromBody] AddIssueDto issue) => 
    await new IssuesService(mapper!).CreateIssue(platform, owner, repo, issue))
.WithName("CreateIssue");

app.MapPut("/{platform}/issue/{number}", 
    async ([FromRoute] string platform, [FromQuery] string? owner, [FromQuery] string? repo, [FromRoute] int number, [FromBody] EditIssueDto issue) => 
    await new IssuesService(mapper!).ModifyIssue(platform, owner, repo, number, issue))
.WithName("ModifyIssue");

app.MapPut("/{platform}/issue/close/{number}", 
    async([FromRoute] string platform, [FromQuery] string? owner, [FromQuery] string? repo, [FromRoute] int number) => 
    await new IssuesService(mapper!).CloseIssue(platform, owner, repo, number))
.WithName("CloseIssue");

app.Run();
