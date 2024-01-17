// Order.cs
namespace Ishop.Domain.Entities
{
    public class Order
    {
        public int OrderID { get; set; }
        public int RetailerID { get; set; }
        public List<Product> Products { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }

        // Additional Fields
        public string CustomerName { get; set; }
        public string OrderNumber { get; set; }
        public int Quantity { get; set; }
        public string ShippingAddress { get; set; }
        public string Contact { get; set; }
        public decimal UnitPrice { get; set; }
        public string Currency { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime DeliveryDeadline { get; set; }
        public string ReturnPolicy { get; set; }
        public string OrderNotes { get; set; }

        // Additional Fields
        public decimal DeliveryCharge { get; set; }
        public decimal Discount { get; set; }
        public decimal Subtotal { get; set; }
        public string Promocode { get; set; }
    }
}