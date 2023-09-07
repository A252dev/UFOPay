namespace UFOPay.Models
{
    public class UserData
    {
        public int Id { get; set; }
        public int userId { get; set; }
        public bool admin { get; set; }
        public string firstName { get; set; }
        public string secondName { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string repeatPassword { get; set; }
        public DateTime registrationData { get; set; }
        public string birthday { get; set; }
        public string passport { get; set; } = "NONE";
        public double balance_usd { get; set; } = 0;
        public double balance_eur { get; set; } = 0;
        public double balance_pln { get; set; } = 0;
        public double balance_rub { get; set; } = 0;
        public bool KeepLoggedIn { get; set; }
        public bool AgreeWithDocs { get; set; }
    }
}
