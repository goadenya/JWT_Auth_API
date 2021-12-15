using AuthAPI.Models;
using System.Threading.Tasks;

namespace AuthAPI.Services
{
    public interface IAuthManager
    {
        Task<LoginResponse> SignInUser(LoginModel loginModel);
    }
}
