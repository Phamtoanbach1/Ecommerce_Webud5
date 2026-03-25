namespace WebUD5.Models.DTOs
{
    public class OrderHistoryDto
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
        public DateTime? CreatedAt { get; set; }
    }

}
