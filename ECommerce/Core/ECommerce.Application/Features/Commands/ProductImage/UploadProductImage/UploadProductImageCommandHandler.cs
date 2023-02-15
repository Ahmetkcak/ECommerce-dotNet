using ECommerce.Application.Abstractions.Storage;
using ECommerce.Application.Repositories.Abstracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Commands.ProductImage.UploadProductImage
{
    public class UploadProductImageCommandHandler : IRequestHandler<UploadProductImageCommandRequest, UploadProductImageCommandResponse>
    {
        readonly IProductReadRepository _productReadRepository;
        readonly IProductImageWriteRepository _productImageWriteRepository;
        readonly IStorageService _storageService;

        public UploadProductImageCommandHandler(IStorageService storageService, IProductImageWriteRepository productImageWriteRepository, IProductReadRepository productReadRepository)
        {
            _storageService = storageService;
            _productImageWriteRepository = productImageWriteRepository;
            _productReadRepository = productReadRepository;
        }

        public async Task<UploadProductImageCommandResponse> Handle(UploadProductImageCommandRequest request, CancellationToken cancellationToken)
        {
            List<(string fileName, string pathOrContainerName)> result = await _storageService.UploadAsycn("photo-images", request.Files);

            ECommerce.Domain.Entities.Product product = await _productReadRepository.GetByIdAsycn(request.Id);

            await _productImageWriteRepository.AddRangeAsycn(result.Select(r => new ECommerce.Domain.Entities.ProductImage
            {
                FileName = r.fileName,
                Path = r.pathOrContainerName,
                Storage = _storageService.StorageName,
                Products = new List<ECommerce.Domain.Entities.Product>() { product }
            }).ToList());

            await _productImageWriteRepository.SaveAsycn();
            return new();
        }
    }
}
