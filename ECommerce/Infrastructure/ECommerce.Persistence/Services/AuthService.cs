using ECommerce.Application.Abstractions.Services;
using ECommerce.Application.Abstractions.Token;
using ECommerce.Application.DTOs;
using ECommerce.Application.Exceptions;
using ECommerce.Application.Features.Commands.User.LoginUser;
using ECommerce.Domain.Entities.Identity;
using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Persistence.Services
{
    public class AuthService : IAuthService
    {
        readonly UserManager<User> _userManager;
        readonly ITokenHandler _tokenHandler;
        readonly IConfiguration _configuration;
        readonly SignInManager<User> _signInManager;

        public AuthService(UserManager<User> userManager, ITokenHandler tokenHandler, IConfiguration configuration, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _tokenHandler = tokenHandler;
            _configuration = configuration;
            _signInManager = signInManager;
        }

        public async Task<Token> GoogleLoginAsycn(string idToken,int accessTokenLifeTime)
        {

            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string> { _configuration["ExternalLoginSettings:Google:Client_ID"] }
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);

            var info = new UserLoginInfo("GOOGLE", payload.Subject,"GOOGLE");
            Domain.Entities.Identity.User user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

            bool result = user != null;
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(payload.Email);
                string[] fullName = payload.Name.Split(" ");
                if (user == null)
                {
                    user = new()
                    {
                        Email = payload.Email,
                        UserName = payload.Email,
                        Name = fullName[0],
                        Surname = fullName[1]
                    };
                    var identityResult = await _userManager.CreateAsync(user);
                    result = identityResult.Succeeded;
                }
            }

            if (result)
                await _userManager.AddLoginAsync(user, info);
            else
                throw new Exception("Invalid external authentication.");

            Token token = _tokenHandler.CreateAccessToken(5);

            return token;
        }

        public async Task<Token> LoginAsycn(string userameOrEmail, string password, int accessTokenLifeTime)
        {
            User user = await _userManager.FindByNameAsync(userameOrEmail);
            if (user == null)
                user = await _userManager.FindByEmailAsync(userameOrEmail);

            if (user == null)
                throw new NotFounUserException();

            SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            if (result.Succeeded)
            {
                Token token = _tokenHandler.CreateAccessToken(accessTokenLifeTime);
                return token;
            }
            throw new AuthenticationErrorException();
        }
    }
}
