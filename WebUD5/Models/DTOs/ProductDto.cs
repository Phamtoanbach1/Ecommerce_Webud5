namespace WebUD5.Models.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int? StockQuantity { get; set; }
        public string Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int SellerId { get; set; }
        public int CategoryId { get; set; }

        // Thêm trường mở rộng (nullable)
        public string? SellerName { get; set; }
        public string? CategoryName { get; set; }
        public List<string>? ImageUrls { get; set; }
    }


}
