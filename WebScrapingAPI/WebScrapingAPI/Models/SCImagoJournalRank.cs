namespace WebScrapingAPI.Models
{
    public class SCImagoJournalRank
    {
        public int Id { get; set; }

        public int FoPublicacion { get; set; }

        public Publicacion? Publicacion { get; set; }

        public string? Year { get; set; }

        public string? SJRImpactMagazine { get; set; }

        public string? MajorQuartil { get; set; }

        public ICollection<SCImagoJournalRankArea>? SCImagoJournalRankAreas { get; set; }

    }
}
