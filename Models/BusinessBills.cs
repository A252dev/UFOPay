namespace UFOPay.Models
{
    public class BusinessBills
    {
        public int Id { get; set; }
        public int kassaId { get; set; }
        public int billingId { get; set; }
        public int paidUserId { get; set; }
        public string status { get; set; }
        public int summa { get; set; }
        public string currency { get; set; }
        public string apiKey { get; set; }
    }
}