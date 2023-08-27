using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BLL.Models.Auth;
using BLL.Models.UserModels;
using Common.Helpers;
using DAL.Consts;
using DAL.Entities;

namespace BLL.AutoMapper.Profiles
{
    public class AuthProfile : AutoMapperProfile
    {
        public AuthProfile() : base()
        {
            CreateMap<LoginRequest, CredentialModel>();
            CreateMap<RegisterRequest, RegisterModel>();
            CreateMap<RegisterModel, User>()
                .ForMember(k => k.PasswordHash, f => f.MapFrom(m => HashHelper.GetHash(m.Password)))
                .ForMember(k => k.RoleId, m => m.MapFrom(f => Roles.UserId));
        }
    }
}
