using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;

public class ScreenshotPdfService
{
    public byte[] GeneratePdfFromImage(byte[] imageBytes)
    {
        using var document = new PdfDocument();
        var page = document.AddPage();

        // PdfSharpCore requires a Func<Stream>, so wrap MemoryStream
        var image = XImage.FromStream(() => new MemoryStream(imageBytes));

        // Adjust PDF page size to match image size
        page.Width = image.PixelWidth;
        page.Height = image.PixelHeight;

        using var gfx = XGraphics.FromPdfPage(page);
        gfx.DrawImage(image, 0, 0, page.Width, page.Height);

        using var output = new MemoryStream();
        document.Save(output);
        return output.ToArray();
    }
}
