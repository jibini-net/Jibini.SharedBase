using Jibini.SharedBase.Controllers;
using Microsoft.JSInterop;

namespace Jibini.SharedBase.Util.Services;

/// <summary>
/// Provides file downloads from within Blazor components. Requires a popup-type
/// redirect to a new tab. Browsers complain on the first download.
/// </summary>
public class DownloadService
{
    private readonly IJSRuntime js;

    public DownloadService(IJSRuntime js)
    {
        this.js = js;
    }

    /// <summary>
    /// Registers the provided file content and redirects the user to download.
    /// </summary>
    public async Task DownloadAsync(Stream data, string fileName)
    {
        var key = await DownloadController.RegisterDownloadAsync(data, fileName);
        //await js.InvokeVoidAsync("open", , "download-portal");
        await js.InvokeVoidAsync("IframeDownloadInterop.triggerDownload", $"download/{key}");
    }

    /// <summary>
    /// Registers the provided file content and redirects the user to download.
    /// </summary>
    public async Task DownloadAsync(string fileName)
    {
        using var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
        await DownloadAsync(stream, Path.GetFileName(fileName));
    }
}
