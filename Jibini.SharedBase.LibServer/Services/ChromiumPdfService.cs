using PuppeteerSharp;

namespace Jibini.SharedBase.Util.Services;

/// <summary>
/// Provides PDf rendering for complex HTML layouts including Blazor components,
/// modern JavaScript, 2D canvases, and websockets.
/// </summary>
public class ChromiumPdfService
{
    /// <summary>
    /// Renders the provided static HTML or static HTML with script to PDF.
    /// </summary>
    public async Task<Stream> RenderPdfAsync(string html, bool isLandscape = false, int additionalDelay = 0)
    {
        using var browserFetcher = new BrowserFetcher();
        await browserFetcher.DownloadAsync(BrowserFetcher.DefaultChromiumRevision);

        using var browser = await Puppeteer.LaunchAsync(new()
        {
            Headless = true
        });

        using var page = await browser.NewPageAsync();
        await page.SetContentAsync(html);
        await Task.Delay(additionalDelay);

        return await page.PdfStreamAsync(new()
        {
            Landscape = isLandscape,
            MarginOptions = new()
            {
                Top = "0.35in",
                Bottom = "0.35in",
                Left = "0.35in",
                Right = "0.35in"
            }
        });
    }

    /// <summary>
    /// Renders the content at the provided location as the contents of a PDF.
    /// </summary>
    public async Task<Stream> RenderPdfAsync(Uri uri, bool isLandscape = false, int additionalDelay = 0)
    {
        using var browserFetcher = new BrowserFetcher();
        await browserFetcher.DownloadAsync(BrowserFetcher.DefaultChromiumRevision);

        using var browser = await Puppeteer.LaunchAsync(new()
        {
            Headless = true
        });

        using var page = await browser.NewPageAsync();
        await page.GoToAsync(uri.ToString());
        await Task.Delay(additionalDelay);

        return await page.PdfStreamAsync(new()
        {
            Landscape = isLandscape,
            MarginOptions = new()
            {
                Top = "0.35in",
                Bottom = "0.35in",
                Left = "0.35in",
                Right = "0.35in"
            }
        });
    }
}