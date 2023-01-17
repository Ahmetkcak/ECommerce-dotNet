using ECommerce.Application.Abstract;
using ECommerce.Application.Repositories.Abstracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IProductReadRepository _productReadRepository;

        public ProductsController(IProductWriteRepository productWriteRepository, IProductReadRepository productReadRepository)
        {
            _productWriteRepository = productWriteRepository;
            _productReadRepository = productReadRepository;
        }

        [HttpGet]
        public async void Get()
        {
            _ = _productWriteRepository.AddRangeAsycn(new()
            {
                new() {Id= 1,Name="Bardak",Price=100,Stock=50,CreatedDate=DateTime.UtcNow},
                 new() {Id= 2,Name="Bardak 1",Price=150,Stock=20,CreatedDate=DateTime.UtcNow},
                  new() {Id= 3,Name="Bardak 2",Price=200,Stock=57,CreatedDate=DateTime.UtcNow},
            });
            await _productWriteRepository.SaveAsycn();
        }
    }
}
