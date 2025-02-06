namespace payment_service.Messages
{
    public class PaymentRequest
    {
        public int InvoiceId { get; set; }
        public decimal TotalAmount { get; set; }
    }
}