namespace EduNexBL.DTOs.PaymentDTOs
{
    public class PaymentKeyMobileWalletRequestDTO
    {
        public string auth_token { get; set; }
        public string amount_cents { get; set; }
        public int expiration = 600;
        public int order_id { get; set; }
        public BillingData billing_data = new BillingData();
        public string currency = "EGP";
        public int integration_id = 4570848;
    }

    
}
