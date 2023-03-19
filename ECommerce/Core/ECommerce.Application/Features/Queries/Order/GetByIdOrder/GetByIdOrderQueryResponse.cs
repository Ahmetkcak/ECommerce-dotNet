namespace ECommerce.Application.Features.Queries.Order.GetByIdOrder
{
    public class GetByIdOrderQueryResponse
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public object BasketItems { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Description { get; set; }
        public string OrderCode { get; set; }
    }
}