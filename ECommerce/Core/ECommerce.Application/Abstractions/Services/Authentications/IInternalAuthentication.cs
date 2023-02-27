using ECommerce.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Abstractions.Services.Authentications
{
    public interface IInternalAuthentication
    {
        Task<DTOs.Token> LoginAsycn(string userameOrEmail,string password,int accessTokenLifeTime);
    }
}
