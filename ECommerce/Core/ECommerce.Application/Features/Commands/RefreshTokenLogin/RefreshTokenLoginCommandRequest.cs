using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Commands.RefreshTokenLogin
{
    public class RefreshTokenLoginCommandRequest :IRequest<RefreshTokenLoginCommandResponse>
    {
        public string RefrestToken { get; set; }
    }
}
