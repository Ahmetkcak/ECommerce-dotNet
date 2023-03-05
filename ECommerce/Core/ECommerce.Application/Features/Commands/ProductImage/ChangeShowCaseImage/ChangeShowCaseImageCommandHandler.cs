using ECommerce.Application.Repositories.Abstracts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Commands.ProductImage.ChangeShowCaseImage
{
    public class ChangeShowCaseImageCommandHandler : IRequestHandler<ChangeShowCaseImageCommandRequest, ChangeShowCaseImageCommandResponse>
    {
        readonly IProductImageWriteRepository _productImageWriteRepository;

        public ChangeShowCaseImageCommandHandler(IProductImageWriteRepository productImageWriteRepository)
        {
            _productImageWriteRepository = productImageWriteRepository;
        }

        public async Task<ChangeShowCaseImageCommandResponse> Handle(ChangeShowCaseImageCommandRequest request, CancellationToken cancellationToken)
        {
            var query = _productImageWriteRepository.Table
                .Include(p => p.Products)
                .SelectMany(p => p.Products, (pif, p) => new
                {
                    pif,
                    p
                });

            var data = await query.FirstOrDefaultAsync(p => p.p.Id == request.ProductId && p.pif.Showcase);

            if(data != null)
                data.pif.Showcase = false;

            var image = await query.FirstOrDefaultAsync(p => p.pif.Id == request.ImageId);
            if (image != null)
                image.pif.Showcase = true;

            await _productImageWriteRepository.SaveAsycn();
            return new();
        }
    }
}
