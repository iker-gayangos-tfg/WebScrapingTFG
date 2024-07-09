namespace WebScrapingAPI.Models
{
    public class DialnetRevista
    {
        public int Id { get; set; }

        public int FoPublicacion { get; set; }

        public Publicacion? Publicacion { get; set; }

        public string? Year { get; set; }

        public string? MagazineImpact { get; set; }

        public string? Ambit { get; set; }

        public string? Quartil { get; set; }

        public string? Position { get; set; }

    }
}
