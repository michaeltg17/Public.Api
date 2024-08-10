namespace Domain.Models
{
    public class Order : Entity
    {
        public long CustomerId { get; init; }
        public required IEnumerable<OrderLine> Lines { get; init; }
        public decimal TotalAmount => Lines.Sum(l => l.ProductNavigation.Price);
        public string PaymentMethod { get; init; } = default!;
        public string OrderStatus { get; init; } = default!;
        public string Currency { get; init; } = default!;
        public string ShippingCarrier { get; init; } = default!;
    }
}
