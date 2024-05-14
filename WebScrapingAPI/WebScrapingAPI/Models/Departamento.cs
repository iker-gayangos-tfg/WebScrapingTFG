namespace WebScrapingAPI.Models
{
    public class Departamento
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Url { get; set; }

        public ICollection<Investigador>? Investigadores { get; set; }

    }
}
