using ECommerce.Application.Abstract;
using ECommerce.Application.Repositories.Abstracts;
using ECommerce.Domain.Entities;
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
        public async Task Get()
        {
            _ = _productWriteRepository.AddRangeAsycn(new()
            {
                new() {Id= 4,Name="Bardak",Price=100,Stock=50,CreatedDate=DateTime.UtcNow},
                 new() {Id= 5,Name="Bardak 1",Price=150,Stock=20,CreatedDate=DateTime.UtcNow},  
                  new() {Id= 6,Name="Bardak 2",Price=200,Stock=57,CreatedDate=DateTime.UtcNow},
            });
            await _productWriteRepository.SaveAsycn();
        }

        [HttpGet("getById")]
        public async Task<IActionResult> Get(int id)
        {
            Product product = await _productReadRepository.GetByIdAsycn(id);
            return Ok(product);
        }
    }
}
