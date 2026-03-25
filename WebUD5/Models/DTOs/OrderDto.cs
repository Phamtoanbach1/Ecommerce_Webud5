namespace WebUD5.Models.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; } 
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }
        public string DeliveryStatus { get; set; }
        public DateTime? CreatedAt { get; set; }
        public List<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
    }

}
