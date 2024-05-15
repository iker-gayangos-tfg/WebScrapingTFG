namespace WebScrapingAPI.Models
{
    public class ProgramaDoctorado
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Url { get; set; }
        public ICollection<InvestigadorProgramaDoctorado>? InvestigadoresProgramasDoctorado { get; set; }
    }
}
