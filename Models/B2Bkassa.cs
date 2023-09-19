namespace UFOPay.Models
{
    public class B2Bkassa
    {
        public int Id { get; set; }
        public int userId { get; set; }
        public int kassaId { get; set; }
        public string website { get; set; }
        public string description { get; set; }
        public string contactEmail { get; set; }
        public string status { get; set; }
        public string comment { get; set; }
        public string apiKey { get; set; }
    }
}