﻿using ECommerce.Application.Abstractions.Services;
using ECommerce.Application.Repositories.Abstracts;
using ECommerce.Application.ViewModels.Basket;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Persistence.Services
{
    public class BasketService : IBasketService
    {
        readonly IHttpContextAccessor _httpContextAccessor;
        readonly UserManager<User> _userManager;
        readonly IOrderReadRepository _orderReadRepository;
        readonly IBasketWriteRepository _basketWriteRepository;
        readonly IBasketReadRepository _basketReadRepository;
        readonly IBasketItemWriteRepository _basketItemWriteRepository;
        readonly IBasketItemReadRepository _basketItemReadRepository;

        public BasketService(IHttpContextAccessor httpContextAccessor, UserManager<User> userManager, IOrderReadRepository orderReadRepository, IBasketWriteRepository basketWriteRepository, IBasketItemWriteRepository basketItemWriteRepository, IBasketItemReadRepository basketItemReadRepository, IBasketReadRepository basketReadRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _orderReadRepository = orderReadRepository;
            _basketWriteRepository = basketWriteRepository;
            _basketItemWriteRepository = basketItemWriteRepository;
            _basketItemReadRepository = basketItemReadRepository;
            _basketReadRepository = basketReadRepository;
        }

        private async Task<Basket?> ContextUser()
        {
            var userName = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
            if(!string.IsNullOrEmpty(userName) )
            {
                User? user = await _userManager.Users
                    .Include(u=>u.Baskets)
                    .FirstOrDefaultAsync(u=>u.UserName == userName);

                var _basket = from basket in user.Baskets
                              join order in _orderReadRepository.Table
                              on basket.Id equals order.Id into BasketOrders
                              from order in BasketOrders.DefaultIfEmpty()
                              select new
                              {
                                  Basket = basket,
                                  Order =  order
                              };
                Basket? targetBasket = null;
                if (_basket.Any(b => b.Order is null))
                    targetBasket = _basket.FirstOrDefault(b => b.Order is null)?.Basket;
                else
                {
                    targetBasket = new();
                    user.Baskets.Add(targetBasket);
                }
                    
                await _basketWriteRepository.SaveAsycn();
                return targetBasket;
            }
            throw new Exception("Beklenmeyen bir hatayla karşılaşıldı.");
        }

        public async Task AddItemToBasketAsync(VM_Create_BasketItem basketItem)
        {
            Basket? basket = await ContextUser();  
            if(basket!=null)
            {
                BasketItem _basketItem = await _basketItemReadRepository.GetSingleAsync(bi=>bi.BasketId == basket.Id && bi.ProductId == basketItem.ProductId);
                if (_basketItem != null)
                    _basketItem.Quantity++;
                else
                    await _basketItemWriteRepository.AddAsycn(new()
                    {
                        BasketId = basket.Id, 
                        ProductId = basketItem.ProductId,
                        Quantity = basketItem.Quantity
                    });
                await _basketItemWriteRepository.SaveAsycn();
            }
        }

        public async Task<List<BasketItem>> GetBasketItemsAsync()
        {
            Basket? basket = await ContextUser();
            Basket? result = await _basketReadRepository.Table.
                Include(b=>b.BasketItems)
                .ThenInclude(bi=>bi.Product)
                .FirstOrDefaultAsync(b=>b.Id == basket.Id);
            return result.BasketItems.ToList();
        }

        public async Task RemoveBasketItemAsync(int basketItemId)
        {
            BasketItem? basketItem = await _basketItemReadRepository.GetByIdAsycn(basketItemId);
            if (basketItem != null)
            {
                _basketItemWriteRepository.Remove(basketItem);
                await _basketItemWriteRepository.SaveAsycn();
            }
        }

        public async Task UpdateQuantityAsync(VM_Update_BasketItem basketItem)
        {
            BasketItem? _basketItem = await _basketItemReadRepository.GetByIdAsycn(basketItem.BasketItemId);
            if(_basketItem != null)
            {
                _basketItem.Quantity = basketItem.Quantity;
                await _basketItemWriteRepository.SaveAsycn();
            }
        }

        public Basket? GetUserActiveBasket
        {
            get
            {
                Basket? basket = ContextUser().Result;
                return basket;
            }
        }
    }
}
