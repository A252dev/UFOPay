using System.Net.Sockets;

namespace UFOPay.Models
{
    public class AddBalance
    {
        public int Id { get; set; }
        public int userId { get; set; }
        public long summa { get; set; }
        public string currency { get; set; }
        public string status { get; set; }
        public string comment { get; set; }
    }
}