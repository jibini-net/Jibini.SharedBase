namespace Jibini.SharedBase.Util.Services;

/// <summary>
/// Any service which can generate basic letter PDFs, whether via a local
/// library, 3rd party service, or external serverside application.
/// </summary>
public interface IPdfService
{
    /// <summary>
    /// Renders the provided static HTML or static HTML with script to PDF.
    /// </summary>
    Task<Stream> RenderPdfAsync(string html, bool isLandscape = false, int additionalDelay = 0);

    /// <summary>
    /// Renders the content at the provided location as the contents of a PDF.
    /// </summary>
    Task<Stream> RenderPdfAsync(Uri uri, bool isLandscape = false, int additionalDelay = 0);
}
