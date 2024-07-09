namespace WebScrapingAPI.Models
{
    public class InvestigadorPublicacion
    {
        public int Id { get; set; }
        public int FoInvestigador { get; set; }
        public Investigador? Investigador { get; set; }
        public int FoPublicacion { get; set; }
        public Publicacion? Publicacion { get; set; }
    }
}
