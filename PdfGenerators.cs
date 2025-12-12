using DinkToPdf;
using DinkToPdf.Contracts;

public static class PdfGenerators
{
    public static byte[] ConvertHtmlToPdf(string html)
    {
        var globalSettings = new GlobalSettings
        {
            ColorMode = ColorMode.Color,
            Orientation = Orientation.Portrait,
            PaperSize = PaperKind.A4
        };

        var objectSettings = new ObjectSettings
        {
            HtmlContent = html,
            WebSettings = { DefaultEncoding = "utf-8" }
        };

        var pdf = new HtmlToPdfDocument()
        {
            GlobalSettings = globalSettings,
            Objects = { objectSettings }
        };

        var converter = new SynchronizedConverter(new PdfTools());
        return converter.Convert(pdf);
    }
}
