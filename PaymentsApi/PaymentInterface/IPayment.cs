
using Car_Wash_Library.Models;
namespace PaymentsApi.PaymentInterface
{
    public interface IPayment
    {

        Task<bool> AddPayment(Payment payment);
        Task<bool> DeletePayment(int paymentId);
        Task<Payment> GetPaymentById(int paymentId);
        Task<IEnumerable<Payment>> GetPayments();
        Task<bool> UpdatePayment(int id, Payment payment);
    }
}
