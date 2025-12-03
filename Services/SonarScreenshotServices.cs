// using Microsoft.Playwright;

// public class SonarScreenshotServices
// {
//     public async Task<byte[]> CaptureSonarDashboard(string projectKey)
//     {
//         using var playwright = await Playwright.CreateAsync();
//         var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
//         {
//             Headless = true
//         });

//         var page = await browser.NewPageAsync(new BrowserNewPageOptions
//         {
//             ViewportSize = new() { Width = 1600, Height = 900 }
//         });

//         string url = $"http://localhost:9000/dashboard?id={projectKey}";
//         await page.GotoAsync(url, new PageGotoOptions { WaitUntil = WaitUntilState.NetworkIdle });

//         // Wait for dashboard widgets
//         await page.WaitForTimeoutAsync(2000);

//         // Full-page screenshot
//         var screenshotBytes = await page.ScreenshotAsync(new PageScreenshotOptions
//         {
//             FullPage = true
//         });

//         await browser.CloseAsync();
//         return screenshotBytes;
//     }
// }

using Microsoft.Playwright;

public class SonarScreenshotServices
{
    private readonly string _token;

    public SonarScreenshotServices(IConfiguration config)
    {
        _token = config["Sonar:Token"];
    }

    public async Task<byte[]> CaptureSonarDashboard(string projectKey)
    {
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = true
        });

        var page = await browser.NewPageAsync();

        string url = $"http://{_token}:@localhost:9000/dashboard?id={projectKey}";

        await page.GotoAsync(url, new PageGotoOptions { WaitUntil = WaitUntilState.NetworkIdle });

        // wait for dashboard widgets
        await page.WaitForTimeoutAsync(20000);

        return await page.ScreenshotAsync(new PageScreenshotOptions
        {
            FullPage = true
        });
    }
    
}

