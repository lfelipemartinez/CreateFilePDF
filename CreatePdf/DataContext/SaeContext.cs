using CreatePdf.Models;
using Microsoft.EntityFrameworkCore;

namespace CreatePdf.DataContext
{
    public class SaeContext : DbContext
    {
        public SaeContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Parametros> Parametros { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("sae");
            modelBuilder.Entity<Parametros>(entity =>
            {
                entity.ToTable("P_parametros", "sae");
                entity.HasKey(x => x.Id).HasName("id");
            });
        }
    }
}
