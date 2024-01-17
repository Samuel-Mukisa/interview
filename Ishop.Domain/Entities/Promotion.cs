namespace Ishop.Domain.Entities
{
    public class Promotion
    {
        public int? PromotionId { get; set; }
        public int? ManufacturerId { get; set; }
        public string? ManufacturerName { get; set; }
        public string? Package { get; set; }
        public string? ProductName { get; set; }
        public string? ImageUrl { get; set; }
        public string? BillingMethod { get; set; }
        public string? PromotionDuration { get; set; }
        public DateTime? PromotionDeadline { get; set; }
        public string? PhoneNumber { get; set; }
        public decimal? PromotionAmount { get; set; }
        public int? Views { get; set; }
        public DateTime? PostedTime { get; set; }
        public string? SimilarProducts { get; set; }
        public string? PromotionDescription { get; set; }
    }
}