namespace EduNexBL.DTOs.PaymentDTOs
{
    public class OrderResponseModel
    {
        public int id { get; set; }
        public string created_at { get; set; }
        public int amount_cents { get; set;}
    }
}
