namespace payment_service.Messages
{
    public class PaymentAck
    {
        public int InvoiceId { get; set; }
        public string PaymentStatus { get; set; } = "PENDING";
    }
}