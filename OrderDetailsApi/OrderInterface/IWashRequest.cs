using Car_Wash_Library.Models;

namespace OrderDetailsApi.OrderInterface
{
    public interface IWashRequest
    {
        Task<bool> AddWashRequest(WashRequest washRequest);
        Task<bool> UpdateWashRequest(int requestid);
        Task<bool> DeleteWashRequest(int requestId);
        Task<WashRequest> GetWashRequestById(int requestId);
        Task<IEnumerable<WashRequest>> GetWashRequests();
        Task<bool> AcceptWashRequest(int requestId, int washerId);
        Task<bool> HandleWashRequest(int requestId);
        
    }
}
