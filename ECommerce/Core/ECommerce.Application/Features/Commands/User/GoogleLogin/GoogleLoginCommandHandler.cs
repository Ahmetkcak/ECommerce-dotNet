using ECommerce.Application.Abstractions.Token;
using ECommerce.Application.DTOs;
using ECommerce.Domain.Entities.Identity;
using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace ECommerce.Application.Features.Commands.User.GoogleLogin
{
    public class GoogleLoginCommandHandler : IRequestHandler<GoogleLoginCommandRequest, GoogleLoginCommandResponse>
    {
        readonly UserManager<Domain.Entities.Identity.User> _userManager;
        readonly ITokenHandler _tokenHandler;

        public GoogleLoginCommandHandler(UserManager<Domain.Entities.Identity.User> userManager, ITokenHandler tokenHandler = null)
        {
            _userManager = userManager;
            _tokenHandler = tokenHandler;
        }

        public async Task<GoogleLoginCommandResponse> Handle(GoogleLoginCommandRequest request, CancellationToken cancellationToken)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string> { "529526286725-nmq99bhc7t17rioa2lhn29i8p6loctgp.apps.googleusercontent.com" }
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken, settings);

            var info = new UserLoginInfo(request.Provider, payload.Subject, request.Provider);
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
                        Surname= fullName[1]
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

            return new()
            {
                Token = token
            };
        }
    }
}
