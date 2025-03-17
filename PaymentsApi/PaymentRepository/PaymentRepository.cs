using PaymentsApi.PaymentInterface;
using Car_Wash_Library.Models;
using Microsoft.EntityFrameworkCore;

namespace PaymentsApi.PaymentRepository
{
    public class PaymentRepository :IPayment
    {
        private readonly PaymentDbContext _context;

        public PaymentRepository(PaymentDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddPayment(Payment payment)
        {
            var status = await _context.Payments.FirstOrDefaultAsync(w => w.PaymentId == payment.PaymentId);
            if (status == null)
            {
                _context.Payments.Add(payment);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> DeletePayment(int paymentId)
        {
            var item = await _context.Payments.FirstOrDefaultAsync(w => w.PaymentId == paymentId);
            if (item != null)
            {
                 item.IsActive = false;
                //_context.Payments.Remove(item);
                //await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<Payment> GetPaymentById(int paymentId)
        {
            var item = await _context.Payments.FirstOrDefaultAsync(w => w.PaymentId == paymentId);
            if (item != null)
            {
                return item;
            }
            return null;
        }

        public async Task<IEnumerable<Payment>> GetPayments()
        {
            var item = await _context.Payments.Where(c=>c.IsActive==true).ToListAsync();
            return item;
        }

        public async Task<bool> UpdatePayment(int id, Payment payment)
        {
            var status = await _context.Payments.FirstOrDefaultAsync(w => w.PaymentId == id);
            if (status != null)
            {
                status.TransactionId = payment.TransactionId;
                status.TransactionDate = payment.TransactionDate;
                status.Amount = payment.Amount;
                status.PaymentMethod = payment.PaymentMethod;
                status.PaymentStatus = payment.PaymentStatus;
                status.IsActive = payment.IsActive;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
