namespace WebScrapingAPI.Models
{
    public class Publicacion
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? Url { get; set; }

        public string? Magazine { get; set; }

        public string? Book { get; set; }

        public string? BookCollection { get; set; }

        public string? Editorial { get; set; }

        public string? ISSN { get; set; }

        public string? ISBN { get; set; }

        public string? Year { get; set; }

        public string? Volumen { get; set; }

        public string? Number { get; set; }

        public string? Pages { get; set; }

        public string? Type { get; set; }

        public string? Summary { get; set; }

        public ICollection<InvestigadorPublicacion>? InvestigadoresPublicaciones { get; set; }

        public CitaRecibida? CitaRecibida { get; set; }

        public JournalImpactFactor? JournalImpactFactor { get; set; }

        public SCImagoJournalRank? SCImagoJournalRank { get; set; }

        public ScopusCitescore? ScopusCitescore { get; set; }

        public JournalCitationIndicator? JournalCitationIndicator { get; set; }

        public Dimensions? Dimensions { get; set; }

        public DialnetRevista? DialnetRevista { get; set; }

    }
}
