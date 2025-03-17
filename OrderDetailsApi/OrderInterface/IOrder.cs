using Car_Wash_Library.Models;
namespace OrderDetailsApi.Interface
{
    public interface IOrder
    {
        Task<int> AddOrder(Order order);
        Task<bool> UpdateOrder( Order order);
        Task<bool> DeleteOrder(int orderId);
        Task<Order> GetOrderById(int orderId);
        Task<List<Order>> GetOrdersByCustomerId(int CustomerId);
        Task<List<Order>> GetOrdersByWasherId(int WasherId);
        Task<IEnumerable<Order>> GetOrders();
    }
}
