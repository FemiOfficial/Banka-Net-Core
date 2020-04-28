using System;
using AutoMapper;
using System.Threading.Tasks;
using banka_net_core.Models;
using banka_net_core.Dtos.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using banka_net_core.Services.Accounts;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Logging;

namespace banka_net_core.Data.Repositories.Auth
{
    public class AuthRepository : IAuthRepository
    {
        DataContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public AuthRepository(DataContext context, IConfiguration configuration, IMapper mapper, ILogger<AuthRepository> logger) {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<User> Register(UserRegisterDto newuser, string password)
        {   
            try
            {
                ServiceResponse<UserAuthDto> response = new ServiceResponse<UserAuthDto>();
                CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

                User user = _mapper.Map<User>(newuser);

                if(await UserExists(user.Email) == true) 
                {
                    return null;
                }

                user.createdAt = DateTime.Now;
                user.updatedAt = DateTime.Now;
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;

                await _context.Users.AddAsync(user);

                await _context.SaveChangesAsync();

                return user;
            }
            catch (Exception ex)
            {

                _logger.Log(LogLevel.Error, ex.StackTrace);
                _logger.Log(LogLevel.Error, ex.Message);

                return null;
          
            }


            
        }

        public async Task<ServiceResponse<UserAuthDto>> Login(string email, string password) {

            ServiceResponse<UserAuthDto> response = new ServiceResponse<UserAuthDto>();
            User user = await _context.Users.FirstOrDefaultAsync(i => i.Email.ToLower().Equals(email));

            if(user == null) 
            {
                response.Status = ServiceResponseCodes.NotFound;
                response.Message = "Invalid Email Address";
                response.Data = null;
            } else if (VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt) == false) {
                response.Status = ServiceResponseCodes.BadRequest;
                response.Message = "Invalid Password";
                response.Data = null;
            } else {
                response.Status = ServiceResponseCodes.Success;
                response.Message = "user successfully logged in";
                response.Data = new UserAuthDto 
                {
                    Email = user.Email,
                    Firstname = user.Firstname,
                    Lastname = user.Lastname,
                    Token = GenerateToken(user)
                };
            }

            return response;

        }

        public async Task<bool> UserExists(string email)
        {
            if (await _context.Users.AnyAsync(x => x.Email.ToLower() == email)) 
            {
                return true;
            }

            return false;
        }

        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt) 
        {
            using (var hmac = new HMACSHA512()) 
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            }
        }


        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public string GenerateToken(User user) 
        {
            List<Claim> claims = new List<Claim> 
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Firstname + " " + user.Lastname),
                new Claim(ClaimTypes.Role, user.Type),
                new Claim(ClaimTypes.Email, user.Email)
            };


            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = System.DateTime.Now.AddHours(2),
                SigningCredentials = creds
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}