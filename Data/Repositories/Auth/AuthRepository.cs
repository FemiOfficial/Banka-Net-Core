using System;
using System.Threading.Tasks;
using banka_net_core.Models;
using banka_net_core.Dtos.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace banka_net_core.Data.Repositories.Auth
{
    public class AuthRepository : IAuthRepository
    {
        DataContext _context;
        private readonly IConfiguration _configuration;

        public AuthRepository(DataContext context, IConfiguration configuration) {
            context = _context;
            configuration = _configuration;
        }

        public async Task<ServiceResponse<UserAuthDto>> Register(User user, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse<UserAuthDto>> Login(string email, string password) {
            throw new NotImplementedException();
        }

        public async Task<bool> UserExists(string email)
        {
            throw new NotImplementedException();
        }
    }
}