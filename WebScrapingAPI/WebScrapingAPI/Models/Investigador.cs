using Newtonsoft.Json;

namespace WebScrapingAPI.Models
{
    public class Investigador
    {
        public int Id { get; set; }

        public string? Nombre { get; set; }

        public string? Apellidos { get; set; }

        public string? IdInvestigador { get; set; }

        public string? Email { get; set; }

        public int? FoDepartamento { get; set; }
        public Departamento? Departamento { get; set; }

        public ICollection<InvestigadorFacultad>? InvestigadoresFacultades { get; set; }

        public ICollection<InvestigadorArea>? InvestigadoresAreas { get; set; }

        public ICollection<InvestigadorProgramaDoctorado>? InvestigadoresProgramasDoctorado { get; set; }

        public ICollection<InvestigadorGrupoInvestigacion>? InvestigadoresGruposInvestigacion { get; set; }

        public ICollection<InvestigadorPublicacion>? InvestigadoresPublicaciones { get; set; }

        public ICollection<Tesis>? Tesis { get; set; }

        public ICollection<TesisDirector>? TesisDirectores { get; set; }

        public ICollection<InvestigadorPatente>? InvestigadoresPatentes { get; set; }

        public InvestigadorResponse ConvertToResponse()
        {
            return new InvestigadorResponse()
            {
                Id = Id,
                Nombre = Nombre,
                Apellidos = Apellidos,
                IdInvestigador = IdInvestigador,
                Email = Email,
                Departamento = Departamento != null ? Departamento.ConvertToResponse() : new DepartamentoResponse(),
            };
        }
    }


    public class InvestigadorResponse
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Apellidos { get; set; }
        public string? IdInvestigador { get; set; }
        public string? Email { get; set; }
        public DepartamentoResponse Departamento { get; set; }
    }

    public class FilterInvestigadores
    {
        public string? Nombre { get; set; }
        public List<int>? InvestigatorIds { get; set; }
        public int Page { get; set; }
        public int Limit { get; set; }
    }

    public class InvestigadorBindRequest
    {
        public int Id { get; set; }
        public required List<int> InvestigatorIds { get; set; }
    }

}
