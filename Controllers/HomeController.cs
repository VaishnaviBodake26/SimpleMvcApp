using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Diagnostics;

namespace SimpleMvcApp.Controllers
{
    public class HomeController : Controller
    {

        // private readonly SonarqubeServices _sonar;
        vaishnavi bodake 
        escape coding!!!
        return View();

        
    }

        // public HomeController(SonarqubeServices sonar)
        // {
        //     _sonar = sonar;
        // }
        public async Task<IActionResult> Index()
        {
         var process = new Process();
process.StartInfo.FileName = @"C:\\Users\\Lenovo\\Downloads\\sonarqube-25.11.0.114957\\sonarqube-25.11.0.114957\\bin\\windows-x86-64\\StartSonar.bat";
process.StartInfo.WorkingDirectory = @"C:\\Users\\Lenovo\\Downloads\\sonarqube-25.11.0.114957\\sonarqube-25.11.0.114957\\bin\\windows-x86-64";
process.StartInfo.CreateNoWindow = false;
process.StartInfo.UseShellExecute = true;
process.Start();
  
        return View();
        }
    }
}
