using Car_Wash_Library.Models;
using static AuthenticationApi.Repository.UserRespository;
using Microsoft.AspNetCore.Http;
using Car_Wash_Library.DTOClass;
using System.Net.Http;


namespace AuthenticationApi.Process
{
    public class AuthProcess
    {
        private readonly IUserRepository _repository;
        private readonly TokenManager _tokenManager;
        private readonly IHttpContextAccessor _context;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public AuthProcess(IUserRepository repository, TokenManager mgr, IHttpContextAccessor ctx, HttpClient httpClient, IConfiguration configuration)
        {
            _repository = repository;
            _tokenManager = mgr;
            _context = ctx;
            _httpClient = httpClient;
            _configuration = configuration;
        }
        public async Task<AuthResponse> ValidateUserAndGenerateResponse(AuthRequest request)
        {
            var user = await _repository.Authenticate(request);
            Role role = null!;
            string token = string.Empty;
            if (user != null)
            {
                role = await _repository.GetRoleForUser(user.UserId);
                token = _tokenManager.GetJwtToken(user, role);
                return new AuthResponse(user, role, token);
            }
            return null!;
        }
        public async Task<AuthResponse> ValidateTokenAndReturnUser()
        {

            var userEmail = _context.HttpContext?.Items["Email"]?.ToString();
            if (string.IsNullOrEmpty(userEmail))
            {
                throw new ArgumentException("Invalid token");
            }
            //Based on the email, call the repository to get the user
            var user = await _repository.GetUserByEmail(userEmail);
            if (user != null)
            {
                var role = await _repository.GetRoleForUser(user.UserId);

                return new AuthResponse(user, role, "");
            }
            return null!;
        }

        public async Task<bool> RegisterUser(SignupDto signupDto)
        {
            // Check if email already exists in AuthDB
            if (_repository.IsEmailExists(signupDto.Email))
                throw new Exception("Email already exists!");
            if (signupDto.Role != "Admin" && signupDto.Role != "Customer" && signupDto.Role != "Washer")
                return false;

            // Hash password for security
            //string passwordHash = BCrypt.Net.BCrypt.HashPassword(signupDto.Password);

            // Store User Details in AuthDB
            var newUser = new User
            {
                UserName = signupDto.Name,
                Email = signupDto.Email,
                PasswordHash = signupDto.Password,
                IsActive = true
            };

            var res = await _repository.RegisterUser(newUser, signupDto.Role);
            if (!res) return res;

            // API URLs from configuration
            string customerApiUrl = _configuration["CustomerApi:BaseUrl"];
            string washerApiUrl = _configuration["WasherApi:BaseUrl"];

            if (signupDto.Role == "Customer")
            {
                var newCustomer = new
                {
                    Name = signupDto.Name,
                    Email = signupDto.Email,
                    PhoneNumber = signupDto.PhoneNumber,
                    Address = signupDto.Address,
                    City = signupDto.City,
                    ZipCode = signupDto.ZipCode,
                    IsActive = true
                };

                var response = await _httpClient.PostAsJsonAsync($"{customerApiUrl}/api/customers/AddCustomer", newCustomer);
                if (!response.IsSuccessStatusCode)
                    throw new Exception("Failed to register customer.");
            }
            else if (signupDto.Role == "Washer")
            {
                var newWasher = new
                {
                    WasherName = signupDto.Name,
                    Email = signupDto.Email,
                    PhoneNo = signupDto.PhoneNumber,
                    IsActive = true
                };

                var response = await _httpClient.PostAsJsonAsync($"{washerApiUrl}/api/washers/AddWasher", newWasher);
                if (!response.IsSuccessStatusCode)
                    throw new Exception("Failed to register washer.");
            }

            return true;
        }

    }
}
