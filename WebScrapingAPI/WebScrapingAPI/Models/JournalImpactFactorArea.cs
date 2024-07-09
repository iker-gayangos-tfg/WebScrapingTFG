namespace WebScrapingAPI.Models
{
    public class JournalImpactFactorArea
    {
        public int Id { get; set; }

        public int FoJournalImpactFactor { get; set; }

        public JournalImpactFactor? JournalImpactFactor { get; set; }

        public string? Area { get; set; }

        public string? Quartil { get; set; }

        public string? Position { get; set; }

    }
}
