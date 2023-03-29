using iText.Html2pdf;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Events;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Action;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Layout;
using iText.Layout.Properties;
using iText.Layout.Renderer;

namespace IronPDF
{
    public class ItextPDF
    {
        public void ConvertToPdfItextSharp()
        {
            Console.WriteLine("Inicio ejecucion itext7");
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            PdfWriter writer = new PdfWriter("C:\\test\\img.pdf");
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf, PageSize.A4);
            Console.WriteLine("Inicio ejecucon itexsharp");
            Image img = new Image(ImageDataFactory
          .Create("C:\\test\\y2urwjovx1k8.png"))
          .SetTextAlignment(TextAlignment.CENTER);
            HeaderEventHandle handle = new HeaderEventHandle(img);
            pdf.AddEventHandler(PdfDocumentEvent.START_PAGE, handle);
            watch.Stop();
            // Header
            Paragraph header = new Paragraph("HEADER")
               .SetTextAlignment(TextAlignment.CENTER)
               .SetFontSize(20);

            // New line
            Paragraph newline = new Paragraph(new iText.Layout.Element.Text("\n"));

            document.Add(newline);
            document.Add(header);

            // Add sub-header
            Paragraph subheader = new Paragraph("SUB HEADER")
               .SetTextAlignment(TextAlignment.CENTER)
               .SetFontSize(15);
            document.Add(subheader);

            // Line separator
            LineSeparator ls = new LineSeparator(new SolidLine());
            document.Add(ls);

            // Add paragraph1
            Paragraph paragraph1 = new Paragraph("Lorem ipsum " +
               "dolor sit amet, consectetur adipiscing elit, " +
               "sed do eiusmod tempor incididunt ut labore " +
               "et dolore magna aliqua.");
            document.Add(paragraph1);

            // Add image

            document.Add(img);
            //iText.Layout.Element.Image img = new iText.Layout.Element.Image(ImageDataFactory
            //   .Create("C:\\test\\y2urwjovx1k8.png"));

            document.Add(img);

            // Table
            Table table = new Table(2, false);
            Cell cell11 = new Cell(1, 1)
               .SetBackgroundColor(ColorConstants.GRAY)
               .SetTextAlignment(TextAlignment.CENTER)
               .Add(new Paragraph("State"));
            Cell cell12 = new Cell(1, 1)
               .SetBackgroundColor(ColorConstants.GRAY)
               .SetTextAlignment(TextAlignment.CENTER)
               .Add(new Paragraph("Capital"));

            Cell cell21 = new Cell(1, 1)
               .SetTextAlignment(TextAlignment.CENTER)
               .Add(new Paragraph("New York"));
            Cell cell22 = new Cell(1, 1)
               .SetTextAlignment(TextAlignment.CENTER)
               .Add(new Paragraph("Albany"));

            Cell cell31 = new Cell(1, 1)
               .SetTextAlignment(TextAlignment.CENTER)
               .Add(new Paragraph("New Jersey"));
            Cell cell32 = new Cell(1, 1)
               .SetTextAlignment(TextAlignment.CENTER)
               .Add(new Paragraph("Trenton"));

            Cell cell41 = new Cell(1, 1)
               .SetTextAlignment(TextAlignment.CENTER)
               .Add(new Paragraph("California"));
            Cell cell42 = new Cell(1, 1)
               .SetTextAlignment(TextAlignment.CENTER)
               .Add(new Paragraph("Sacramento"));

            table.AddCell(cell11);
            table.AddCell(cell12);
            table.AddCell(cell21);
            table.AddCell(cell22);
            table.AddCell(cell31);
            table.AddCell(cell32);
            table.AddCell(cell41);
            table.AddCell(cell42);
            table.SetTextAlignment(TextAlignment.CENTER);
            document.Add(newline);
            document.Add(table);

            // Hyper link
            Link link = new Link("click here",
               PdfAction.CreateURI("https://www.google.com"));
            Paragraph hyperLink = new Paragraph("Please ")
               .Add(link.SetBold().SetUnderline()
               .SetItalic().SetFontColor(ColorConstants.BLUE))
               .Add(" to go www.google.com.");

            document.Add(newline);
            document.Add(hyperLink);

