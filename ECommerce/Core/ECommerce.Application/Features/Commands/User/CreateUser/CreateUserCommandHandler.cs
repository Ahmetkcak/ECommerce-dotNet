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
        readonly UserManager<Domain.Entities.Identity.User> _userManager;

        public CreateUserCommandHandler(UserManager<Domain.Entities.Identity.User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<CreateUserCommandResponse> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
        {
            IdentityResult result = await _userManager.CreateAsync(new()
            {
                Name = request.Name,
                Surname = request.Surname,
                UserName = request.Username,
                Email = request.Email,
            }, request.Password);

            CreateUserCommandResponse response = new() { Succeded = result.Succeeded};
            if (result.Succeeded)
                response.Message = "Kullanıcı oluşturuldu";
            else
                foreach (var error in result.Errors)
                    response.Message += $"{error.Code} - {error.Description}";
            return response;
        }
    }
}
