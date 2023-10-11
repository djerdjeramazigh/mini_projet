using System;
using System.IO;
using DinkToPdf;

class Program
{
    static void Main()
    {
        // Parse the template
        string templateFilePath = "template.html";
        string htmlContent = File.ReadAllText(templateFilePath);

        // Generate PDF
        string pdfFilePath = "output.pdf";
        GeneratePdfFromHtml(htmlContent, pdfFilePath);

        Console.WriteLine($"PDF generated successfully at: {pdfFilePath}");
    }

    static void GeneratePdfFromHtml(string html, string pdfFilePath)
    {
        var converter = new BasicConverter(new PdfTools());

        var doc = new HtmlToPdfDocument
        {
            GlobalSettings = {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
            },
            Objects = {
                new ObjectSettings
                {
                    PagesCount = true,
                    HtmlContent = html,
                }
            }
        };

        var pdfBytes = converter.Convert(doc);
        File.WriteAllBytes(pdfFilePath, pdfBytes);
    }
}

