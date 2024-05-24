namespace WebScrapingAPI.Models
{
    public class TesisDirector
    {
        public int Id { get; set; }
        public int? FoTesis { get; set; }
        public Tesis? Tesis { get; set; }
        public int? FoInvestigador { get; set; }
        public Investigador? Investigador { get; set; }
    }
}
