// Product.cs
namespace Ishop.Domain.Entities
{
    public class Product
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public String Category { get; set; }
        public int ManufacturerID { get; set; } // New field
        public decimal Price { get; set; }
        public string Currency { get; set; }
        public string ImageURL { get; set; }
        public string VideoURL { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public string Material { get; set; }
        public decimal Weight { get; set; }
        public string ReturnAndWarranty { get; set; }
        public int StockQuantity { get; set; }
        public int StockThreshold { get; set; }
        public int Rating { get; set; } // New field
    }
}