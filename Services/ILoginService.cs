using System.Threading.Tasks;
using TasksListAPI.Models;

namespace TasksListAPI.Services
{
    public interface ILoginService
    {
        Task<UserModel> AuthenticateUserAsync(UserModel user);
        string GenerateJSONWebToken(UserModel user);
    }
}