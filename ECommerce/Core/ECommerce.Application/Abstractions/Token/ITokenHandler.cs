using ECommerce.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Abstractions.Token
{
    public interface ITokenHandler
    {
        DTOs.Token CreateAccessToken(int minute);
    }
}
