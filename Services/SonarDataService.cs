using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.Text;
public class SonarDataService
{
    private readonly string _token;
    private readonly HttpClient _client;

    public SonarDataService(IConfiguration config)
    {
        _token = config["Sonar:Token"];
        _client = new HttpClient();
        var authToken = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_token}:"));
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);
    }

    public async Task<SonarReportDto> GetDashboardData(string projectKey)
    {
        var summary = await GetJson($"http://localhost:9000/api/measures/component?component={projectKey}&metricKeys=bugs,vulnerabilities,code_smells,coverage,duplicated_lines_density,ncloc");

        var issues = await GetJson($"http://localhost:9000/api/issues/search?componentKeys={projectKey}&ps=500");

        var qualityGate = await GetJson($"http://localhost:9000/api/qualitygates/project_status?projectKey={projectKey}");

        return new SonarReportDto
        {
            Summary = summary,
            Issues = issues,
            QualityGate = qualityGate
        };
    }

    private async Task<JObject> GetJson(string url)
    {
        var json = await _client.GetStringAsync(url);
        return JObject.Parse(json);
    }
}
