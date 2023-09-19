using UFOPay.Models;

public class MyViewModel
{
    public GoogleCaptchaConfig googleCaptchaConfig { get; set; }
    public UserData UserData { get; set; }
    public TransferModel TransferModel { get; set; }
    public AddBalance AddBalance { get; set; }
    public Currency Currency { get; set; }
    public Wallets Wallets { get; set; }
    public ConvertModel ConvertModel { get; set; }
    public WithdrawModel WithdrawModel { get; set; }
    public B2BBalance B2BBalance { get; set; }
    public B2Bwithdraw B2Bwithdraw { get; set; }
    public B2Bkassa B2Bkassa { get; set; }
    public BusinessBills BusinessBills { get; set; }
}
