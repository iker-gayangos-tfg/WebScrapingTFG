namespace WebScrapingAPI.Models
{
    public class JournalImpactFactor
    {
        public int Id { get; set; }

        public int FoPublicacion { get; set; }

        public Publicacion? Publicacion { get; set; }

        public string? Year { get; set; }

        public string? MagazineImpact { get; set; }

        public string? NoAutoImpact { get; set; }

        public string? ArticleInfluenceScore { get; set; }

        public string? MajorQuartil { get; set; }

        public ICollection<JournalImpactFactorArea>? JournalImpactFactorAreas { get; set; }

    }
}
