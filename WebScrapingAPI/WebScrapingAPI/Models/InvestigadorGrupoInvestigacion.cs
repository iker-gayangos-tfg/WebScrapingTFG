namespace WebScrapingAPI.Models
{
    public class InvestigadorGrupoInvestigacion
    {
        public int Id { get; set; }
        public int? FoInvestigador { get; set; }
        public Investigador? Investigador { get; set; }
        public int? FoGrupoInvestigacion { get; set; }
        public GrupoInvestigacion? GrupoInvestigacion { get; set; }
    }
}
