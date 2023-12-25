namespace Domain.Models
{
    public class Order : Entity
    {
        public long CustomerId { get; set; } = default!;
        public IEnumerable<OrderLine> Lines { get; set; } = default!;
        public decimal TotalAmount => Lines.Sum(l => l.ProductNavigation.Price);
        public string PaymentMethod { get; set; } = default!;
        public string OrderStatus { get; set; } = default!;
        public string Currency { get; set; } = default!;
        public string ShippingCarrier { get; set; } = default!;
    }
}
