using Newtonsoft.Json;

namespace GitPlatformsIssuesManager.Library.Models.PlatformConfig;

public class PlatformConfig
{
    public string PlatformName { get; set; }
    public string BaseUrl { get; set; }
    public string DefaultOwner { get; set; }
    public string DefaultRepo { get; set; }
    public List<RequestHeader> RequestHeaders { get; set; }
    public List<ApiEndpoint> EndpointsUrls { get; set; }

    public PlatformConfig() { }

    public PlatformConfig(string platformName)
    {
        if (File.Exists("platforms.json"))
        {
            var configs = JsonConvert.DeserializeObject<List<PlatformConfig>>(File.ReadAllText("platforms.json"));
            if (configs?.Any(p => p.PlatformName == platformName) == true)
            {
                var config = configs.FirstOrDefault(p => p.PlatformName == platformName);
                PlatformName = config.PlatformName;
                BaseUrl = config.BaseUrl;
                DefaultOwner = config.DefaultOwner;
                DefaultRepo = config.DefaultRepo;
                RequestHeaders = config.RequestHeaders;
                EndpointsUrls = config.EndpointsUrls;
            }
            else throw new NotImplementedException("Selected platform hasn't already implemented!");
        }
        else throw new FileNotFoundException("Configuration file not found!");
    }
}
