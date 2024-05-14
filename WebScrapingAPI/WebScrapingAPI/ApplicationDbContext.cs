using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using WebScrapingAPI.Models;

namespace WebScrapingAPI
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Departamento> Departamentos { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options) { 
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Investigador>(entity =>
            {
                entity.HasOne(d => d.Departamento)
                    .WithMany(p => p.Investigadores)
                    .HasForeignKey(d => d.FoDepartamento)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("[FK_Investigador_Departamento]");
            });
        }
    }
}
