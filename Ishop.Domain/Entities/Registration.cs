namespace Ishop.Domain.Entities
{
    public class Registration
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int IsActive { get; set; }

        // New fields
        public string BusinessName { get; set; }
        public string AddressCity { get; set; }
        public string AddressCountry { get; set; }
        public string TypeOfProducts { get; set; }
        public string BusinessRegistrationNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string imageUrl { get; set; }
    }
}