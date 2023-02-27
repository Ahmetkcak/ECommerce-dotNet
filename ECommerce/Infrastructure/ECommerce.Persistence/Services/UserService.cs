﻿using ECommerce.Application.Abstractions.Services;
using ECommerce.Application.DTOs.User;
using ECommerce.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Persistence.Services
{
    public class UserService : IUserService
    {
        readonly UserManager<User> _userManager;

        public UserService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<CreateUserResponse> CreateAsync(CreateUser model)
        {
            IdentityResult result = await _userManager.CreateAsync(new()
            {
                Name = model.Name,
                Surname = model.Surname,
                UserName = model.Username,
                Email = model.Email,
            }, model.Password);

            CreateUserResponse response = new() { Succeded = result.Succeeded };
            if (result.Succeeded)
                response.Message = "Kullanıcı oluşturuldu";
            else
                foreach (var error in result.Errors)
                    response.Message += $"{error.Code} - {error.Description}";
            return response;
        }
    }
}