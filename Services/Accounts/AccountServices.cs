using System;
using System.Linq;
using AutoMapper;
using System.Threading.Tasks;
using banka_net_core.Models;
using banka_net_core.Dtos.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using banka_net_core.Data;
using banka_net_core.Data.Repositories.Auth;
using Microsoft.Extensions.Logging;

namespace banka_net_core.Services.Accounts
{
    public class AccountServices : IAccountSerevices
    {

        DataContext _context;
        private readonly IAuthRepository _authRepository;
        private readonly ILogger _logger;
        public AccountServices(IAuthRepository authRepository, ILogger<AccountServices> logger, DataContext context) {
            _authRepository = authRepository;
            _logger = logger;
            _context = context;

        }
        public string GenerateRandomAccountNumber(int length) {
            const string chars = "0123456789";
            Random _random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[_random.Next(s.Length)]).ToArray());
        }

        public async Task<ServiceResponse<UserAuthDto>> CreateAccount(User user, float openingBalance) 
        {
            Models.Accounts newAccount = new Models.Accounts();
            ServiceResponse<UserAuthDto> response = new ServiceResponse<UserAuthDto>();

            if(user == null) 
            {
                response.Message = "An error occurred will registering user";
                response.Status = ServiceResponseCodes.ServerError;
                return response;
            }

            newAccount.User = user;
            newAccount.UserId = user.Id;
            newAccount.AccountBalance = openingBalance;
            newAccount.AccountOpeningBalance = openingBalance;
            newAccount.AccountNumber = GenerateRandomAccountNumber(10);

            newAccount.createdAt = DateTime.Now;
            newAccount.updatedAt = DateTime.Now;

            try
            {
                await _context.Accounts.AddAsync(newAccount);

                await _context.SaveChangesAsync();

                response.Message = "user account created successully";

                response.Status = ServiceResponseCodes.Created;

                response.Data = new UserAuthDto 
                {
                    Email = user.Email,
                    Firstname = user.Firstname,
                    Lastname = user.Lastname,
                    Token = _authRepository.GenerateToken(user)
                };

                return response;
            }
            catch (Exception ex)
            {

                _logger.Log(LogLevel.Error, ex.StackTrace);
                _logger.Log(LogLevel.Error, ex.Message);

                response.Message = "An error occurred will registering user";
                response.Status = ServiceResponseCodes.ServerError;
                return response;
            }

        }   



    }
}