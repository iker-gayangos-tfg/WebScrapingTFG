namespace WebScrapingAPI.Models
{
    public class InvestigadorProgramaDoctorado
    {
        public int Id { get; set; }
        public int? FoInvestigador { get; set; }
        public Investigador? Investigador { get; set; }
        public int? FoProgramaDoctorado { get; set; }
        public ProgramaDoctorado? ProgramaDoctorado { get; set; }
    }
}
