using Car_Wash_Library.Models;

namespace OrderDetailsApi.OrderInterface
{
    public interface IServiceOrder
    {

        Task<bool> AddServiceOrder(ServiceOrder serviceorder);
        Task<bool> UpdateServiceOrder(int id,  ServiceOrder serviceorder);
        Task<bool> DeleteServiceOrder(int serviceorderId);
        Task<ServiceOrder> GetServiceOrderById(int serviceorderId);
        Task<IEnumerable<ServiceOrder>> GetServiceOrders();
        Task<IEnumerable<int>> GetServiceOrderByOrderId(int orderId);
    }
}
