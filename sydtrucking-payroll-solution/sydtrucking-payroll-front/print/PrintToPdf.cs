namespace sydtrucking_payroll_front.print
{
    using PdfSharp.Drawing;
    using PdfSharp.Pdf;

    public class PrintToPdf
    {
        private const string FONT = "Calibri";
        private PdfDocument _document;
        private PdfPage _page;
        private XGraphics _gfx;

        public string Filename { get; set; }

        public PrintToPdf(string filename)
        {
            _document = new PdfDocument();
            Filename = filename;
        }

        public void AddPage(PdfSharp.PageSize size)
        {
            _page = _document.AddPage();
            _page.Size = size;
            _gfx = XGraphics.FromPdfPage(_page);
        }

        public void DrawString(string text, FormatText format, double x, double y, double height, double width, XStringFormat location)
        {
            XFont font;

            switch (format)
            {
                case FormatText.Title:
                    font = new XFont(FONT, 18, XFontStyle.Bold);
                    break;
                case FormatText.Bold:
                    font = new XFont(FONT, 11, XFontStyle.Bold);
                    break;
                case FormatText.Italic:
                    font = new XFont(FONT, 11, XFontStyle.Italic);
                    break;
                case FormatText.BoldItalic:
                    font = new XFont(FONT, 11, XFontStyle.BoldItalic);
                    break;
                case FormatText.Regular:
                default:
                    font = new XFont(FONT, 11, XFontStyle.Regular);
                    break;
            }

            _gfx.DrawString(text, font, XBrushes.Black, new XRect(x, y, width, height), location);
        }

        public void DrawImage(string filename, double x, double y, double sizeInPercentage)
        {
            XImage image = XImage.FromFile(filename);
            _gfx.DrawImage(image, x, y, image.PixelWidth * sizeInPercentage, image.PixelHeight * sizeInPercentage);
        }

        public void Print()
        {
            _document.Info.Title = Filename;
            _document.Save(Filename);
        }   
    }

    public enum FormatText
    {
        Title = 1,
        Regular = 2,
        Bold = 3,
        Italic = 4,
        BoldItalic = 5
    }
}
