using System.Threading.Tasks;
using banka_net_core.Models;
using banka_net_core.Dtos.User;

namespace banka_net_core.Data.Repositories.Auth
{
    public interface IAuthRepository
    {
        Task<ServiceResponse<UserAuthDto>> Register(User user, string password);
        Task<ServiceResponse<UserAuthDto>> Login(string email, string password);
        Task<bool> UserExists(string email);
    }
}