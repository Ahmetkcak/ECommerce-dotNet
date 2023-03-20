using MediatR;

namespace ECommerce.Application.Features.Commands.User.UpdatePassword
{
    public class UpdatePasswordCommandRequest:IRequest<UpdatePasswordCommandResponse>
    {
        public int UserId { get; set; }
        public string ResetToken { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
    }
}