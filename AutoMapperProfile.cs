using AutoMapper;
using banka_net_core.Dtos.User;
using banka_net_core.Models;

namespace banka_net_core
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile() {
            CreateMap<UserRegisterDto, User>();
        }   
    }
}