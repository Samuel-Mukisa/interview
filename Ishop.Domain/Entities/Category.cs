namespace Ishop.Domain.Entities
{
    public class Category
    {
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? Description { get; set; }
        public int? NumberOfProducts { get; set; }
        public string? ImageVideoURL { get; set; }
    }
}