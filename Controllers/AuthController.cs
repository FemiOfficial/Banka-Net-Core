using Microsoft.AspNetCore.Mvc;
using banka_net_core.Data.Repositories.Auth;
using banka_net_core.Services.Accounts;
using banka_net_core.Dtos.User;
using banka_net_core.Models;
using System.Threading.Tasks;

namespace banka_net_core.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAccountSerevices _accountServices;
        private readonly IAuthRepository _authrepo;
        public AuthController(IAccountSerevices accountSerevices, IAuthRepository authRepository) 
        {
            _accountServices = accountSerevices;
            _authrepo = authRepository;

        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserRegisterDto user) 
        {
            User newUser = await _authrepo.Register(user, user.Password);
            
            ServiceResponse<UserAuthDto> response = await _accountServices.CreateAccount(newUser, user.AccountOpeningBalance);

            if(response.Status != ServiceResponseCodes.Created) 
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost("Signin")]
        public async Task<IActionResult> Login(UserLoginDto user) 
        {
            ServiceResponse<UserAuthDto> response = await _authrepo.Login(user.Email, user.Password);

            if(response.Status != ServiceResponseCodes.Created) 
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
};