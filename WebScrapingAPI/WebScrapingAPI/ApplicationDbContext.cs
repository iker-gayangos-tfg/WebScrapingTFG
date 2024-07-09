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

        public DbSet<SCImagoJournalRank> SCImagoJournalRanks { get; set; }

        public DbSet<SCImagoJournalRankArea> SCImagoJournalRankAreas { get; set; }

        public DbSet<ScopusCitescore> ScopusCitescores { get; set; }

        public DbSet<ScopusCitescoreArea> ScopusCitescoreAreas { get; set; }

        public DbSet<JournalCitationIndicator> JournalCitationIndicators { get; set; }

        public DbSet<JournalCitationIndicatorArea> JournalCitationIndicatorAreas { get; set; }

        public DbSet<Dimensions> Dimensions { get; set; }

        public DbSet<DialnetRevista> DialnetRevistas { get; set; }

        public DbSet<Tesis> Tesis { get; set; }

        public DbSet<TesisDirector> TesisDirectores { get; set; }

        public DbSet<Patente> Patentes { get; set; }

        public DbSet<InvestigadorPatente> InvestigadoresPatentes { get; set; }



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

                entity.HasOne(d => d.SCImagoJournalRank)
                    .WithOne(p => p.Publicacion)
                    .HasForeignKey<SCImagoJournalRank>(d => d.FoPublicacion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("[FK_Publicacion_SCImagoJournalRank]");

                entity.HasOne(d => d.ScopusCitescore)
                    .WithOne(p => p.Publicacion)
                    .HasForeignKey<ScopusCitescore>(d => d.FoPublicacion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("[FK_Publicacion_ScopusCitescore]");

                entity.HasOne(d => d.JournalCitationIndicator)
                    .WithOne(p => p.Publicacion)
                    .HasForeignKey<JournalCitationIndicator>(d => d.FoPublicacion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("[FK_Publicacion_JournalCitationIndicator]");

                entity.HasOne(d => d.Dimensions)
                    .WithOne(p => p.Publicacion)
                    .HasForeignKey<Dimensions>(d => d.FoPublicacion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("[FK_Publicacion_Dimensions]");

                entity.HasOne(d => d.DialnetRevista)
                    .WithOne(p => p.Publicacion)
                    .HasForeignKey<DialnetRevista>(d => d.FoPublicacion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("[FK_Publicacion_DialnetRevista]");
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

            modelBuilder.Entity<SCImagoJournalRankArea>(entity =>
            {
                entity.HasOne(d => d.SCImagoJournalRank)
                    .WithMany(p => p.SCImagoJournalRankAreas)
                    .HasForeignKey(d => d.FoSCImagoJournalRank)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("[FK_SCImagoJournalRankAreas_SCImagoJournalRank]");
            });

            modelBuilder.Entity<ScopusCitescoreArea>(entity =>
            {
                entity.HasOne(d => d.ScopusCitescore)
                    .WithMany(p => p.ScopusCitescoreAreas)
                    .HasForeignKey(d => d.FoScopusCitescore)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("[FK_ScopusCitescoreAreas_ScopusCitescore]");
            });

            modelBuilder.Entity<JournalCitationIndicatorArea>(entity =>
            {
                entity.HasOne(d => d.JournalCitationIndicator)
                    .WithMany(p => p.JournalCitationIndicatorAreas)
                    .HasForeignKey(d => d.FoJournalCitationIndicator)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("[FK_JournalCitationIndicatorAreas_JournalCitationIndicator]");
            });

            modelBuilder.Entity<Tesis>(entity =>
            {
                entity.HasOne(d => d.Investigador)
                    .WithMany(p => p.Tesis)
                    .HasForeignKey(d => d.FoInvestigador)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("[FK_Tesis_Investigador]");
            });

            modelBuilder.Entity<TesisDirector>(entity =>
            {
                entity.HasOne(d => d.Investigador)
                    .WithMany(p => p.TesisDirectores)
                    .HasForeignKey(d => d.FoInvestigador)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("[FK_TesisDirector_Invstigador]");

                entity.HasOne(d => d.Tesis)
                    .WithMany(p => p.TesisDirectores)
                    .HasForeignKey(d => d.FoTesis)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("[FK_TesisDirector_Tesis]");
            });

            modelBuilder.Entity<InvestigadorPatente>(entity =>
            {
                entity.HasOne(d => d.Investigador)
                    .WithMany(p => p.InvestigadoresPatentes)
                    .HasForeignKey(d => d.FoInvestigador)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("[FK_InvestigadorPatente_Investigador]");

                entity.HasOne(d => d.Patente)
                    .WithMany(p => p.InvestigadoresPatentes)
                    .HasForeignKey(d => d.FoPatente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("[FK_InvestigadorPatente_Patente]");
            });
        }
    }
}
