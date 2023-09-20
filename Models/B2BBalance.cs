namespace UFOPay.Models
{
    public class B2BBalance
    {
        public int Id { get; set; }
        public int userId { get; set; }
        public int kassaId { get; set; }
        public bool b2bAccess { get; set; } = false;
        public double b2b_balance_usd { get; set; }
        public double b2b_balance_eur { get; set; }
        public double b2b_balance_pln { get; set; }
        public double b2b_balance_rub { get; set; }
    }
}