            // Page numbers
            int n = pdf.GetNumberOfPages();
            for (int i = 1; i <= n; i++)
            {
                document.ShowTextAligned(new Paragraph(String
                   .Format("page" + i + " of " + n)),
                   559, 806, i, TextAlignment.RIGHT,
                   VerticalAlignment.TOP, 0);
            }

            // Close document
            document.Close();
            Console.WriteLine($"itext7 pdf {watch.ElapsedMilliseconds} ms");
        }

        public static void ConvertToPdfWithHtml()
        {
            using FileStream htmlSource = File.Open("input.html", FileMode.Open);
            using FileStream pdfDest = File.Open("output.pdf", FileMode.Create);
            ConverterProperties converterProperties = new ConverterProperties();
            HtmlConverter.ConvertToPdf(htmlSource, pdfDest, converterProperties);
        }

        public static byte[] ConvertHtmlToPdf(string html)
        {
            byte[] bytes = Array.Empty<byte>();
            using (MemoryStream stream = new MemoryStream())
            {
                PdfWriter pdf = new PdfWriter(stream);

                //HtmlConverter.ConvertToPdf(html, pdf);
                //PdfDocument pdfDocument = new PdfDocument(pdf);

                Document document = HtmlConverter.ConvertToDocument(html, pdf);

                //Document document = new Document(pdfDocument, PageSize.LETTER);
                Image image = new Image(ImageDataFactory
          .Create("C:\\test\\y2urwjovx1k8.png"))
          .SetTextAlignment(TextAlignment.CENTER);
                HeaderEventHandle handle = new (image);
                var pdfDocument = document.GetPdfDocument();
                pdfDocument.AddEventHandler(PdfDocumentEvent.END_PAGE, handle);
                FooterEventHandle footerHandler = new ();
                pdfDocument.AddEventHandler(PdfDocumentEvent.END_PAGE, footerHandler);

                // Header
                //Paragraph header = new Paragraph("HEADER")
                //   .SetTextAlignment(TextAlignment.CENTER)
                //   .SetFontSize(20);

                // New line
                Paragraph newline = new Paragraph(new iText.Layout.Element.Text("\n"));

                //document.Add(newline);
                //document.Add(header);

                document.Close();
                bytes = stream.ToArray();


                return bytes;
            }
        }

