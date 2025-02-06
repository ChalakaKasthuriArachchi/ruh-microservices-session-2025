using Microsoft.AspNetCore.Mvc;
using invoicing_service.Models;
using invoicing_service.DTOs;
using invoicing_service.Messages;

namespace invoicing_service
{
    [ApiController]
    [Route("Invoice")]
    public class InvoiceController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly PaymentRequestService _paymentRequestService;

        public InvoiceController(AppDbContext context, PaymentRequestService paymentRequestService)
        {
            _context = context;
            _paymentRequestService = paymentRequestService;
        }

        [HttpGet]
        /* Get All Invoices */
        public IEnumerable<Invoice> GetAll()
        {
            return _context.Invoices.ToList();
        }

        [HttpGet("{id}")]
        /* Get a Single Invoices */
        public Invoice? Get(int id)
        {
            return _context.Invoices.FirstOrDefault(inv => inv.Id == id);
        }

        [HttpPost]
        /* Save a new Invoice */
        public async Task<IActionResult> PostInvoice(InvoiceDTO dto)
        {
            Invoice invoice = new Invoice(){
                TotalAmount = dto.TotalAmount,
            };

            _context.Invoices.Add(invoice);
            _context.SaveChanges();

            var paymentRequset = new PaymentRequset(){
                InvoiceId = invoice.Id,
                TotalAmount = invoice.TotalAmount
            };

            await _paymentRequestService.PublishMessage(paymentRequset);   

            return Ok(invoice);
        }
    }
}
