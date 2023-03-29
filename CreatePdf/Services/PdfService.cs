using CreatePdf.DataContext;
using CreatePdf.Interfaces;
using IronPDF;
using Microsoft.EntityFrameworkCore;

namespace CreatePdf.Services
{
    public class PdfService : IPdfService
    {
        private readonly SaeContext _context;

        public PdfService(SaeContext context)
        {
            _context = context;
        }

        public async Task<byte[]> GeneratePdf()
        {
            string templatePath = string.Empty;
            string headerPath = string.Empty;
            string footerPath = string.Empty;
            string contentPath = string.Empty;
            byte[] result = Array.Empty<byte>();
            try
            {
                templatePath = @"c:\test\example.html";
                if (File.Exists(templatePath))
                {
                    StreamReader sr = new StreamReader(templatePath);
                    string template = sr.ReadToEnd();
                    sr.Close();

                    headerPath = @"c:\test\header.html";
                    StreamReader srHeader = new StreamReader(headerPath);
                    string header = srHeader.ReadToEnd();
                    srHeader.Close();
                    //header = header.Replace("@[image]", @"c:\test\SAEHeader.jpg");
                    header = header.Replace("@[image]", "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcT3PLur_rz_pA71Fa6dry470d5u15zN6IvkkRngtsm3&s");
                    contentPath = @"c:\test\content.html";
                    StreamReader srContent = new StreamReader(contentPath);
                    string content = srContent.ReadToEnd();
                    srContent.Close();
                    var parameter = await _context.Parametros.Where(x => x.DescripcionParametro.Equals("PlantillaProcesoJuridicoSubsanar")).FirstOrDefaultAsync();
                    content = content.Replace("@[tableEjemplo]", parameter!.Valor);
                    footerPath = @"c:\test\footer.html";
                    StreamReader srFooter = new StreamReader(footerPath);
                    string footer = srFooter.ReadToEnd();
                    srFooter.Close();

                    template = template
                               .Replace("@[header]", header)
                               .Replace("@[contentBody]", content)
                               .Replace("@[footer]", footer);
                    result = ItextPDF.ConvertHtmlToPdf(template);


                }
            }
            catch (Exception ex)
            {

            }

            return result;
        }
    }
}
