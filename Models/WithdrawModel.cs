namespace UFOPay.Models
{
    public class WithdrawModel
    {
        public int Id { get; set; }
        public int userId { get; set; }
        public string currency { get; set; }
        public string bank { get; set; }
        public int summa { get; set; }
        public string comment { get; set; }
        public DateTime withdrawData { get; set; }
    }
}