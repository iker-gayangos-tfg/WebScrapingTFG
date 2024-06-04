namespace WebScrapingAPI.Models
{
    public class PagedCollectionResponse<T>
    {
        public List<T> Items { get; set; }
        public int Total { get; set; }
    }
}
