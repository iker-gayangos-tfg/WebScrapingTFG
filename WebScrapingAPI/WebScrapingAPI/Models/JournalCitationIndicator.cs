namespace WebScrapingAPI.Models
{
    public class JournalCitationIndicator
    {
        public int Id { get; set; }

        public int FoPublicacion { get; set; }

        public Publicacion? Publicacion { get; set; }

        public string? Year { get; set; }

        public string? MagazineJCI { get; set; }

        public string? MajorQuartil { get; set; }

        public ICollection<JournalCitationIndicatorArea>? JournalCitationIndicatorAreas { get; set; }

    }
}
