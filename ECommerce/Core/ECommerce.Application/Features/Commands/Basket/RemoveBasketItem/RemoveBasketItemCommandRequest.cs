using MediatR;

namespace ECommerce.Application.Features.Commands.Basket.RemoveBasketItem
{
    public class RemoveBasketItemCommandRequest : IRequest<RemoveBasketItemCommandResponse>
    {
        public int BasketItemId { get; set; }
    }
}