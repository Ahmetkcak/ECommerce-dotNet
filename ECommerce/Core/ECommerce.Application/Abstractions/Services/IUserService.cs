using ECommerce.Application.DTOs.User;
using ECommerce.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Abstractions.Services
{
    public interface IUserService
    {
        Task<CreateUserResponse> CreateAsync(CreateUser model);
        Task UpdateRefreshTokenAsync(User user, string refreshToken, DateTime accessTokenDate, int addOnAccessTokenDate);
        Task UpdatePasswordAsync(int userId, string resetToken, string newPassword);
    }
}
