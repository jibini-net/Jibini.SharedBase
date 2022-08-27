using Microsoft.AspNetCore.Components;
using Winnovative;

namespace Jibini.SharedBase.Util.Services;

public class WinnovativePdfService
{
    private readonly NavigationManager nav;
    private readonly IConfiguration config;

    public WinnovativePdfService(NavigationManager nav, IConfiguration config)
    {
        this.nav = nav;
        this.config = config;
    }

    public async Task<Stream> RenderPdfAsync(string html, bool isLandscape = false, int additionalDelay = 0) =>
        await Task.Run(() =>
        {
            var toPdf = new HtmlToPdfConverter()
            {
                LicenseKey = config["Winnovative:LicenseKey"],
                ConversionDelay = (int)Math.Ceiling((decimal)additionalDelay / 1000),
            };

            toPdf.PdfDocumentOptions.PdfPageOrientation = isLandscape
                ? PdfPageOrientation.Landscape
                : PdfPageOrientation.Portrait;
            toPdf.PdfDocumentOptions.PdfPageSize = PdfPageSize.Letter;

            toPdf.PdfDocumentOptions.TopMargin = 25.2f;
            toPdf.PdfDocumentOptions.BottomMargin = 25.2f;
            toPdf.PdfDocumentOptions.LeftMargin = 25.2f;
            toPdf.PdfDocumentOptions.RightMargin = 25.2f;

            var result = new MemoryStream();
            toPdf.ConvertHtmlToStream(html, nav.BaseUri, result);
            result.Position = 0;

            return result;
        });

    public async Task<Stream> RenderPdfAsync(Uri uri, bool isLandscape = false, int additionalDelay = 0) =>
        await Task.Run(() =>
        {
            var toPdf = new HtmlToPdfConverter()
            {
                LicenseKey = config["Winnovative:LicenseKey"],
                ConversionDelay = (int)Math.Ceiling((decimal)additionalDelay / 1000),
            };

            toPdf.PdfDocumentOptions.PdfPageOrientation = isLandscape
                ? PdfPageOrientation.Landscape
                : PdfPageOrientation.Portrait;
            toPdf.PdfDocumentOptions.PdfPageSize = PdfPageSize.Letter;

            toPdf.PdfDocumentOptions.TopMargin = 25.2f;
            toPdf.PdfDocumentOptions.BottomMargin = 25.2f;
            toPdf.PdfDocumentOptions.LeftMargin = 25.2f;
            toPdf.PdfDocumentOptions.RightMargin = 25.2f;

            var result = new MemoryStream();
            toPdf.ConvertUrlToStream(uri.ToString(), result);
            result.Position = 0;

            return result;
        });
}