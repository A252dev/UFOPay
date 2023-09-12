namespace UFOPay.Models
{
    public class B2Bwithdraw
    {
        public int Id { get; set; }
        public int userId { get; set; }
        public string bank { get; set; }
        public string currency { get; set; }
        public double summa { get; set; }
    }
}