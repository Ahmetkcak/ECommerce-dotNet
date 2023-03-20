using ECommerce.Application.Abstractions.Services;
using ECommerce.Application.Abstractions.Token;
using ECommerce.Application.DTOs;
using ECommerce.Application.Exceptions;
using ECommerce.Application.Features.Commands.User.LoginUser;
using ECommerce.Application.Helpers;
using ECommerce.Domain.Entities.Identity;
using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
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
        readonly IUserService _userService;
        readonly IMailService _mailService;

        public AuthService(UserManager<User> userManager, ITokenHandler tokenHandler, IConfiguration configuration, SignInManager<User> signInManager, IUserService userService, IMailService mailService)
        {
            _userManager = userManager;
            _tokenHandler = tokenHandler;
            _configuration = configuration;
            _signInManager = signInManager;
            _userService = userService;
            _mailService = mailService;
        }

        public async Task<Token> GoogleLoginAsycn(string idToken,int accessTokenLifeTime)
        {

            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string> { _configuration["Google:Client_ID"] }
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

            Token token = _tokenHandler.CreateAccessToken(accessTokenLifeTime,user);
            //await _userService.UpdateRefreshToken(user, token.RefreshToken, token.Expiration, 10);
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
                Token token = _tokenHandler.CreateAccessToken(accessTokenLifeTime,user);
                //await _userService.UpdateRefreshToken(user, token.RefreshToken, token.Expiration, 10);
                return token;
            }
            else
                throw new AuthenticationErrorException();
        }

        public async Task PasswordResetAsync(string email)
        {
            User user = await _userManager.FindByEmailAsync(email);
            if(user != null)
            {
                string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                resetToken = resetToken.UrlEncode();
                await _mailService.SendPasswordResetMailAsync(email,user.Id, resetToken);
            }
        }

        public async Task<Token> RefreshTokenLoginAsycn(string refreshToken)
        {
           User? user = _userManager.Users.FirstOrDefault(u => u.RefreshToken== refreshToken);
            if(user != null && user?.RefreshTokenEndDate > DateTime.UtcNow)
            {
                Token token = _tokenHandler.CreateAccessToken(15,user);
                await _userService.UpdateRefreshTokenAsync(user,token.RefreshToken, token.Expiration, 15);
                return token;
            }
            else
                throw new NotFounUserException();

        }

        public async Task<bool> VerifyResetTokenAsync(string resetToken, int userId)
        {
            User user = await _userManager.FindByIdAsync(userId.ToString());
            if(user != null)
            {
                resetToken = resetToken.UrlDecode();
                return await _userManager.VerifyUserTokenAsync(user, _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", resetToken);  
            }
            return false;
        }
    }
}
