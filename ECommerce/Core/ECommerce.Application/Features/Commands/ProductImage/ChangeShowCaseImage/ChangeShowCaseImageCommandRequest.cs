using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Commands.ProductImage.ChangeShowCaseImage
{
    public class ChangeShowCaseImageCommandRequest : IRequest<ChangeShowCaseImageCommandResponse>
    {
        public int ImageId { get; set; }
        public int ProductId { get; set; }
    }
}
