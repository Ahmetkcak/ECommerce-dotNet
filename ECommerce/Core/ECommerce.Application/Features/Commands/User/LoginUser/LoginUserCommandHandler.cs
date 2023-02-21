using ECommerce.Application.Abstractions.Token;
using ECommerce.Application.DTOs;
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
        readonly ITokenHandler _tokenHandler;

        public LoginUserCommandHandler(UserManager<Domain.Entities.Identity.User> userManager, SignInManager<Domain.Entities.Identity.User> signInManager, ITokenHandler tokenHandler)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenHandler = tokenHandler;
        }

        public async Task<LoginUserCommandResponse> Handle(LoginUserCommandRequest request, CancellationToken cancellationToken)
        {
            Domain.Entities.Identity.User user = await _userManager.FindByNameAsync(request.UsernameOrMail);
            if (user == null)
                user = await _userManager.FindByEmailAsync(request.UsernameOrMail);

            if (user == null)
                throw new NotFounUserException();

            SignInResult result =  await _signInManager.CheckPasswordSignInAsync(user,request.Password,false);
            if (result.Succeeded)
            {
                Token token =_tokenHandler.CreateAccessToken(5);
                return new LoginUserSuccessCommandResponse()
                {
                    Token = token
                };
            }
            //return new LoginUserErrorCommandResponse()
            //{
            //    Message = "Kullanıcı adı veya şifre hatalı"
            //};

            throw new AuthenticationErrorException();

        }
    }
}
