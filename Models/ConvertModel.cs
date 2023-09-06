namespace UFOPay.Models
{
    public class ConvertModel
    {
        public int Id { get; set; }
        public int userId { get; set; }
        public string currencyFrom { get; set; }
        public string currencyTo { get; set; }
        public int summa { get; set; }
        public string status { get; set; }
        public string comment { get; set; }
    }
}