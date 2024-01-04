namespace Ishop.Domain.Entities;

public class Product
{
    public int ProductID { get; set; }
    public int ManufacturerID { get; set; }
    public int CategoryID { get; set; }
    public string ProductName { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int Rating { get; set; }
    public int StockQuantity { get; set; }
    public bool InStock { get; set; }
    public string Image { get; set; }
    public string VideoURL { get; set; }
    public string ReturnPolicy { get; set; }
    public string WarrantyInformation { get; set; }
    
}