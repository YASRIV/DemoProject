using OrderDetailsApi.Interface;
using Car_Wash_Library.Models;
using Microsoft.EntityFrameworkCore;
namespace OrderDetailsApi.OrderRepository
{
    public class OrderRepository : IOrder
    {
        private readonly OrderDbContext _context;

        public OrderRepository(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddOrder(Order order)
        {
            try
            {
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
                return order.OrderId;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Database Update Error: Unable to save order.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error occurred while adding the order.", ex);
            }
        }

        public async Task<bool> DeleteOrder(int orderId)
        {
            var item = await _context.Orders.FirstOrDefaultAsync(w => w.OrderId == orderId);
            if (item != null)
            {
                item.IsActive = false;
                return true;
            }
            return false;
        }

        public async Task<Order> GetOrderById(int orderId)
        {
            var item = await _context.Orders.FirstOrDefaultAsync(w => w.OrderId == orderId);
            if (item != null)
            {
                return item;
            }
            return null;
        }


        public async Task<List<Order>> GetOrdersByCustomerId(int CustomerId)
        {
            var item = await _context.Orders.Where(w => w.CustomerId == CustomerId).ToListAsync();
            if (item.Count!=0)
            {
                return item;
            }
            return null;
        }


        public async Task<List<Order>> GetOrdersByWasherId(int WasherId)
        {
            var item = await _context.Orders.Where(w => w.WasherId == WasherId).ToListAsync();
            if (item.Count != 0)
            {
                return item;
            }
            return null;
        }

        public async Task<IEnumerable<Order>> GetOrders()
        {
            var item = await _context.Orders.Where(w => w.IsActive == true).ToListAsync();

            return item;
        }

        public async Task<bool> UpdateOrder( Order order)
        {
            var status = await _context.Orders.FirstOrDefaultAsync(w => w.OrderId == order.OrderId);
            if (status != null)
            {
                status.OrderId = order.OrderId;
                status.CustomerId = order.CustomerId;
                status.WasherId = order.WasherId;
                status.OrderDate = order.OrderDate;
                status.WashDate = order.WashDate;
                status.TransactionId = order.TransactionId;
                status.RequestId = order.RequestId;
                status.ServiceOrderId = order.ServiceOrderId;
                status.OrderStatus = order.OrderStatus;
                status.TotalPrice = order.TotalPrice;
                status.IsInvoiceGenerated = order.IsInvoiceGenerated;
                status.IsActive = order.IsActive;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }








    }
}
