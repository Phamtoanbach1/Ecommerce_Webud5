namespace WebUD5.Models.DTOs
{
    public class TransactionDto
    {
        public int Id { get; set; }
        public int PaymentId { get; set; }
        public string TransactionCode { get; set; }
        public decimal Amount { get; set; }
        public string TransactionStatus { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? ResponseMessage { get; set; }
    }

}
