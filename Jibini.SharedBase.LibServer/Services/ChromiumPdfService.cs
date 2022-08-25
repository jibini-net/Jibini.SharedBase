using PuppeteerSharp;

namespace Jibini.SharedBase.Util.Services;

public class ChromiumPdfService
{
    public async Task<Stream> RenderPdf(string html, bool isLandscape = false, int additionalDelay = 0)
    {
        using var browserFetcher = new BrowserFetcher();
        await browserFetcher.DownloadAsync(BrowserFetcher.DefaultChromiumRevision);

        var browser = await Puppeteer.LaunchAsync(new()
        {
            Headless = true
        });

        var page = await browser.NewPageAsync();
        await page.SetContentAsync(html);
        await Task.Delay(additionalDelay);

        return await page.PdfStreamAsync(new()
        {
            Landscape = isLandscape
        });
    }

    public async Task<Stream> RenderPdf(Uri uri, bool isLandscape = false, int additionalDelay = 0)
    {
        using var browserFetcher = new BrowserFetcher();
        await browserFetcher.DownloadAsync(BrowserFetcher.DefaultChromiumRevision);

        var browser = await Puppeteer.LaunchAsync(new()
        {
            Headless = true
        });

        var page = await browser.NewPageAsync();
        await page.GoToAsync(uri.ToString());
        await Task.Delay(additionalDelay);

        return await page.PdfStreamAsync(new()
        {
            Landscape = isLandscape
        });
    }
}