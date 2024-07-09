namespace WebScrapingAPI.Models
{
    public class Dimensions
    {
        public int Id { get; set; }

        public int FoPublicacion { get; set; }

        public Publicacion? Publicacion { get; set; }

        public string? CitasTotales { get; set; }

        public string? CitasRecientes { get; set; }

        public string? FieldCitationRatio { get; set; }

    }
}
