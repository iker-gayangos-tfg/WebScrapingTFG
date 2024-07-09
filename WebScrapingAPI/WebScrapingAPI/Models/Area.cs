namespace WebScrapingAPI.Models
{
    public class Area
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Url { get; set; }

        public ICollection<InvestigadorArea>? InvestigadoresAreas { get; set; }
    }
}
