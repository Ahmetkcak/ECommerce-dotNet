using ECommerce.Application.Abstractions.Services;
using ECommerce.Application.DTOs.Order;
using ECommerce.Application.Repositories.Abstracts;
using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Persistence.Services
{
    public class OrderService : IOrderService
    {
        readonly IOrderWriteRepository _orderWriteRepository;
        readonly IOrderReadRepository _orderReadRepository;

        public OrderService(IOrderWriteRepository orderWriteRepository, IOrderReadRepository orderReadRepository)
        {
            _orderWriteRepository = orderWriteRepository;
            _orderReadRepository = orderReadRepository;
        }

        public async Task CreateOrderAsync(CreateOrder createOrder)
        {
            var orderCode = (new Random().NextDouble() * 10000).ToString();
            orderCode.Substring(orderCode.IndexOf('.') + 1,orderCode.Length-orderCode.IndexOf(".") - 1);
            await _orderWriteRepository.AddAsycn(new()
            {
                Id = (int)createOrder.BasketId,
                Address = createOrder.Address,
                Description = createOrder.Description,
                OrderCode =orderCode
            });

            await _orderWriteRepository.SaveAsycn();
        }

        public async Task<ListOrder> GetAllOrdersAsync(int page, int size)
        {
            var query = _orderReadRepository.Table.Include(o => o.Basket)
                .ThenInclude(b => b.User)
                .Include(o => o.Basket)
                    .ThenInclude(b => b.BasketItems)
                    .ThenInclude(bi => bi.Product);

            var data = query.Skip(page * size).Take(size);
            return new()
            {
                TotalCount = await query.CountAsync(),
                Orders = await data.Select(o => new
                {
                    Id = o.Id,
                    CreatedDate = o.CreatedDate,
                    OrderCode = o.OrderCode,
                    TotalPrice = o.Basket.BasketItems.Sum(bi => bi.Product.Price * bi.Quantity),
                    UserName = o.Basket.User.UserName
                }).ToListAsync()
            };
        }

        public async Task<SingleOrder> GetOrderByIdAsync(int id)
        {
            var data = await _orderReadRepository.Table 
                            .Include(o => o.Basket) 
                                .ThenInclude(b => b.BasketItems)
                                    .ThenInclude(bi => bi.Product)
                                        .FirstOrDefaultAsync(o => o.Id == id);
            return new()
            {
                Id = data.Id,
                BasketItems = data.Basket.BasketItems.Select(bi => new
                {
                    bi.Product.Name,
                    bi.Product.Price,
                    bi.Quantity
                }),
                Address = data.Address,
                CreatedDate = data.CreatedDate,
                Description = data.Description,
                OrderCode = data.OrderCode,
            };
        }
    }
}
