using System;

public class UserData
{
    public int Id { get; set; }
    public int userId { get; set; }
    public string firstName { get; set; }
    public string secondName { get; set; }
    public string email { get; set; }
    public string password { get; set; }
    public string repeatPassword { get; set; }
    public DateTime registrationData { get; set; }
    public string birthday { get; set; } = "1900/08/28";
    public string passport { get; set; } = "PL32068";
    public long balance { get; set; } = 8000;
    public bool KeepLoggedIn { get; set; }
    public bool AgreeWithDocs { get; set; }
}
