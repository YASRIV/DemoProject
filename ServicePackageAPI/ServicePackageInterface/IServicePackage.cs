
using Car_Wash_Library.Models;
namespace ServicePackageAPI.Interface
{
    public interface IServicePackage
    {

        Task<bool> AddService(ServicePlan service);
        Task<bool> UpdateService(int id,ServicePlan service);
        Task<bool> DeleteService(int ServiceId);
        Task<ServicePlan> GetServiceById(int ServiceId);
        Task<IEnumerable<ServicePlan>> GetServices();
    }
}
