using Car_Wash_Library.Models;
using Microsoft.EntityFrameworkCore;
using OrderDetailsApi.OrderInterface;

namespace OrderDetailsApi.OrderRepository
{
    public class WashRequestRepository :IWashRequest
    {
        private readonly OrderDbContext _context;

        public WashRequestRepository(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddWashRequest(WashRequest washrequest)
        {
            var status = await _context.WashRequests.FirstOrDefaultAsync(w => w.RequestId == washrequest.RequestId);
            if (status == null)
            {
                _context.WashRequests.Add(washrequest);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteWashRequest(int requestId)
        {
            var item = await _context.WashRequests.FirstOrDefaultAsync(w => w.RequestId == requestId);
            if (item != null)
            {
                 item.IsActive = false;
                //_context.ServiceOrders.Remove(item);
                //await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<WashRequest> GetWashRequestById(int requestId)
        {
            var item = await _context.WashRequests.FirstOrDefaultAsync(w => w.RequestId == requestId);
            if (item != null)
            {
                return item;
            }
            return null;
        }

        public async Task<IEnumerable<WashRequest>> GetWashRequests()
        {
            var item = await _context.WashRequests.Where(c=>c.IsActive==true).ToListAsync();

            return item;
        }

        public async Task<List<WashRequest>> GetAllWashRequests()
        {
            return await _context.WashRequests.ToListAsync();
        }

       

        public async Task<bool> AcceptWashRequest(int requestId, int washerId)
        {
            var request = await _context.WashRequests.FindAsync(requestId);
            if (request == null || request.RequestStatus != "Pending")
                return false;

            request.WasherId = washerId;
            request.RequestStatus = "Accepted";
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> HandleWashRequest(int requestId)
        {
            var request = await _context.WashRequests.FindAsync(requestId);
            if (request == null || request.RequestStatus != "Accepted")
                return false;

            request.RequestStatus = "In Progress";
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateWashRequest(int requestId)
        {
            var request = await _context.WashRequests.FindAsync(requestId);
            if (request == null || request.RequestStatus != "In Progress")
                return false;

            request.RequestStatus = "Completed";
            request.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
