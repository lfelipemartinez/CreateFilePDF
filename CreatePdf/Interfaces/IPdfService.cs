namespace CreatePdf.Interfaces
{
    public interface IPdfService
    {
      Task<byte[]> GeneratePdf();
    }
}
