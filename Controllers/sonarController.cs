using Microsoft.AspNetCore.Mvc;
using SimpleMvcApp.Services;
using Microsoft.Playwright;
using System.Text.Json;


namespace SimpleMvcApp.Controllers
{
    public class sonarController : Controller
    {
        private readonly SonarAutomationServices _service;
        private readonly string _ProjectKey;
        private readonly SonarDataService _sonarDataService;
        private readonly ScreenshotPdfService _pdfService;
        public sonarController(SonarAutomationServices service,IConfiguration config,SonarDataService sonarDataService,ScreenshotPdfService pdfService)
        {
            _service = service;
            _ProjectKey = config["Sonar:ProjectKey"]!;
            _sonarDataService=sonarDataService;
            _pdfService=pdfService;
        }

        [HttpGet("/run-analysis")]
        public async Task<IActionResult> RunAnalysis()
        {
           // console.log("logg in controller");
            await _service.RunFullAnalysis();
            return Redirect($"http://localhost:9000/dashboard?id={_ProjectKey}");
        }



 [HttpGet("download-report")]
public IActionResult DownloadReport()
{
    string html = "<h1>SonarQube Report</h1><p>Your metrics go here...</p>";

    byte[] pdfBytes = PdfGenerators.ConvertHtmlToPdf(html);

    return File(pdfBytes, "application/pdf", "SonarReport.pdf");
}



//         [HttpGet("/download")]
// public async Task<IActionResult> DownloadSonarDashboard()
// {
//     string projectKey = "Ctpl-Code-Reviewer";

//     var screenshot = await _screenshotService.CaptureSonarDashboard(projectKey);
//     var pdfBytes = _pdfService.GeneratePdfFromImage(screenshot);

//     return File(pdfBytes, "application/pdf", $"{projectKey}_report.pdf");
// }



    }
}
