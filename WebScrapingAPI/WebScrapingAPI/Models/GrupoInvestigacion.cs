namespace WebScrapingAPI.Models
{
    public class GrupoInvestigacion
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Url { get; set; }
        public ICollection<InvestigadorGrupoInvestigacion>? InvestigadoresGruposInvestigacion { get; set; }
    }
}
