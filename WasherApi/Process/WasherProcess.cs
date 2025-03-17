using WasherApi.Interface;
using Car_Wash_Library.Models;
using WasherApi.Repository;
using System.Net.Http;
namespace WasherApi.Process
{
    public class WasherProcess
    {
        private readonly IWasher _repo;
        private readonly HttpClient _httpClient;
        public WasherProcess(IWasher repo, HttpClient httpClient)
        {
            _repo = repo;
            _httpClient = httpClient;
        }

        public async Task<bool> AddWasher(Washer washer)
        {
            return await _repo.AddWasher(washer);
        }

        public async Task<bool> UpdateWasher(int id,Washer washer)
        {
            return await _repo.UpdateWasher(id,washer);
        }

        public async Task<bool> DeleteWasher(int washerId)
        {
            return await _repo.DeleteWasher(washerId);
        }

        public async Task<Washer> GetWasherById(int washerId)
        {
            return await _repo.GetWasherById(washerId);
        }

        public async Task<IEnumerable<Washer>> GetWashers()
        {
            return await _repo.GetWashers();
        }



        public async Task<bool> AcceptWashRequest(int requestId, int washerId)
        {
            var response = await _httpClient.PutAsync($"http://localhost:8803/api/Order/AcceptWashRequest/{requestId}?washerId={washerId}", null);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> HandleWashRequest(int requestId)
        {
            var response = await _httpClient.PutAsync($"http://localhost:8803/api/Order/HandleWashRequest/{requestId}", null);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateWashRequest(int requestId)
        {
            var response = await _httpClient.PutAsync($"http://localhost:8803/api/Order/UpdateWashRequestStatus/{requestId}", null);
            return response.IsSuccessStatusCode;
        }



    }
}
