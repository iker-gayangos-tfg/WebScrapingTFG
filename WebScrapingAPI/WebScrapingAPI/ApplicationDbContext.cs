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

        public DbSet<GrupoInvestigacion> GruposInvestigacion { get; set; }

        public DbSet<InvestigadorGrupoInvestigacion> InvestigadoresGruposInvestigacion { get; set; }

        public DbSet<Publicacion> Publicaciones { get; set; }

        public DbSet<InvestigadorPublicacion> InvestigadoresPublicaciones { get; set; }

        public DbSet<CitaRecibida> CitasRecibidas { get; set; }

        public DbSet<JournalImpactFactor> JournalImpactFactors { get; set; }

        public DbSet<JournalImpactFactorArea> JournalImpactFactorAreas { get; set; }
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
                    .HasConstraintName("[FK_InvestigadorFacultad_Investigador]");

                entity.HasOne(d => d.Facultad)
                    .WithMany(p => p.InvestigadoresFacultades)
                    .HasForeignKey(d => d.FoFacultad)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("[FK_InvestigadorFacultad_Facultad]");
            });

            modelBuilder.Entity<InvestigadorArea>(entity =>
            {
                entity.HasOne(d => d.Investigador)
                    .WithMany(p => p.InvestigadoresAreas)
                    .HasForeignKey(d => d.FoInvestigador)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("[FK_InvestigadorArea_Investigador]");

                entity.HasOne(d => d.Area)
                    .WithMany(p => p.InvestigadoresAreas)
                    .HasForeignKey(d => d.FoArea)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("[FK_InvestigadorArea_Area]");
            });

            modelBuilder.Entity<InvestigadorProgramaDoctorado>(entity =>
            {
                entity.HasOne(d => d.Investigador)
                    .WithMany(p => p.InvestigadoresProgramasDoctorado)
                    .HasForeignKey(d => d.FoInvestigador)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("[FK_InvestigadorProgramaDoctorado_Investigador]");

                entity.HasOne(d => d.ProgramaDoctorado)
                    .WithMany(p => p.InvestigadoresProgramasDoctorado)
                    .HasForeignKey(d => d.FoProgramaDoctorado)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("[FK_InvestigadorProgramaDoctorado_ProgramaDoctorado]");
            });

            modelBuilder.Entity<InvestigadorGrupoInvestigacion>(entity =>
            {
                entity.HasOne(d => d.Investigador)
                    .WithMany(p => p.InvestigadoresGruposInvestigacion)
                    .HasForeignKey(d => d.FoInvestigador)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("[FK_InvestigadorGrupoInvestigacion_Investigador]");

                entity.HasOne(d => d.GrupoInvestigacion)
                    .WithMany(p => p.InvestigadoresGruposInvestigacion)
                    .HasForeignKey(d => d.FoGrupoInvestigacion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("[FK_InvestigadorGrupoInvestigacion_GrupoInvestigacion]");
            });


            modelBuilder.Entity<InvestigadorPublicacion>(entity =>
            {
                entity.HasOne(d => d.Investigador)
                    .WithMany(p => p.InvestigadoresPublicaciones)
                    .HasForeignKey(d => d.FoInvestigador)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("[FK_InvestigadorPublicacion_Investigador]");

                entity.HasOne(d => d.Publicacion)
                    .WithMany(p => p.InvestigadoresPublicaciones)
                    .HasForeignKey(d => d.FoPublicacion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("[FK_InvestigadorPublicacion_Publicacion]");
            });

            modelBuilder.Entity<Publicacion>(entity =>
            {
                entity.HasOne(d => d.CitaRecibida)
                    .WithOne(p => p.Publicacion)
                    .HasForeignKey<CitaRecibida>(d => d.FoPublicacion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("[FK_Publicacion_CitaRecibida]");

                entity.HasOne(d => d.JournalImpactFactor)
                    .WithOne(p => p.Publicacion)
                    .HasForeignKey<JournalImpactFactor>(d => d.FoPublicacion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("[FK_Publicacion_JournalImpactFactor]");
            });

            modelBuilder.Entity<JournalImpactFactorArea>(entity =>
            {
                entity.HasOne(d => d.JournalImpactFactor)
                    .WithMany(p => p.JournalImpactFactorAreas)
                    .HasForeignKey(d => d.FoJournalImpactFactor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("[FK_JournalImpactFactorAreas_JournalImpactFactor]");
            });
            });
            });
        }
    }
}
