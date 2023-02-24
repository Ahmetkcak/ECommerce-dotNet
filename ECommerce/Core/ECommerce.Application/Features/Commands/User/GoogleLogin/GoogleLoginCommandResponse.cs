using ECommerce.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Commands.User.GoogleLogin
{
    public class GoogleLoginCommandResponse
    {
        public Token Token { get; set; }
    }
}
