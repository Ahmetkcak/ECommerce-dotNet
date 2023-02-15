using ECommerce.Application.Repositories.Abstracts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Commands.ProductImage.DeleteProductImage
{
    public class DeleteProductImageQueryHandler : IRequestHandler<DeleteProductImageQueryRequest, DeleteProductImageQueryResponse>
    {
        readonly IProductReadRepository _productReadRepository;
        readonly IProductWriteRepository _productWriteRepository;

        public DeleteProductImageQueryHandler(IProductReadRepository productReadRepository, IProductWriteRepository productWriteRepository)
        {
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
        }

        public async Task<DeleteProductImageQueryResponse> Handle(DeleteProductImageQueryRequest request, CancellationToken cancellationToken)
        {
            Domain.Entities.Product? product = await _productReadRepository.Table.Include(p => p.ProductImages).FirstOrDefaultAsync(p => p.Id == request.Id);
            Domain.Entities.ProductImage? productImage = product?.ProductImages.FirstOrDefault(p => p.Id == request.ImageId);

            if(productImage != null)
                product?.ProductImages.Remove(productImage);
            await _productWriteRepository.SaveAsycn();
            return new();
        }
    }
}
