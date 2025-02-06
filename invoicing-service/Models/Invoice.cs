namespace invoicing_service.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
        public decimal TotalAmount { get; set; }
        public string PaymentStatus {get; set;} = "PENDING";
    }
}