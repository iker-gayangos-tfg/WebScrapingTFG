namespace WebScrapingAPI.Models
{
    public class ScopusCitescore
    {
        public int Id { get; set; }

        public int FoPublicacion { get; set; }

        public Publicacion? Publicacion { get; set; }

        public string? Year { get; set; }

        public string? MagazineCitescore { get; set; }

        public ICollection<ScopusCitescoreArea>? ScopusCitescoreAreas { get; set; }

    }
}