        public void ManipulatePdf()
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter("C:\\test\\demo.pdf"));
            Document doc = new Document(pdfDoc);

            TableHeaderEventHandler handler = new TableHeaderEventHandler(doc);
            pdfDoc.AddEventHandler(PdfDocumentEvent.END_PAGE, handler);

            // Calculate top margin to be sure that the table will fit the margin.
            float topMargin = 20 + handler.GetTableHeight();
            doc.SetMargins(topMargin, 36, 36, 36);

            for (int i = 0; i < 50; i++)
            {
                doc.Add(new Paragraph("Hello World!"));
            }

            doc.Add(new AreaBreak());
            doc.Add(new Paragraph("Hello World!"));
            doc.Add(new AreaBreak());
            doc.Add(new Paragraph("Hello World!"));

            doc.Close();
        }

        private class TableHeaderEventHandler : IEventHandler
        {
            private Table table;
            private float tableHeight;
            private Document doc;

            public TableHeaderEventHandler(Document doc)
            {
                this.doc = doc;
                InitTable();

                TableRenderer renderer = (TableRenderer)table.CreateRendererSubTree();
                renderer.SetParent(new DocumentRenderer(doc));

                // Simulate the positioning of the renderer to find out how much space the header table will occupy.
                LayoutResult result = renderer.Layout(new LayoutContext(new LayoutArea(0, PageSize.A4)));
                tableHeight = result.GetOccupiedArea().GetBBox().GetHeight();
            }

            public void HandleEvent(Event currentEvent)
            {
                PdfDocumentEvent docEvent = (PdfDocumentEvent)currentEvent;
                PdfDocument pdfDoc = docEvent.GetDocument();
                PdfPage page = docEvent.GetPage();
                PdfCanvas canvas = new PdfCanvas(page.NewContentStreamBefore(), page.GetResources(), pdfDoc);
                PageSize pageSize = pdfDoc.GetDefaultPageSize();
                float coordX = pageSize.GetX() + doc.GetLeftMargin();
                float coordY = pageSize.GetTop() - doc.GetTopMargin();
                float width = pageSize.GetWidth() - doc.GetRightMargin() - doc.GetLeftMargin();
                float height = GetTableHeight();
                Rectangle rect = new Rectangle(coordX, coordY, width, height);

                new Canvas(canvas, rect)
                    .Add(table);
            }

            public float GetTableHeight()
            {
                return tableHeight;
            }

            private void InitTable()
            {
                table = new Table(1).UseAllAvailableWidth();
                table.AddCell("Header row 1");
                table.AddCell("Header row 2");
                table.AddCell("Header row 3");
            }
        }

        public class HeaderEventHandle : IEventHandler
        {
            iText.Layout.Element.Image img;
            public HeaderEventHandle(iText.Layout.Element.Image image)
            {
                this.img = image;
            }

            public void HandleEvent(Event @event)
            {
                PdfDocumentEvent docEvent = (PdfDocumentEvent)@event;
                PdfDocument pdfDocument = docEvent.GetDocument();
                PdfPage page = docEvent.GetPage();
                PdfCanvas canvas = new PdfCanvas(page.NewContentStreamBefore(), page.GetResources(), pdfDocument);
                Rectangle rootArea = new Rectangle(35, page.GetPageSize().GetTop() - 75, page.GetPageSize().GetWidth() - 72, 60);
                new Canvas(canvas, rootArea).Add(GetTable(docEvent)).Close();
            }

            public Table GetTable(PdfDocumentEvent docEvent)
            {
                float[] cellWidth = { 30f, 80f };
                Table tableEvent = new Table(UnitValue.CreatePercentArray(cellWidth)).UseAllAvailableWidth();
                Style style = new Style().SetBorder(Border.NO_BORDER);
                Style styleText = new Style().SetTextAlignment(TextAlignment.RIGHT).SetFontSize(10f);

                Cell cell = new Cell().Add(img.SetAutoScale(true));

                tableEvent.AddCell(cell.AddStyle(style).SetTextAlignment(TextAlignment.LEFT));
                PdfFont bold = PdfFontFactory.CreateFont(StandardFonts.TIMES_BOLD);
                cell = new Cell()
                    .Add(new Paragraph("reporte diario").SetFont(bold)).AddStyle(styleText).AddStyle(style);
                tableEvent.AddCell(cell);
                return tableEvent;
            }




        }

        public class FooterEventHandle : IEventHandler
        {



            public void HandleEvent(Event @event)
            {
                PdfDocumentEvent docEvent = (PdfDocumentEvent)@event;
                PdfDocument pdfDocument = docEvent.GetDocument();
                PdfPage page = docEvent.GetPage();
                PdfCanvas canvas = new PdfCanvas(page.NewContentStreamAfter(), page.GetResources(), pdfDocument);
                Rectangle rootArea = new Rectangle(35, 20, page.GetPageSize().GetWidth() - 70, 50);
                new Canvas(canvas, rootArea).Add(GetTable(docEvent)).Close();
            }

            public Table GetTable(PdfDocumentEvent docEvent)
            {
                float[] cellWidth = { 92f, 8f };
                Table tableEvent = new Table(UnitValue.CreatePercentArray(cellWidth)).UseAllAvailableWidth();

                PdfPage page = docEvent.GetPage();
                int pageNum = docEvent.GetDocument().GetPageNumber(page);
                Style styleCell = new Style().SetPadding(5).SetBorder(Border.NO_BORDER).SetBorderTop(new SolidBorder(ColorConstants.BLACK, 2));
                Cell cell = new Cell().Add(new Paragraph(DateTime.Now.ToLongDateString()));

                tableEvent.AddCell(cell.AddStyle(styleCell).SetTextAlignment(TextAlignment.RIGHT).SetFontColor(ColorConstants.LIGHT_GRAY));
                PdfFont bold = PdfFontFactory.CreateFont(StandardFonts.TIMES_BOLD);
                cell = new Cell().Add(new Paragraph(pageNum.ToString()));
                cell.AddStyle(styleCell).SetBackgroundColor(ColorConstants.BLACK)
                    .SetFontColor(ColorConstants.WHITE)
                    .SetTextAlignment(TextAlignment.CENTER);
                tableEvent.AddCell(cell);
                return tableEvent;
            }




        }
    }
}
