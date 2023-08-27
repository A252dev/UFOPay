using System.Net.Sockets;

namespace UFOPay.Models
{
    public class AddBalance
    {
        public long summa { get; set; }
        public string currency { get; set; }
    }
}