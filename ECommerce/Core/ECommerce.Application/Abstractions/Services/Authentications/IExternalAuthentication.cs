using ECommerce.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Abstractions.Services.Authentications
{
    public interface IExternalAuthentication
    {
        Task<DTOs.Token> GoogleLoginAsycn(string idToken,int accessTokenLifeTime);
    }
}
