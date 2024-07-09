namespace WebScrapingAPI.Models
{
    public class Tesis
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? Url { get; set; }

        public string? University { get; set; }

        public string? Date { get; set; }

        public string? Summary { get; set; }

        public int? FoInvestigador { get; set; }
        public Investigador? Investigador { get; set; }

        public ICollection<TesisDirector>? TesisDirectores { get; set; }


    }
}
