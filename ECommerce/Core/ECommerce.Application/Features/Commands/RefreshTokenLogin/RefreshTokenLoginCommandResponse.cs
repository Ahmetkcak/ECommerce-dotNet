using ECommerce.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Commands.RefreshTokenLogin
{
    public class RefreshTokenLoginCommandResponse
    {
        public Token Token { get; set; }
    }
}
