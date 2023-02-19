using ECommerce.Application.Exceptions;
using ECommerce.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Commands.User.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommandRequest, LoginUserCommandResponse>
    {
        readonly UserManager<Domain.Entities.Identity.User> _userManager;
        readonly SignInManager<Domain.Entities.Identity.User> _signInManager;

        public LoginUserCommandHandler(UserManager<Domain.Entities.Identity.User> userManager, SignInManager<Domain.Entities.Identity.User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<LoginUserCommandResponse> Handle(LoginUserCommandRequest request, CancellationToken cancellationToken)
        {
            Domain.Entities.Identity.User user = await _userManager.FindByNameAsync(request.UsernameOrMail);
            if (user == null)
                user = await _userManager.FindByEmailAsync(request.UsernameOrMail);

            if (user == null)
                throw new NotFounUserException("Kullanıcı adı veya şifre hatalı");

            SignInResult result =  await _signInManager.CheckPasswordSignInAsync(user,request.Password,false);
            if (result.Succeeded)
            {
                //Yetkiler
            }

            return new();
        }
    }
}
