using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using WebScrapingAPI.Models;

namespace WebScrapingAPI
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Investigador> Investigadores { get; set; }

        public DbSet<Departamento> Departamentos { get; set; }

        public DbSet<Facultad> Facultades { get; set; }

        public DbSet<InvestigadorFacultad> InvestigadoresFacultades { get; set; }

        public DbSet<Area> Areas { get; set; }

        public DbSet<InvestigadorArea> InvestigadoresAreas { get; set; }

        public DbSet<ProgramaDoctorado> ProgramasDoctorado { get; set; }

        public DbSet<InvestigadorProgramaDoctorado> InvestigadoresProgramasDoctorado { get; set; }
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

            modelBuilder.Entity<InvestigadorFacultad>(entity =>
            {
                entity.HasOne(d => d.Investigador)
                    .WithMany(p => p.InvestigadoresFacultades)
                    .HasForeignKey(d => d.FoInvestigador)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("[FK_Investigador]");

                entity.HasOne(d => d.Facultad)
                    .WithMany(p => p.InvestigadoresFacultades)
                    .HasForeignKey(d => d.FoFacultad)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("[FK_Facultad]");
            });

            modelBuilder.Entity<InvestigadorArea>(entity =>
            {
                entity.HasOne(d => d.Investigador)
                    .WithMany(p => p.InvestigadoresAreas)
                    .HasForeignKey(d => d.FoInvestigador)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("[FK_Investigador]");

                entity.HasOne(d => d.Area)
                    .WithMany(p => p.InvestigadoresAreas)
                    .HasForeignKey(d => d.FoArea)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("[FK_Area]");
            });

            modelBuilder.Entity<InvestigadorProgramaDoctorado>(entity =>
            {
                entity.HasOne(d => d.Investigador)
                    .WithMany(p => p.InvestigadoresProgramasDoctorado)
                    .HasForeignKey(d => d.FoInvestigador)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("[FK_Investigador]");

                entity.HasOne(d => d.ProgramaDoctorado)
                    .WithMany(p => p.InvestigadoresProgramasDoctorado)
                    .HasForeignKey(d => d.FoProgramaDoctorado)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("[FK_ProgramaDoctorado]");
            });
        }
    }
}
