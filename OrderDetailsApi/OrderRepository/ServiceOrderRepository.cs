using OrderDetailsApi.OrderInterface;
using Car_Wash_Library.Models;
using Microsoft.EntityFrameworkCore;

namespace OrderDetailsApi.OrderRepository
{
    public class ServiceOrderRepository : IServiceOrder
    {
        private readonly OrderDbContext _context;

        public ServiceOrderRepository(OrderDbContext context)
        {
            _context = context;
        }


        public async Task<bool> AddServiceOrder(ServiceOrder serviceorder)
        {
            var status = await _context.ServiceOrders.FirstOrDefaultAsync(w => w.ServiceOrderId == serviceorder.ServiceOrderId);
            if (status == null)
            {
                _context.ServiceOrders.Add(serviceorder);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteServiceOrder(int serviceorderId)
        {
            var item = await _context.ServiceOrders.FirstOrDefaultAsync(w => w.ServiceOrderId == serviceorderId);
            if (item != null)
            {
              //  item.IsActive = false;
              _context.ServiceOrders.Remove(item);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }



        public async Task<IEnumerable<int>> GetServiceOrderByOrderId(int orderId)
        {
            try
            {
                var item = await _context.ServiceOrders.Where(c => c.OrderId == orderId).Select(c => c.ServiceId).ToListAsync();
                return item;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Database Update Error: Unable to save service order.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error occurred while adding the service order.", ex);
            }
        }
        public async Task<ServiceOrder> GetServiceOrderById(int serviceorderId)
        {
            var item = await _context.ServiceOrders.FirstOrDefaultAsync(w => w.ServiceOrderId == serviceorderId);
            if (item != null)
            {
                return item;
            }
            return null;
        }

        public async Task<IEnumerable<ServiceOrder>> GetServiceOrders()
        {
            var item = await _context.ServiceOrders.ToListAsync();

            return item;
        }

        public async Task<bool> UpdateServiceOrder(int id, ServiceOrder serviceorder)
        {
            var status = await _context.ServiceOrders.FirstOrDefaultAsync(w => w.ServiceOrderId == id);
            if (status != null)
            {
                status.ServiceOrderId = serviceorder.ServiceOrderId;
                status.ServiceId = serviceorder.ServiceId;
                status.OrderId = serviceorder.OrderId;
                status.Price = serviceorder.Price;
               
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }



    }
}
