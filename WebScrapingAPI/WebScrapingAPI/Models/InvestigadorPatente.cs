namespace WebScrapingAPI.Models
{
    public class InvestigadorPatente
    {
        public int Id { get; set; }
        public int? FoInvestigador { get; set; }
        public Investigador? Investigador { get; set; }
        public int? FoPatente { get; set; }
        public Patente? Patente { get; set; }
    }
}
