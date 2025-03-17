using Car_Wash_Library.Models;
using Microsoft.EntityFrameworkCore;
using WasherApi.Interface;

namespace WasherApi.Repository
{
    public class WasherRepository : IWasher
    {
        private readonly WashersDbContext _context;
        public WasherRepository(WashersDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddWasher(Washer washer)
        {
            var status = await _context.Washers.FirstOrDefaultAsync(w => w.WasherId == washer.WasherId);
            if (status == null)
            {
                _context.Washers.Add(washer);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteWasher(int washerId)
        {
            var item = await _context.Washers.FirstOrDefaultAsync(w => w.WasherId == washerId);
            if (item != null)
            {
                item.IsActive = false;
                return true;
            }
            return false;
        }

        public async Task<Washer> GetWasherById(int washerId)
        {
            var item = await _context.Washers.FirstOrDefaultAsync(w => w.WasherId == washerId);
            if (item != null)
            {
              return item;
            }
            return null;
        }

        public async Task<IEnumerable<Washer>> GetWashers()
        {
            var item = await _context.Washers.Where(w => w.IsActive == true).ToListAsync();
           
            return item;
        }

        public async Task<bool> UpdateWasher(int id,Washer washer)
        {
            var status = await _context.Washers.FirstOrDefaultAsync(w => w.WasherId == id);
            if (status != null)
            {    
                status.WasherId = washer.WasherId;
                status.WasherName = washer.WasherName;
                status.Email = washer.Email;
                status.PhoneNo = washer.PhoneNo;
                status.IsActive = washer.IsActive;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }




    }
}
