using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Car_Wash_Library.Models;

namespace Car_Wash_Library.DTOClass
{
    public class PaymentRequest
    {
        public int OrderId { get; set; }
        public double Amount { get; set; }
        public string PaymentMethod { get; set; }

    }

    public class ResponsePayment
    {
        public bool IsSuccess { get; set; }
        public int TransactionId { get; set; }
        public string Message { get; set; }
    }

    public class InvoiceDto
    {
        public int OrderId { get; set; }
        public int TransactionId { get; set; }
        public int WasherId { get; set; }
        public string WasherName { get; set; } = string.Empty;
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public List<Car> CarDetails { get; set; }// = string.Empty;
        public List<string> ServiceName { get; set; } //= string.Empty;
        public double Price { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; } = "Paid";
    }
}
