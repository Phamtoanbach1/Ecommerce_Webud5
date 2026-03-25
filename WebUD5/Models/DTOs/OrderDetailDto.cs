namespace WebUD5.Models.DTOs
{
    public class OrderDetailDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int? ShipperId { get; set; }
        public string ShipperName { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }
        public string DeliveryStatus { get; set; }
        public DateTime? CreatedAt { get; set; }
        public List<OrderItemDto> Items { get; set; }
    }

}
