using CreatePdf.DataContext;
using CreatePdf.Interfaces;
using CreatePdf.Models;
using IronPDF;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Diagnostics;

namespace CreatePdf.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPdfService _pdfService;
        private readonly SaeContext _context;

        public HomeController(ILogger<HomeController> logger, IPdfService pdfService, SaeContext context)
        {
            _logger = logger;
            _pdfService = pdfService;
            _context = context;
        }

        public IActionResult Index()
        {
            //ConvertIronPDF.ConvertWithHeaderAndFooter();
            //ConvertIronPDF.ConvertToIronPDF();
            var itextPDF = new ItextPDF();
            itextPDF.ConvertToPdfItextSharp();
            itextPDF.ManipulatePdf();
            //GenerateQuestPdf();
            //ItextPDF.ConvertToPdfWithHtml();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public async Task<IActionResult> GeneratePDF()
        {
            return File(await _pdfService.GeneratePdf(), "application/pdf", "example.pdf");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private void GenerateQuestPdf()
        {
            Console.WriteLine("Inicio ejecucon QuestPDF");
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(20));
                    page.Header()
                        .Text("Hello PDF!")
                        .SemiBold().FontSize(36).FontColor(Colors.Blue.Medium);

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(x =>
                        {
                            x.Spacing(20);

                            x.Item().Text(Placeholders.LoremIpsum());
                            x.Item().Image(Placeholders.Image(200, 100));
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Page ");
                            x.CurrentPageNumber();
                        });
                });
            })
              .GeneratePdf("hello.pdf");
            Console.WriteLine($"QuestPDF {watch.ElapsedMilliseconds} ms");
        }
    }
}