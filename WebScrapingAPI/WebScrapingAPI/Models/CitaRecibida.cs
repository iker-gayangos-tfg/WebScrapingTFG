namespace WebScrapingAPI.Models
{
    public class CitaRecibida
    {
        public int Id { get; set; }

        public int FoPublicacion { get; set; }

        public Publicacion? Publicacion { get; set; }

        public string? ScopusCount { get; set; }

        public string? ScopusUrl { get; set; }

        public string? ScopusDate { get; set; }

        public string? WebScienceCount { get; set; }

        public string? WebScienceUrl { get; set; }

        public string? WebScienceDate { get; set; }

        public string? DimensionsCount { get; set; }

        public string? DimensionsUrl { get; set; }

        public string? DimensionsDate { get; set; }

    }
}
