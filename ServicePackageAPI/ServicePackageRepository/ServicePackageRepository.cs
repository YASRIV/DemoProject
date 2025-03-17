using Car_Wash_Library.Models;
using Microsoft.EntityFrameworkCore;
using ServicePackageAPI.Interface;

namespace ServicePackageAPI.ServicePackageRepository
{
    public class ServicePackageRepository : IServicePackage
    {
        private readonly ServicePackageDbContext _context;

        public ServicePackageRepository(ServicePackageDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddService(ServicePlan service)
        {
            var status = await _context.Services.FirstOrDefaultAsync(w => w.ServiceId == service.ServiceId);
            if (status == null)
            {
                _context.Services.Add(service);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteService(int ServiceId)
        {
            var item = await _context.Services.FirstOrDefaultAsync(w => w.ServiceId == ServiceId);
            if (item != null)
            {
                item.IsActive = false;
                return true;
            }
            return false;
        }

        public async Task<ServicePlan> GetServiceById(int ServiceId)
        {
            var item = await _context.Services.FirstOrDefaultAsync(w => w.ServiceId == ServiceId);
            if (item != null)
            {
                return item;
            }
            return null;
        }

        public async Task<IEnumerable<ServicePlan>> GetServices()
        {
            var item = await _context.Services.Where(w => w.IsActive == true).ToListAsync();

            return item;
        }

        public async Task<bool> UpdateService(int id,ServicePlan service)
        {
            var status = await _context.Services.FirstOrDefaultAsync(w => w.ServiceId == id);
            if (status != null)
            {
                status.ServiceId = service.ServiceId;
                status.ServiceName = service.ServiceName;
                status.ServiceDescription = service.ServiceDescription;
                status.Price = service.Price;
                status.IsActive = service.IsActive;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
