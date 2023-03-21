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
        readonly ICompletedOrderWriteRepository _completedOrderWriteRepository;
        readonly ICompletedOrderReadRepository _completedOrderReadRepository;

        public OrderService(IOrderWriteRepository orderWriteRepository, IOrderReadRepository orderReadRepository, ICompletedOrderWriteRepository completedOrderWriteRepository, ICompletedOrderReadRepository completedOrderReadRepository)
        {
            _orderWriteRepository = orderWriteRepository;
            _orderReadRepository = orderReadRepository;
            _completedOrderWriteRepository = completedOrderWriteRepository;
            _completedOrderReadRepository = completedOrderReadRepository;
        }

        public async Task CompleteOrderAsync(int id)
        {
            Order order = await _orderReadRepository.GetByIdAsycn(id);
            if(order != null)
            {
                await _completedOrderWriteRepository.AddAsycn(new(){OrderId = id});
                await _completedOrderWriteRepository.SaveAsycn();
            }
        }

        public async Task CreateOrderAsync(CreateOrder createOrder)
        {
            var orderCode = (new Random().NextDouble() * 10000).ToString();
            orderCode = orderCode.Substring(orderCode.IndexOf('.') + 1,orderCode.Length-orderCode.IndexOf(".") - 1);
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


            var data2 = from order in data
                        join completedOrder in _completedOrderReadRepository.Table
                        on order.Id equals completedOrder.OrderId into co
                        from _co in co.DefaultIfEmpty()
                        select new
                        {
                            Id = order.Id,
                            CreatedDate = order.CreatedDate,
                            OrderCode = order.OrderCode,
                            Basket = order.Basket,
                            Completed = _co != null ? true : false,
                        };



            return new()
            {
                TotalCount = await query.CountAsync(),
                Orders = await data2.Select(o => new
                {
                    Id = o.Id,
                    CreatedDate = o.CreatedDate,
                    OrderCode = o.OrderCode,
                    TotalPrice = o.Basket.BasketItems.Sum(bi => bi.Product.Price * bi.Quantity),
                    UserName = o.Basket.User.UserName,
                    o.Completed
                }).ToListAsync()
            };
        }

        public async Task<SingleOrder> GetOrderByIdAsync(int id)
        {
            var data = _orderReadRepository.Table
                            .Include(o => o.Basket)
                                .ThenInclude(b => b.BasketItems)
                                    .ThenInclude(bi => bi.Product);
            var data2 = await (from order in data
                        join completedOrder in _completedOrderReadRepository.Table
                        on order.Id equals completedOrder.Id into co
                        from _co in co.DefaultIfEmpty()
                        select new
                        {
                            Id = order.Id,
                            CreatedDate = order.CreatedDate,
                            OrderCode = order.OrderCode,
                            Basket = order.Basket,
                            Completed = _co != null ? true : false,
                            Address = order.Address,
                            Description = order.Description,
                        }).FirstOrDefaultAsync(o => o.Id == id);
                                        
            return new()
            {
                Id = data2.Id,
                BasketItems = data2.Basket.BasketItems.Select(bi => new
                {
                    bi.Product.Name,
                    bi.Product.Price,
                    bi.Quantity
                }),
                Address = data2.Address,
                CreatedDate = data2.CreatedDate,
                Description = data2.Description,
                OrderCode = data2.OrderCode,
                Completed = data2.Completed,
            };
        }
    }
}
