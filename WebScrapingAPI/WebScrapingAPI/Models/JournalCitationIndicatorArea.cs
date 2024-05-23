namespace WebScrapingAPI.Models
{
    public class JournalCitationIndicatorArea
    {
        public int Id { get; set; }

        public int FoJournalCitationIndicator { get; set; }

        public JournalCitationIndicator? JournalCitationIndicator { get; set; }

        public string? Area { get; set; }

        public string? Quartil { get; set; }

        public string? Position { get; set; }

    }
}
