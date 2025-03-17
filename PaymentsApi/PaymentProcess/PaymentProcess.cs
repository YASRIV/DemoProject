using PaymentsApi.PaymentInterface;
using Car_Wash_Library.Models;
using Car_Wash_Library.DTOClass;

namespace PaymentsApi.PaymentProcess
{
    public class PayProcess :IPayment
    {

        private readonly IPayment _context;

        public PayProcess(IPayment context)
        {
            _context = context;
        }

        public async Task<bool> AddPayment(Payment payment)
        {
            return await _context.AddPayment(payment);
        }

        public async Task<bool> DeletePayment(int paymentId)
        {
            return await _context.DeletePayment(paymentId);
        }

        public async Task<Payment> GetPaymentById(int paymentId)
            {
            return await _context.GetPaymentById(paymentId);
        }

        public async Task<IEnumerable<Payment>> GetPayments()
        {
            return await _context.GetPayments();
        }

        public async Task<bool> UpdatePayment(int id, Payment payment)
        {
            return await _context.UpdatePayment(id, payment);
        }


        public async Task<ResponsePayment> ProcessPayment(PaymentRequest paymentRequest)
        {
            try
            {
                var payment = new Payment
                {
                    TransactionId = new Random().Next(100000, 999999),
                    Amount = paymentRequest.Amount,
                    PaymentMethod = paymentRequest.PaymentMethod,
                    PaymentStatus = "Success", // Assuming payment is successful for now
                    TransactionDate = DateTime.UtcNow
                };

                await _context.AddPayment(payment);

                return new ResponsePayment
                {
                    IsSuccess = true,
                    TransactionId = payment.TransactionId,
                    Message = "Payment processed successfully."
                };
            }
            catch (Exception ex)
            {
                return new ResponsePayment
                {
                    IsSuccess = false,
                    Message = $"Payment failed: {ex.Message}"
                };
            }
        }


    }
}
