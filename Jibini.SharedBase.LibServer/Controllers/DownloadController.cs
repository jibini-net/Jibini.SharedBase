using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;

namespace Jibini.SharedBase.Controllers;

/// <summary>
/// Allows registration and downloading of arbitrary data or local files through
/// an MVC conroller, such as a PDF report or CSV export file.
/// </summary>
public class DownloadController : Controller
{
    /// <summary>
    /// Contains data held until the user navigates to the download page. The
    /// user's download request must include the unique ID of the download.
    /// </summary>
    private static readonly IDictionary<Guid, (MemoryStream data, string name)> arbitraryDownloads =
        new ConcurrentDictionary<Guid, (MemoryStream data, string name)>();

    /// <summary>
    /// Amount of time in milliseconds to wait for the user to download.
    /// </summary>
    private const int DOWNLOAD_START_TIMEOUT = 10000;

    /// <summary>
    /// Error about failed request which doesn't reveal much about the file.
    /// </summary>
    private const string VAGUE_ERROR = "Unauthorized or invalid access to downloadable resource";

    /// <summary>
    /// Task to automatically remove and delete data which is abandoned.
    /// </summary>
    private static async Task DownloadTimeoutAsync(Guid key)
    {
        await Task.Delay(DOWNLOAD_START_TIMEOUT);

        if (arbitraryDownloads.Remove(key, out var removed))
            removed.data.Dispose();
    }

    /// <summary>
    /// Loads the provided data into a memory stream and stages it for download.
    /// Redirect the user to the downloads controller with the returned key.
    /// </summary>
    public static async Task<Guid> RegisterDownloadAsync(Stream data, string name)
    {
        var key = Guid.NewGuid();

        var stream = new MemoryStream();
        await data.CopyToAsync(stream);
        stream.Position = 0;

        // Store file info and schedule timeout for starting the download
        arbitraryDownloads[key] = (stream, name);
        _ = DownloadTimeoutAsync(key);

        return key;
    }

    /// <summary>
    /// Endpoint which allows downloading of stored data within the initial time
    /// period.
    /// </summary>
    [HttpGet("/download/{key:Guid}")]
    public IActionResult Index(Guid key)
    {
        if (!arbitraryDownloads.Remove(key, out var removed))
            return StatusCode(401, VAGUE_ERROR);

        return File(removed.data, "application/octet-stream", removed.name);
    }
}
