using AuthAPI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyPortfolio_AuthAPI.Data.Entities;
using MyPortfolio_AuthAPI.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyPortfolio_AuthAPI.Services
{
    public class AuthManager : IAuthManager
    {
        private readonly IConfiguration _config;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public AuthManager(IConfiguration config, SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _config = config;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public async Task<LoginResponse> SignInUser(LoginModel loginModel)
        {
            User user = loginModel.User.Contains("@") ? await _userManager.FindByEmailAsync(loginModel.User) : await _userManager.FindByNameAsync(loginModel.User);
            if (user != null)
            {
                var signInResult = await _signInManager.CheckPasswordSignInAsync(user, loginModel.Password, false);

                if (signInResult.Succeeded)
                {
                    var userRoles = await _userManager.GetRolesAsync(user);

                    var tokenHandler = new JwtSecurityTokenHandler();

                    var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);

                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[] {
                            new Claim(ClaimTypes.Name, user.UserName),
                            new Claim(ClaimTypes.Email, user.Email),
                            new Claim(ClaimTypes.Role, userRoles.First().ToString())
                        }),

                        Expires = DateTime.UtcNow.AddHours(3),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                        Issuer = "AuthApi"
                    };

                    var token = tokenHandler.CreateToken(tokenDescriptor);

                    var tokenString = tokenHandler.WriteToken(token);

                    var response = new LoginResponse
                    {
                        Token = tokenString,
                        LoginIsSuccess = true,
                        ResponseMessage = "Login was successful."
                    };

                    return response;
                }
                else
                {
                    var response = new LoginResponse
                    {
                        LoginIsSuccess = false,
                        ResponseMessage = "Login failed for unknown reasons."
                    };
                    return response;
                }
            }
            else
            {
                var response = new LoginResponse
                {
                    LoginIsSuccess = false,
                    ResponseMessage = "The username or email address that you've entered doesn't match any account. Sign up for an account."
                };
                return response;
            }
        }
    }
}
