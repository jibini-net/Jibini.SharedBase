using PuppeteerSharp;
using System;

namespace Jibini.SharedBase.Util.Services;

/// <summary>
/// Provides PDf rendering for complex HTML layouts including Blazor components,
/// modern JavaScript, 2D canvases, and websockets.
/// </summary>
public class ChromiumPdfService
{
    /// <summary>
    /// Dots per inch of the Chromium renderer, to calculate viewport size.
    /// </summary>
    public static readonly double DEFAULT_DPI = 96.0;

    private readonly IConfiguration config;

    public ChromiumPdfService(IConfiguration config)
    {
        this.config = config;
    }

    /// <summary>
    /// Shared render function for HTML and URI content to PDF.
    /// </summary>
    private async Task RenderPdfAsync(IBrowser browser, Stream result, bool isLandscape = false, int additionalDelay = 0, string? html = "", string? uri = "")
    {
        using var page = await browser.NewPageAsync();

        await page.SetViewportAsync(new()
        {
            Width = (int)Math.Round(DEFAULT_DPI * (isLandscape ? 11 : 8.5)),
            Height = (int)Math.Round(DEFAULT_DPI * (isLandscape ? 8.5 : 11)),
            DeviceScaleFactor = 8
        });

        if (!string.IsNullOrEmpty(html))
        {
            await page.SetContentAsync(html);
        } else if (!string.IsNullOrEmpty(uri))
        {
            await page.GoToAsync(uri);
        }

        await page.WaitForNetworkIdleAsync();
        await Task.Delay(additionalDelay);

        using var pdf = await page.PdfStreamAsync(new()
        {
            Width = "8.5in",
            Height = "11in",
            Landscape = isLandscape,
            MarginOptions = new()
            {
                Top = "0.35in",
                Bottom = "0.35in",
                Left = "0.35in",
                Right = "0.35in"
            }
        });

        await pdf.CopyToAsync(result);
        result.Position = 0;
    }

    /// <summary>
    /// Renders the provided static HTML or static HTML with script to PDF.
    /// </summary>
    public async Task<Stream> RenderPdfAsync(string html, bool isLandscape = false, int additionalDelay = 0)
    {
        using var browserFetcher = new BrowserFetcher();
        await browserFetcher.DownloadAsync(BrowserFetcher.DefaultChromiumRevision);

        using var browser = await Puppeteer.LaunchAsync(new()
        {
            Headless = true,
            IgnoreHTTPSErrors = config.GetValue<bool>("Chromium:IgnoreHttpsErrors")
        });

        var result = new MemoryStream();
        try
        {
            await RenderPdfAsync(browser, result, isLandscape, additionalDelay, html: html);
        } finally
        {
            await browser.CloseAsync();
        }
        return result;
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
            Headless = true,
            IgnoreHTTPSErrors = config.GetValue<bool>("Chromium:IgnoreHttpsErrors")
        });

        var result = new MemoryStream();
        try
        {
            await RenderPdfAsync(browser, result, isLandscape, additionalDelay, uri: uri.ToString());
        } finally
        {
            await browser.CloseAsync();
        }
        return result;
    }
}