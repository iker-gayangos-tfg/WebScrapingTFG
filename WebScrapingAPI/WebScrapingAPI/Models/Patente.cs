namespace WebScrapingAPI.Models
{
    public class Patente
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? Url { get; set; }

        public string? Summary { get; set; }

        public ICollection<InvestigadorPatente>? InvestigadoresPatentes { get; set; }


    }
}
