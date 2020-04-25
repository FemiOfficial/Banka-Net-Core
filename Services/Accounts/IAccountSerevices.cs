using System.Threading.Tasks;
using banka_net_core.Models;
using banka_net_core.Dtos.User;

namespace banka_net_core.Services.Accounts
{
    public interface IAccountSerevices
    {
        Task<ServiceResponse<UserAuthDto>> CreateAccount(User user, float openingBalance);


    }
}