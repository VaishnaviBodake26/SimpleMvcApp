using Newtonsoft.Json.Linq;

public static class ReportBuilder
{
    public static string BuildHtml(SonarReportDto data)
    {
        var summary = data.Summary["component"]["measures"];

        string bugs = GetMetric(summary, "bugs");
        string vul = GetMetric(summary, "vulnerabilities");
        string smell = GetMetric(summary, "code_smells");
        string cov = GetMetric(summary, "coverage");
        string dup = GetMetric(summary, "duplicated_lines_density");
        string loc = GetMetric(summary, "ncloc");

        return $@"
<html>
<head>
<style>
body {{ font-family: Arial; }}
table {{ width: 100%; border-collapse: collapse; }}
td, th {{ border: 1px solid #ddd; padding: 8px; }}
th {{ background: #f2f2f2; }}
</style>
</head>
<body>

<h2>SonarQube Project Report</h2>

<h3>Summary Metrics</h3>
<table>
<tr><th>Metric</th><th>Value</th></tr>
<tr><td>Bugs</td><td>{bugs}</td></tr>
<tr><td>Vulnerabilities</td><td>{vul}</td></tr>
<tr><td>Code Smells</td><td>{smell}</td></tr>
<tr><td>Coverage</td><td>{cov}</td></tr>
<tr><td>Duplications</td><td>{dup}%</td></tr>
<tr><td>Lines of Code</td><td>{loc}</td></tr>
</table>

<h3>Quality Gate</h3>
<p>Status: <b>{data.QualityGate["projectStatus"]["status"]}</b></p>

<h3>Issues</h3>
<p>Total Issues: {data.Issues["total"]}</p>

</body>
</html>";
    }

    private static string GetMetric(JToken measures, string key)
    {
        return measures.FirstOrDefault(m => m["metric"].Value<string>() == key)?["value"]?.ToString() ?? "0";
    }
}
