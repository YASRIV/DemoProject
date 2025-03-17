using static WasherApi.Interface.IWasher;
using Car_Wash_Library.Models;

namespace WasherApi.Interface
{


    public interface IWasher
    {
        Task<bool> AddWasher(Washer washer);
        Task<bool> UpdateWasher(int id,Washer washer);
        Task<bool> DeleteWasher(int washerId);
        Task<Washer> GetWasherById(int washerId);
        Task<IEnumerable<Washer>> GetWashers();
        //
        //Task<bool> AcceptRequest(int requestId);
    }
}    

