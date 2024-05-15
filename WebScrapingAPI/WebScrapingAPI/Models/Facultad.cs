namespace WebScrapingAPI.Models
{
    public class Facultad
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Url { get; set; }

        public ICollection<InvestigadorFacultad>? InvestigadoresFacultades { get; set; }
    }
}
