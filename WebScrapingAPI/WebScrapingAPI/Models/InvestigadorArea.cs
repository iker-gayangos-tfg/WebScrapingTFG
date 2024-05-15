namespace WebScrapingAPI.Models
{
    public class InvestigadorArea
    {
        public int Id { get; set; }
        public int? FoInvestigador { get; set; }
        public Investigador? Investigador { get; set; }
        public int? FoArea { get; set; }
        public Area? Area { get; set; }
    }
}
