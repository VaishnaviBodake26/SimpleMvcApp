
using Newtonsoft.Json.Linq;
public class SonarReportDto
{
    public JObject Summary { get; set; }
    public JObject Issues { get; set; }
    public JObject QualityGate { get; set; }
}
