using MediatR;

namespace ECommerce.Application.Features.Commands.User.VerifyResetToken
{
    public class VerifyResetTokenCommandRequest:IRequest<VerifyResetTokenCommandResponse>
    {
        public int UserId { get; set; }
        public string ResetToken { get; set; }
    }
}