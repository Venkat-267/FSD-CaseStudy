using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PayRollManagement.DTO;
using PayRollManagement.Interface;

namespace PayRollManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        public AuthController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegistrationDto dto)
        {
            try
            {
                var userId = await _userRepo.RegisterUser(dto);
                return Ok(new { Message = "User Registered Successfully!", UserId = userId });

            }
            catch(Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            try
            {
                var user = await _userRepo.LoginUser(dto);
                return Ok(new { Message = "Login successful", UserId = user.UserId, Role = user.RoleId });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { Error = ex.Message });
            }
        }
    }
}
