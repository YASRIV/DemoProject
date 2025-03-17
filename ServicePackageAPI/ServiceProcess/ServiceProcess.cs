using ServicePackageAPI.Interface;
using Car_Wash_Library.Models;

namespace ServicePackageAPI.ServiceProcess
{
    public class Process
    {
        private readonly IServicePackage _servicePackage;
        public Process(IServicePackage servicePackage)
        {
            _servicePackage = servicePackage;
        }
        public async Task<bool> AddService(ServicePlan service)
        {
            return await _servicePackage.AddService(service);
        }
        public async Task<bool> UpdateService(int id,ServicePlan service)
        {
            return await _servicePackage.UpdateService(id,service);
        }
        public async Task<bool> DeleteService(int ServiceId)
        {
            return await _servicePackage.DeleteService(ServiceId);
        }
        public async Task<ServicePlan> GetServiceById(int ServiceId)
        {
            return await _servicePackage.GetServiceById(ServiceId);
        }
        public async Task<IEnumerable<ServicePlan>> GetServices()
        {
            return await _servicePackage.GetServices();
        }


    }
}
