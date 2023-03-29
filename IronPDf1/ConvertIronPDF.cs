using IronPdf;

namespace IronPDf1
{
    public static class ConvertIronPDF
    {

        public static void ConvertToIronPDF()
        {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            ChromePdfRenderer renderer = new ChromePdfRenderer();
            PdfDocument pdf = renderer.RenderUrlAsPdf("https://www.wikipedia.org/");
            pdf.SaveAs("wiki.pdf");
            Console.WriteLine($"{watch.ElapsedMilliseconds} ms");
        }

        public static void ConvertWithHeaderAndFooter()
        {
            var watch = new System.Diagnostics.Stopwatch();
            Console.WriteLine($"Inicio ejecucion ironPdf");
            watch.Start();
            ChromePdfRenderer renderer = new ChromePdfRenderer();
            // Build a footer using html to style the text
            // mergeable fields are:
            // {page} {total-pages} {url} {date} {time} {html-title} & {pdf-title}
            renderer.RenderingOptions.HtmlFooter = new HtmlHeaderFooter()
            {
                MaxHeight = 30,
                HtmlFragment = "<center><i>{page} of {total-pages}<i></center>",
                DrawDividerLine = true
            };
            // Build a header using an image asset
            // Note the use of BaseUrl to set a relative path to the assets
            renderer.RenderingOptions.HtmlHeader = new HtmlHeaderFooter()
            {
                MaxHeight = 30,
                HtmlFragment = "<img src='logo.jpg'>",
                BaseUrl = new Uri(@"C:\\test\\y2urwjovx1k8.png").AbsoluteUri
            };
            PdfDocument pdf = renderer.RenderHtmlAsPdf("<h1>Hello World<h1>");
            pdf.SaveAs("html-string.pdf");
            Console.WriteLine($"iron pdf {watch.ElapsedMilliseconds}ms");
        }
    }
}