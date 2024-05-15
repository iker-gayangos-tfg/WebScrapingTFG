namespace WebScrapingAPI.Models
{
    public class InvestigadorFacultad
    {
        public int Id { get; set; }
        public int? FoInvestigador { get; set; }
        public Investigador? Investigador { get; set; }
        public int? FoFacultad { get; set; }
        public Facultad? Facultad { get; set; }
    }
}
