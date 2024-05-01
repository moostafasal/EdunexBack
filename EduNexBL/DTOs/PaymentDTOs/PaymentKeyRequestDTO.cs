namespace EduNexBL.DTOs.PaymentDTOs
{
    public class PaymentKeyRequestDTO
    {
        public string auth_token { get; set; }
        public string amount_cents { get; set; }
        public int expiration = 600;
        public int order_id { get; set; }
        public BillingData billing_data = new BillingData();
        public string currency = "EGP";
        public int integration_id = 4557496;
    }

    public class BillingData
    {
        public string apartment { get; set; }
        public string email { get; set; }
        public string floor { get; set; }
        public string first_name { get; set; }
        public string street { get; set; }
        public string building { get; set; }
        public string phone_number { get; set; }
        public string shipping_method { get; set; }
        public string postal_code { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string last_name { get; set; }
        public string state { get; set; }

        // Default constructor
        public BillingData()
        {
            apartment = "NA";
            email = "NA";
            floor = "NA";
            first_name = "NA";
            street = "NA";
            building = "NA";
            phone_number = "NA";
            shipping_method = "NA";
            postal_code = "NA";
            city = "NA";
            country = "NA";
            last_name = "NA";
            state = "NA";
        }
    }
}
