using AuthAPI.Models;
using AuthAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthManager _authManager;

        public AuthController(AuthManager authManager)
        {
            _authManager = authManager;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginModel userLogin)
        {
            if (userLogin != null)
            {
                var loginResponse = await _authManager.SignInUser(userLogin);

                if (loginResponse.LoginIsSuccess == true)
                {
                    return Ok(new
                    {
                        Token = loginResponse.Token,
                        StatusMessage = loginResponse.ResponseMessage
                    });
                }
                else
                {
                    return Unauthorized(new
                    {
                        StatusMessage = loginResponse.ResponseMessage
                    });
                }
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
