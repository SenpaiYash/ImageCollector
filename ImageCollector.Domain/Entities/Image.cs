namespace ImageCollector.Domain.Entities
{
    public class Image
    {
        public int Id { get; set; }
        public string Location { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
