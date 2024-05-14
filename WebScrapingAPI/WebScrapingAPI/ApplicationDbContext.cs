using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using WebScrapingAPI.Models;

namespace WebScrapingAPI
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Facultad> Facultades { get; set; }
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

                entity.HasOne(d => d.Facultad)
                    .WithMany(p => p.Investigadores)
                    .HasForeignKey(d => d.FoFacultad)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("[FK_Investigador_Facultad]");
            });
        }
    }
}
