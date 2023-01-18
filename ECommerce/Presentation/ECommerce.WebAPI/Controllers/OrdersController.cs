using ECommerce.Application.Repositories.Abstracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private IOrderWriteRepository _orderWriteRepository;

        public OrdersController(IOrderWriteRepository orderWriteRepository)
        {
            _orderWriteRepository = orderWriteRepository;
        }

        [HttpGet]
        public async Task Add()
        {
            await _orderWriteRepository.AddAsycn(new() { Description="Evlere özel",Address="Antalya",CustomerId=2});
            await _orderWriteRepository.AddAsycn(new() { Description = "Çalışanlara özel", Address = "Bursa", CustomerId = 2 });
            await _orderWriteRepository.SaveAsycn();
        }
    }
}
