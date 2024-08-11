namespace Domain.Models
{
    public class Order : Entity
    {
        public long CustomerId { get; init; }
        public required IEnumerable<OrderLine> Lines { get; init; }
        public decimal TotalAmount => Lines.Sum(l => l.ProductNavigation.Price);
        public required string PaymentMethod { get; init; }
        public required string OrderStatus { get; init; }
        public required string Currency { get; init; }
        public required string ShippingCarrier { get; init; }
    }
}
