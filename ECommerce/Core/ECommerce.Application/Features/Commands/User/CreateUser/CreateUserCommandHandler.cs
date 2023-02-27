using ECommerce.Application.Abstractions.Services;
using ECommerce.Application.DTOs.User;
using ECommerce.Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Commands.User.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommandRequest, CreateUserCommandResponse>
    {
        readonly IUserService _userService;

        public CreateUserCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<CreateUserCommandResponse> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
        {
            CreateUserResponse response = await _userService.CreateAsync(new()
            {
                Email = request.Email,
                Surname = request.Surname,
                Name = request.Name,
                Password = request.Password,
                Username = request.Username
            });

            return new()
            {
                Message = response.Message,
                Succeded = response.Succeded,
            };
        }
    }
}
