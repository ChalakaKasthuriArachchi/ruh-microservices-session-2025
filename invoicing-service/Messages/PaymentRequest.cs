namespace invoicing_service.Messages
{
    public class PaymentRequset
    {
        public int InvoiceId { get; set; }
        public decimal TotalAmount { get; set; }
    }
}