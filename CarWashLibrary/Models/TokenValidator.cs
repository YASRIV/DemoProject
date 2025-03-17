using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Car_Wash_Library.Models
{
    public class TokenValidator
    {
        private readonly ILogger<TokenValidator> _logger;
        public TokenValidator(ILogger<TokenValidator> logger)
        {
            _logger = logger;
        }
        public async Task<AuthResponse> Validate(string token)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8802");
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var response = await client.GetAsync("/api/accounts/validate");
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Validation succeeded");
                    var user = await response.Content.ReadFromJsonAsync<AuthResponse>();
                    return user;
                }
                else
                {
                    _logger.LogError("Validation Failed");
                }
            }
            return null;
        }
    }
}
