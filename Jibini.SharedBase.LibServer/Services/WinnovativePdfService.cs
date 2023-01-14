﻿using Microsoft.AspNetCore.Components;
using Winnovative;

namespace Jibini.SharedBase.Util.Services;

/// <summary>
/// A PDF rendering service with fewer moving parts, using Winnovative's PDF
/// libraries to render HTML with moderate script support to a document.
/// </summary>
public class WinnovativePdfService
{
    private readonly NavigationManager nav;
    private readonly IConfiguration config;

    public WinnovativePdfService(NavigationManager nav, IConfiguration config)
    {
        this.nav = nav;
        this.config = config;
    }

    /// <summary>
    /// Renders the provided static HTML or static HTML with script to PDF.
    /// </summary>
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

    /// <summary>
    /// Renders the content at the provided location as the contents of a PDF.
    /// </summary>
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