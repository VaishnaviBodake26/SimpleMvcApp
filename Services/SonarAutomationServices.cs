// using System.Diagnostics;

// namespace SimpleMvcApp.Services
// {
//     public class SonarAutomationServices
//     {
//         private readonly string _scannerPath;
//         private readonly string _projectPath;
//         private readonly string _token;
//         private readonly string _sonarHost;

//         public SonarAutomationServices(IConfiguration config)
//         {
//             _scannerPath = config["Sonar:ScannerPath"]!;
//             _projectPath = config["Sonar:ProjectPath"]!;
//             _token = config["Sonar:Token"]!;
//             _sonarHost = config["Sonar:Host"]!;
//         }

//         private async Task RunProcess(string file, string args, string? workingDir = null)
//         {
//             var psi = new ProcessStartInfo
//             {
//                 FileName = file,
//                 Arguments = args,
//                 WorkingDirectory = workingDir ?? _projectPath,
//                 UseShellExecute = false,
//                 RedirectStandardOutput = true,
//                 RedirectStandardError = true
//             };

//             var process = Process.Start(psi);

//             if (process == null)
//                 throw new Exception("Process failed to start.");

//             await process.WaitForExitAsync();
//         }

// // dotnet sonarscanner begin /k:"Ctpl-Code-Reviewer" /d:sonar.host.url="http://localhost:9000"  /d:sonar.token="squ_a00bfb402df247cd56b224fd94fae4099faef456"
// // dotnet sonarscanner end /d:sonar.token="squ_a00bfb402df247cd56b224fd94fae4099faef456"
//         // public async Task RunFullAnalysis()
//         // {
//         //     // 1️⃣ BEGIN ANALYSIS
//         //     await RunProcess(
//         //         _scannerPath,
//         //         $"begin /k:\"MyProjectKey\" /d:sonar.host.url=\"{_sonarHost}\" /d:sonar.login=\"{_token}\""
//         //     );

//         //     // 2️⃣ BUILD THE PROJECT
//         //     await RunProcess("dotnet", "build");

//         //     // 3️⃣ END ANALYSIS
//         //     await RunProcess(
//         //         _scannerPath,
//         //         $"end /d:sonar.token=\"{_token}\""
//         //     );
//         // }
//         public async Task RunFullAnalysis()
// {
//     Console.WriteLine("runprocess --");
//     // 1️⃣ BEGIN ANALYSIS
//     await RunProcess(
//         "dotnet",
//         $"sonarscanner begin /k:\"SimpleMvcApp\" /d:sonar.host.url=\"{_sonarHost}\" /d:sonar.token=\"{_token}\""
//     );
//     Console.WriteLine("runprocess build--");

//     // 2️⃣ BUILD
//     await RunProcess("dotnet", "build");

//     Console.WriteLine("runprocess end --");

//     // 3️⃣ END ANALYSIS
//     await RunProcess(
//         "dotnet",
//         $"sonarscanner end /d:sonar.token=\"{_token}\""
//     );
// }

//     }
// }


using System.Diagnostics;

namespace SimpleMvcApp.Services
{
    public class SonarAutomationServices
    {
        private readonly string _projectPath;
        private readonly string _token;
        private readonly string _sonarHost;
        private readonly string _ProjectKey;


        public SonarAutomationServices(IConfiguration config)
        {
            _projectPath = config["Sonar:ProjectPath"]!;
            _token = config["Sonar:Token"]!;
            _sonarHost = config["Sonar:Host"]!;
            _ProjectKey = config["Sonar:ProjectKey"]!;
        }

        private async Task Run(string file, string args)
        {
            var psi = new ProcessStartInfo
            {
                FileName = file,
                Arguments = args,
                WorkingDirectory = _projectPath,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            // IMPORTANT: give full PATH, otherwise dotnet build gets stuck
            psi.EnvironmentVariables["PATH"] = Environment.GetEnvironmentVariable("PATH")!;

            var process = new Process { StartInfo = psi };
            process.Start();

            // Read live output (prevents deadlock)
            _ = Task.Run(() =>
            {
                while (!process.StandardOutput.EndOfStream)
                    Console.WriteLine(process.StandardOutput.ReadLine());
            });

            _ = Task.Run(() =>
            {
                while (!process.StandardError.EndOfStream)
                    Console.WriteLine(process.StandardError.ReadLine());
            });

            await process.WaitForExitAsync();

            if (process.ExitCode != 0)
                throw new Exception($"Command failed: {file} {args}");
        }

        public async Task RunFullAnalysis()
        {
            // 1️⃣ BEGIN ANALYSIS
            await Run("dotnet",
               // $"sonarscanner begin /k:\"Ctpl-Code-Reviewer\" /d:sonar.host.url=\"{_sonarHost}\" /d:sonar.token=\"{_token}\"");
            $"sonarscanner begin /k:\"{_ProjectKey}\" /d:sonar.host.url=\"{_sonarHost}\" /d:sonar.token=\"{_token}\"");


            // 2️⃣ BUILD PROJECT
            await Run("dotnet", "build --no-incremental");

            // 3️⃣ END ANALYSIS
            await Run("dotnet",
                $"sonarscanner end /d:sonar.token=\"{_token}\"");
        }
    }
}

