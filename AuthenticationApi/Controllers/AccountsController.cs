using AuthenticationApi.Process;
using Car_Wash_Library.Models;
using Car_Wash_Library.DTOClass;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("allowAllPolicy")]
    public class AccountsController : ControllerBase
    {

        private readonly AuthProcess _repository;

        public AccountsController(AuthProcess repository)
        {
            _repository = repository;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(AuthRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                var error1 = new FaultContract()
                {
                    FaultId = 1,
                    FaultDescription = "Either name or password is empty",
                    FaultName = "Missing Values",
                    FaultType = "Authentication"
                };
                return BadRequest(error1);
            }
            var user = await _repository.ValidateUserAndGenerateResponse(request);
            if (user != null)
            {
                return Ok(user);
            }

            var error2 = new FaultContract()
            {
                FaultId = 2,
                FaultDescription = "Incorrect credentials",
                FaultName = "Unauthorized",
                FaultType = "Authentication"
            };
            return Unauthorized(error2);
        }
        [HttpGet("validate")]
        public async Task<ActionResult<AuthResponse>> Validate()
        {
            try
            {
                var user = await _repository.ValidateTokenAndReturnUser();
                if (user != null)
                {
                    return Ok(user);
                }
                else
                    return Unauthorized();
            }
            catch (Exception ex)
            {
            }
            return Unauthorized();
        }
        [HttpPost("Signup")]
        public async Task<IActionResult> Signup([FromBody]SignupDto signupDto)
        {
            try
            {
                if (await _repository.RegisterUser(signupDto))
                {
                    return Ok("user added successfully");
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
