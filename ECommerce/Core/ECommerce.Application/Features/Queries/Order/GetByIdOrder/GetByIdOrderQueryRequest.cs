using MediatR;

namespace ECommerce.Application.Features.Queries.Order.GetByIdOrder
{
    public class GetByIdOrderQueryRequest : IRequest<GetByIdOrderQueryResponse>
    {
        public int Id { get; set; }
    }
}