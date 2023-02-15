using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Commands.ProductImage.DeleteProductImage
{
    public class DeleteProductImageQueryRequest : IRequest<DeleteProductImageQueryResponse>
    {
        public int Id { get; set; }
        public int? ImageId { get; set; }
    }
}
