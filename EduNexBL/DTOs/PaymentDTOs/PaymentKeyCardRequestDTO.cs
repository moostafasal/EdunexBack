namespace EduNexBL.DTOs.PaymentDTOs
{
    public class PaymentKeyCardRequestDTO
    {
        public string auth_token { get; set; }
        public string amount_cents { get; set; }
        public int expiration = 600;
        public int order_id { get; set; }
        public BillingData billing_data = new BillingData();
        public string currency = "EGP";
        public int integration_id = 4557496;

        public PaymentKeyCardRequestDTO(string authToken, string amountCents, int orderId, int integrationId)
        {
            this.auth_token = authToken;
            this.amount_cents = amountCents;
            this.order_id = orderId;
            this.integration_id = integrationId;
        }
    }
}
