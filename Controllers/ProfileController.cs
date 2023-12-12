using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using UFOPay.Models;

[Authorize]
public class ProfileController : Controller
{
    private readonly UFODbContext _dbContext;

    public ProfileController(UFODbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public string GenerateComment()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        Random random = new Random();
        string comment = new string(Enumerable.Repeat(chars, 10) // 10 this is number of chars in comment
        .Select(s => s[random.Next(s.Length)]).ToArray());
        while (_dbContext.AddBalanceRequest.Any(x => x.comment == comment))
            continue;
        return comment;
    }

    [Route("/profile")]
    public IActionResult profile()
    {
        return View();
    }

    [Route("/admin")]
    public IActionResult admin()
    {
        return View();
    }

    [HttpPost]
    public IActionResult dataChange(UserData userData)
    {
        var user = _dbContext.UserData.FirstOrDefault(x => x.email == User.Identity.Name);
        if (userData.firstName != null)
        {
            user.firstName = userData.firstName;
            _dbContext.SaveChanges();
        }
        if (userData.secondName != null)
        {
            user.secondName = userData.secondName;
            _dbContext.SaveChanges();
        }
        if (userData.birthday != null)
        {
            user.birthday = userData.birthday;
            _dbContext.SaveChanges();
        }
        if (userData.passport != null)
        {
            user.passport = userData.passport;
            _dbContext.SaveChanges();
        }
        if (userData.password != null)
        {
            user.password = userData.password;
            _dbContext.SaveChanges();
        }
        _dbContext.SaveChanges();
        TempData["SettingsSuccessMessage"] = "Settings updated successfully";
        return RedirectToAction("profile", "Profile");
    }

    [HttpGet]
    public IActionResult transfer()
    {
        return View();
    }

    [HttpPost]
    public IActionResult transfer(TransferModel transferModel)
    {
        var user = _dbContext.UserData.FirstOrDefault(x => x.email == User.Identity.Name);
        var targetForTheTransferUser = _dbContext.UserData.FirstOrDefault(x => x.email == transferModel.reciever);
        if (targetForTheTransferUser != null)
        {
            TransferModel transfer = new TransferModel
            {
                sender = user.email,
                reciever = transferModel.reciever,
                summa = transferModel.summa,
                currency = transferModel.currency,
                comment = transferModel.comment,
                transferData = DateTime.Now,
            };

            // TRANSFER WITH USD
            if (transferModel.currency == "USD")
            {
                if (user.balance_usd <= transferModel.summa || transferModel.summa < 1)
                {
                    TempData["TransferErrorMessage"] = "Not enough money";
                    return RedirectToAction("profile", "Profile");
                }
                else
                {
                    user.balance_usd -= transferModel.summa;
                    Math.Round(targetForTheTransferUser.balance_usd += transferModel.summa, 2);
                    _dbContext.Transactions.Update(transfer);
                    _dbContext.SaveChanges();
                    TempData["TransferSuccessMessage"] = "Transfer completed";
                    return RedirectToAction("profile", "Profile");
                }
            }

            // TRANSFER WITH EUR
            if (transferModel.currency == "EUR")
            {
                if (user.balance_eur <= transferModel.summa || transferModel.summa < 1)
                {
                    TempData["TransferErrorMessage"] = "Not enough money";
                    return RedirectToAction("profile", "Profile");
                }
                else
                {
                    user.balance_eur -= transferModel.summa;
                    Math.Round(targetForTheTransferUser.balance_eur += transferModel.summa, 2);
                    _dbContext.Transactions.Update(transfer);
                    _dbContext.SaveChanges();
                    TempData["TransferSuccessMessage"] = "Transfer completed";
                    return RedirectToAction("profile", "Profile");
                }
            }

            // TRANSFER WITH PLN
            if (transferModel.currency == "PLN")
            {
                if (user.balance_pln <= transferModel.summa || transferModel.summa < 1)
                {
                    TempData["TransferErrorMessage"] = "Not enough money";
                    return RedirectToAction("profile", "Profile");
                }
                else
                {
                    user.balance_pln -= transferModel.summa;
                    Math.Round(targetForTheTransferUser.balance_pln += transferModel.summa, 2);
                    _dbContext.Transactions.Update(transfer);
                    _dbContext.SaveChanges();
                    TempData["TransferSuccessMessage"] = "Transfer completed";
                    return RedirectToAction("profile", "Profile");
                }
            }

            // TRANSFER WITH RUB
            if (transferModel.currency == "RUB")
            {
                if (user.balance_rub <= transferModel.summa || transferModel.summa < 1)
                {
                    TempData["TransferErrorMessage"] = "Not enough money";
                    return RedirectToAction("profile", "Profile");
                }
                else
                {
                    user.balance_rub -= transferModel.summa;
                    Math.Round(targetForTheTransferUser.balance_rub += transferModel.summa, 2);
                    _dbContext.Transactions.Update(transfer);
                    _dbContext.SaveChanges();
                    TempData["TransferSuccessMessage"] = "Transfer completed";
                    return RedirectToAction("profile", "Profile");
                }
            }
        }
        else
        {
            TempData["TransferErrorMessage"] = "User not found";
            return RedirectToAction("profile", "Profile");
        }
        TempData["TransferErrorMessage"] = "Data is invalid";
        return RedirectToAction("profile", "Profile");
    }

    [HttpPost]
    public IActionResult addBalance(AddBalance addBalance)
    {
        if (addBalance.summa >= 1 && addBalance.currency != null)
        {
            var user = _dbContext.UserData.FirstOrDefault(x => x.email == User.Identity.Name);


            // DEBUG
            if (addBalance.currency == "USD")
            {
                Math.Round(user.balance_usd += addBalance.summa, 2);
                _dbContext.SaveChanges();
            }
            if (addBalance.currency == "EUR")
            {
                Math.Round(user.balance_eur += addBalance.summa, 2);
                _dbContext.SaveChanges();
            }
            if (addBalance.currency == "PLN")
            {
                Math.Round(user.balance_pln += addBalance.summa, 2);
                _dbContext.SaveChanges();
            }
            if (addBalance.currency == "RUB")
            {
                Math.Round(user.balance_rub += addBalance.summa, 2);
                _dbContext.SaveChanges();
            }
            _dbContext.SaveChanges();
            TempData["BillCreated"] = "Balance is successfully added";
            return RedirectToAction("profile", "Profile");

            // FOR RELEASE

            // AddBalance addNewStatus = new AddBalance()
            // {
            //     userId = user.userId,
            //     currency = addBalance.currency,
            //     summa = addBalance.summa,
            //     status = "PENDING",
            //     comment = GenerateComment(),
            // };
            // _dbContext.AddBalanceRequest.Update(addNewStatus);
            // _dbContext.SaveChanges();
            // TempData["BillCreated"] = "Bill has been created";
            // return RedirectToAction("billing", "Page");
        }
        else
        {
            TempData["BillErrorMessage"] = "Data is incorrect";
            return RedirectToAction("profile", "Profile");
        }
    }

    [HttpPost]
    public IActionResult billingPay(BusinessBills b2bPay)
    {
        if (b2bPay.billingId != null)
        {
            var billInfo = _dbContext.BusinessBills.FirstOrDefault(x => x.billingId == b2bPay.billingId);
            if (billInfo != null)
            {
                var user = _dbContext.UserData.FirstOrDefault(x => x.email == User.Identity.Name);

                if (billInfo.currency == "USD")
                {
                    if (user.balance_usd >= billInfo.summa)
                    {
                        Math.Round(user.balance_usd -= billInfo.summa);
                        _dbContext.B2BBalance.FirstOrDefault(x => x.kassaId == billInfo.kassaId).b2b_balance_usd += billInfo.summa;
                        billInfo.status = "PAID";
                        billInfo.paidUserId = user.userId;
                        billInfo.paymentData = DateTime.Now;
                        _dbContext.SaveChanges();
                        TempData["BillSuccess"] = "Payment completed";
                        return RedirectToAction("profile", "Profile");
                    }
                    else
                    {
                        TempData["BillError"] = "Not enouth money";
                        return RedirectToAction("billing", "Page");
                    }
                }
            }
            else
            {
                return RedirectToAction("billing", "Page");
            }
            return View();
        }
        else
        {
            return RedirectToAction("index", "Page");
        }
    }

    [HttpPost]
    public IActionResult convertCurrency(ConvertModel convertModel)
    {
        if (convertModel.currencyFrom != null && convertModel.currencyTo != null && convertModel.summa != null)
        {
            var targetUser = _dbContext.UserData.FirstOrDefault(x => x.userId == convertModel.userId);
            if (convertModel.currencyFrom == "USD")
            {
                if (convertModel.summa >= 1 && convertModel.summa <= targetUser.balance_usd)
                {
                    ConvertModel model = new ConvertModel()
                    {
                        currencyFrom = convertModel.currencyFrom,
                        currencyTo = convertModel.currencyTo,
                        summa = convertModel.summa,
                        userId = targetUser.userId,
                        status = "PENDING",
                        comment = GenerateComment(),
                    };
                    _dbContext.Convertation.Update(model);
                    _dbContext.SaveChanges();
                    return RedirectToAction("convertation", "Profile");
                }
                else
                {
                    TempData["TransferErrorMessage"] = "Data is incorrect";
                    return RedirectToAction("profile", "Profile");
                }
            }
            if (convertModel.currencyFrom == "EUR")
            {
                if (convertModel.summa >= 1 && convertModel.summa <= targetUser.balance_eur)
                {
                    ConvertModel model = new ConvertModel()
                    {
                        currencyFrom = convertModel.currencyFrom,
                        currencyTo = convertModel.currencyTo,
                        summa = convertModel.summa,
                        userId = targetUser.userId,
                        status = "PENDING",
                        comment = GenerateComment(),
                    };
                    _dbContext.Convertation.Update(model);
                    _dbContext.SaveChanges();
                    return RedirectToAction("convertation", "Profile");
                }
                else
                {
                    TempData["TransferErrorMessage"] = "Data is incorrect";
                    return RedirectToAction("profile", "Profile");
                }
            }
            if (convertModel.currencyFrom == "PLN")
            {
                if (convertModel.summa >= 1 && convertModel.summa <= targetUser.balance_pln)
                {
                    ConvertModel model = new ConvertModel()
                    {
                        currencyFrom = convertModel.currencyFrom,
                        currencyTo = convertModel.currencyTo,
                        summa = convertModel.summa,
                        userId = targetUser.userId,
                        status = "PENDING",
                        comment = GenerateComment(),
                    };
                    _dbContext.Convertation.Update(model);
                    _dbContext.SaveChanges();
                    return RedirectToAction("convertation", "Profile");
                }
                else
                {
                    TempData["TransferErrorMessage"] = "Data is incorrect";
                    return RedirectToAction("profile", "Profile");
                }
            }
            if (convertModel.currencyFrom == "RUB")
            {
                if (convertModel.summa >= 1 && convertModel.summa <= targetUser.balance_rub)
                {
                    ConvertModel model = new ConvertModel()
                    {
                        currencyFrom = convertModel.currencyFrom,
                        currencyTo = convertModel.currencyTo,
                        summa = convertModel.summa,
                        userId = targetUser.userId,
                        status = "PENDING",
                        comment = GenerateComment(),
                    };
                    _dbContext.Convertation.Update(model);
                    _dbContext.SaveChanges();
                    return RedirectToAction("convertation", "Profile");
                }
                else
                {
                    TempData["TransferErrorMessage"] = "Data is incorrect";
                    return RedirectToAction("profile", "Profile");
                }
            }

        }
        return RedirectToAction("profile", "Profile");
    }

    [HttpGet]
    [Route("/convertation")]
    public IActionResult convertation()
    {
        return View();
    }

    [HttpPost]
    public IActionResult submitConvert(ConvertModel convertModel)
    {
        var user = _dbContext.UserData.FirstOrDefault(x => x.userId == convertModel.userId);
        var base_code = convertModel.currencyFrom;
        System.Net.WebClient client = new System.Net.WebClient();
        var content =
        client.DownloadString("https://v6.exchangerate-api.com/v6/ce3ba77a89e6e5e13dcbaf76/latest/" + base_code);

        string[] lines = content.Split(",");

        if (base_code == "USD")
        {
            string eur = "";
            string pln = "";
            string rub = "";
            foreach (var line in lines)
            {
                if (line.Contains("EUR"))
                    eur = line;
                if (line.Contains("PLN"))
                    pln = line;
                if (line.Contains("RUB"))
                    rub = line;
            }
            double euro = Convert.ToDouble(eur.Substring(eur.LastIndexOf(":") + 1));
            double zloty = Convert.ToDouble(pln.Substring(pln.LastIndexOf(":") + 1));
            double ruble = Convert.ToDouble(rub.Substring(rub.LastIndexOf(":") + 1));
            if (convertModel.currencyTo == "EUR")
            {
                Math.Round(user.balance_usd -= convertModel.summa, 2);
                Math.Round(user.balance_eur += convertModel.summa * euro, 2);
                _dbContext.SaveChanges();
                TempData["TransferSuccessMessage"] = "Currency is converted successfully";
                return RedirectToAction("profile", "Profile");
            }
            if (convertModel.currencyTo == "PLN")
            {
                Math.Round(user.balance_usd -= convertModel.summa, 2);
                Math.Round(user.balance_pln += convertModel.summa * zloty, 2);
                _dbContext.SaveChanges();
                TempData["TransferSuccessMessage"] = "Currency is converted successfully";
                return RedirectToAction("profile", "Profile");
            }
            if (convertModel.currencyTo == "RUB")
            {
                Math.Round(user.balance_usd -= convertModel.summa, 2);
                Math.Round(user.balance_rub += convertModel.summa * ruble, 2);
                _dbContext.SaveChanges();
                TempData["TransferSuccessMessage"] = "Currency is converted successfully";
                return RedirectToAction("profile", "Profile");
            }
        }
        if (base_code == "EUR")
        {
            string usd = "";
            string pln = "";
            string rub = "";
            foreach (var line in lines)
            {
                if (line.Contains("USD"))
                    usd = line;
                if (line.Contains("PLN"))
                    pln = line;
                if (line.Contains("RUB"))
                    rub = line;
            }
            double dollar = Convert.ToDouble(usd.Substring(usd.LastIndexOf(":") + 1));
            double zloty = Convert.ToDouble(pln.Substring(pln.LastIndexOf(":") + 1));
            double ruble = Convert.ToDouble(rub.Substring(rub.LastIndexOf(":") + 1));
            if (convertModel.currencyTo == "USD")
            {
                Math.Round(user.balance_eur -= convertModel.summa, 2);
                Math.Round(user.balance_usd += convertModel.summa * dollar, 2);
                _dbContext.SaveChanges();
                TempData["TransferSuccessMessage"] = "Currency is converted successfully";
                return RedirectToAction("profile", "Profile");
            }
            if (convertModel.currencyTo == "PLN")
            {
                Math.Round(user.balance_eur -= convertModel.summa, 2);
                Math.Round(user.balance_pln += convertModel.summa * zloty, 2);
                _dbContext.SaveChanges();
                TempData["TransferSuccessMessage"] = "Currency is converted successfully";
                return RedirectToAction("profile", "Profile");
            }
            if (convertModel.currencyTo == "RUB")
            {
                Math.Round(user.balance_eur -= convertModel.summa, 2);
                Math.Round(user.balance_rub += convertModel.summa * ruble, 2);
                _dbContext.SaveChanges();
                TempData["TransferSuccessMessage"] = "Currency is converted successfully";
                return RedirectToAction("profile", "Profile");
            }
        }
        if (base_code == "PLN")
        {
            string eur = "";
            string usd = "";
            string rub = "";
            foreach (var line in lines)
            {
                if (line.Contains("EUR"))
                    eur = line;
                if (line.Contains("USD"))
                    usd = line;
                if (line.Contains("RUB"))
                    rub = line;
            }
            double euro = Convert.ToDouble(eur.Substring(eur.LastIndexOf(":") + 1));
            double dollar = Convert.ToDouble(usd.Substring(usd.LastIndexOf(":") + 1));
            double ruble = Convert.ToDouble(rub.Substring(rub.LastIndexOf(":") + 1));
            if (convertModel.currencyTo == "EUR")
            {
                Math.Round(user.balance_pln -= convertModel.summa, 2);
                Math.Round(user.balance_eur += convertModel.summa * euro, 2);
                _dbContext.SaveChanges();
                TempData["TransferSuccessMessage"] = "Currency is converted successfully";
                return RedirectToAction("profile", "Profile");
            }
            if (convertModel.currencyTo == "USD")
            {
                Math.Round(user.balance_pln -= convertModel.summa, 2);
                Math.Round(user.balance_usd += convertModel.summa * dollar, 2);
                _dbContext.SaveChanges();
                TempData["TransferSuccessMessage"] = "Currency is converted successfully";
                return RedirectToAction("profile", "Profile");
            }
            if (convertModel.currencyTo == "RUB")
            {
                Math.Round(user.balance_pln -= convertModel.summa, 2);
                Math.Round(user.balance_pln += convertModel.summa * ruble, 2);
                _dbContext.SaveChanges();
                TempData["TransferSuccessMessage"] = "Currency is converted successfully";
                return RedirectToAction("profile", "Profile");
            }
        }
        if (base_code == "RUB")
        {
            string eur = "";
            string pln = "";
            string usd = "";
            foreach (var line in lines)
            {
                if (line.Contains("EUR"))
                    eur = line;
                if (line.Contains("PLN"))
                    pln = line;
                if (line.Contains("USD"))
                    usd = line;
            }
            double euro = Convert.ToDouble(eur.Substring(eur.LastIndexOf(":") + 1));
            double zloty = Convert.ToDouble(pln.Substring(pln.LastIndexOf(":") + 1));
            double dollar = Convert.ToDouble(usd.Substring(usd.LastIndexOf(":") + 1));
            if (convertModel.currencyTo == "EUR")
            {
                Math.Round(user.balance_rub -= convertModel.summa, 2);
                Math.Round(user.balance_eur += convertModel.summa * euro, 2);
                _dbContext.SaveChanges();
                TempData["TransferSuccessMessage"] = "Currency is converted successfully";
                return RedirectToAction("profile", "Profile");
            }
            if (convertModel.currencyTo == "PLN")
            {
                Math.Round(user.balance_rub -= convertModel.summa, 2);
                Math.Round(user.balance_pln += convertModel.summa * zloty, 2);
                _dbContext.SaveChanges();
                TempData["TransferSuccessMessage"] = "Currency is converted successfully";
                return RedirectToAction("profile", "Profile");
            }
            if (convertModel.currencyTo == "USD")
            {
                Math.Round(user.balance_rub -= convertModel.summa, 2);
                Math.Round(user.balance_usd += convertModel.summa * dollar, 2);
                _dbContext.SaveChanges();
                TempData["TransferSuccessMessage"] = "Currency is converted successfully";
                return RedirectToAction("profile", "Profile");
            }
        }
        // none
        return RedirectToAction("profile", "Profile");
    }

    [HttpPost]
    public IActionResult billConfirm(AddBalance addBalance)
    {
        if (ModelState.IsValid)
        {
            TempData["BillSuccess"] = "Invoice created";
            return RedirectToAction("profile", "Profile");
        }
        else
        {
            TempData["BillUnsuccess"] = "Bill not created";
            return RedirectToAction("profile", "Profile");
        }
    }

    [HttpPost]
    public IActionResult withdraw(WithdrawModel withdrawModel)
    {
        var user = _dbContext.UserData.FirstOrDefault(x => x.email == User.Identity.Name);
        if (withdrawModel.currency != null && withdrawModel.bank != null && withdrawModel.summa != null)
        {
            if (withdrawModel.currency == "USD")
            {
                if (user.balance_usd >= withdrawModel.summa)
                {
                    WithdrawModel withdraw = new WithdrawModel()
                    {
                        userId = user.userId,
                        currency = withdrawModel.currency,
                        bank = withdrawModel.bank,
                        summa = withdrawModel.summa,
                        comment = GenerateComment(),
                        withdrawData = DateTime.Now,
                    };
                    Math.Round(user.balance_usd -= withdrawModel.summa, 2);
                    _dbContext.Withdraw.Update(withdraw);
                    _dbContext.SaveChanges();
                    TempData["BillSuccess"] = "Invoice for withdraw created";
                    return RedirectToAction("profile", "Profile");
                }
                else
                {
                    TempData["BillErrorMessage"] = "Not enough money";
                    return RedirectToAction("profile", "Profile");
                }
            }
            if (withdrawModel.currency == "EUR")
            {
                if (user.balance_eur >= withdrawModel.summa)
                {
                    WithdrawModel withdraw = new WithdrawModel()
                    {
                        userId = user.userId,
                        currency = withdrawModel.currency,
                        bank = withdrawModel.bank,
                        summa = withdrawModel.summa,
                        comment = GenerateComment(),
                    };
                    Math.Round(user.balance_eur -= withdrawModel.summa, 2);
                    _dbContext.Withdraw.Update(withdraw);
                    _dbContext.SaveChanges();
                    TempData["BillSuccess"] = "Invoice for withdraw created";
                    return RedirectToAction("profile", "Profile");
                }
                else
                {
                    TempData["BillErrorMessage"] = "Not enough money";
                    return RedirectToAction("profile", "Profile");
                }
            }
            if (withdrawModel.currency == "PLN")
            {
                if (user.balance_pln >= withdrawModel.summa)
                {
                    WithdrawModel withdraw = new WithdrawModel()
                    {
                        userId = user.userId,
                        currency = withdrawModel.currency,
                        bank = withdrawModel.bank,
                        summa = withdrawModel.summa,
                        comment = GenerateComment(),
                    };
                    Math.Round(user.balance_pln -= withdrawModel.summa, 2);
                    _dbContext.Withdraw.Update(withdraw);
                    _dbContext.SaveChanges();
                    TempData["BillSuccess"] = "Invoice for withdraw created";
                    return RedirectToAction("profile", "Profile");
                }
                else
                {
                    TempData["BillErrorMessage"] = "Not enough money";
                    return RedirectToAction("profile", "Profile");
                }
            }
            if (withdrawModel.currency == "RUB")
            {
                if (user.balance_rub >= withdrawModel.summa)
                {
                    WithdrawModel withdraw = new WithdrawModel()
                    {
                        userId = user.userId,
                        currency = withdrawModel.currency,
                        bank = withdrawModel.bank,
                        summa = withdrawModel.summa,
                        comment = GenerateComment(),
                    };
                    Math.Round(user.balance_rub -= withdrawModel.summa, 2);
                    _dbContext.Withdraw.Update(withdraw);
                    _dbContext.SaveChanges();
                    TempData["BillSuccess"] = "Invoice for withdraw created";
                    return RedirectToAction("profile", "Profile");
                }
                else
                {
                    TempData["BillErrorMessage"] = "Not enough money";
                    return RedirectToAction("profile", "Profile");
                }
            }
            else
            {
                TempData["BillErrorMessage"] = "Currency not selected";
                return RedirectToAction("profile", "Profile");
            }
        }
        else
        {
            TempData["BillErrorMessage"] = "Data incorrect";
            return RedirectToAction("profile", "Profile");
        }
    }

    [HttpPost]
    public IActionResult withdrawConfirm(WithdrawModel withdrawModel)
    {
        var targetUser = _dbContext.UserData.FirstOrDefault(x => x.userId == withdrawModel.userId);
        if (ModelState.IsValid)
        {
            if (withdrawModel.currency == "USD")
            {
                // MAKE IN UPDATE TRANSFER TO THE BANK
                _dbContext.Withdraw.FirstOrDefault(x => x.comment == withdrawModel.comment).comment = "SUCCESS";
                _dbContext.SaveChanges();
                TempData["CheckSuccess"] = "Withdraw are successed";
                return RedirectToAction("admin", "Profile");
            }
            if (withdrawModel.currency == "EUR")
            {
                _dbContext.Withdraw.FirstOrDefault(x => x.comment == withdrawModel.comment).comment = "SUCCESS";
                _dbContext.SaveChanges();
                TempData["CheckSuccess"] = "Withdraw are successed";
                return RedirectToAction("admin", "Profile");
            }
            if (withdrawModel.currency == "PLN")
            {
                _dbContext.Withdraw.FirstOrDefault(x => x.comment == withdrawModel.comment).comment = "SUCCESS";
                _dbContext.SaveChanges();
                TempData["CheckSuccess"] = "Withdraw are successed";
                return RedirectToAction("admin", "Profile");
            }
            if (withdrawModel.currency == "RUB")
            {
                _dbContext.Withdraw.FirstOrDefault(x => x.comment == withdrawModel.comment).comment = "SUCCESS";
                _dbContext.SaveChanges();
                TempData["CheckSuccess"] = "Withdraw are successed";
                return RedirectToAction("admin", "Profile");
            }
            else
            {
                TempData["CheckFailed"] = "Currency not found";
                return RedirectToAction("admin", "Profile");
            }
        }
        else
        {
            TempData["CheckFailed"] = "Data is incorrect";
            return RedirectToAction("admin", "Profile");
        }
    }

    [HttpPost]
    public IActionResult withdrawCancel(WithdrawModel withdrawModel)
    {
        var targetUser = _dbContext.UserData.FirstOrDefault(x => x.userId == withdrawModel.userId);
        if (ModelState.IsValid)
        {
            if (withdrawModel.currency == "USD")
            {
                _dbContext.Withdraw.FirstOrDefault(x => x.comment == withdrawModel.comment).comment = "CANCEL";
                Math.Round(targetUser.balance_usd += withdrawModel.summa, 2);
                _dbContext.SaveChanges();
                TempData["CheckSuccess"] = "Cancel are successed";
                return RedirectToAction("admin", "Profile");
            }
            if (withdrawModel.currency == "EUR")
            {
                _dbContext.Withdraw.FirstOrDefault(x => x.comment == withdrawModel.comment).comment = "CANCEL";
                Math.Round(targetUser.balance_eur += withdrawModel.summa, 2);
                _dbContext.SaveChanges();
                TempData["CheckSuccess"] = "Cancel are successed";
                return RedirectToAction("admin", "Profile");
            }
            if (withdrawModel.currency == "PLN")
            {
                _dbContext.Withdraw.FirstOrDefault(x => x.comment == withdrawModel.comment).comment = "CANCEL";
                Math.Round(targetUser.balance_pln += withdrawModel.summa, 2);
                _dbContext.SaveChanges();
                TempData["CheckSuccess"] = "Cancel are successed";
                return RedirectToAction("admin", "Profile");
            }
            if (withdrawModel.currency == "RUB")
            {
                _dbContext.Withdraw.FirstOrDefault(x => x.comment == withdrawModel.comment).comment = "CANCEL";
                Math.Round(targetUser.balance_rub += withdrawModel.summa, 2);
                _dbContext.SaveChanges();
                TempData["CheckSuccess"] = "Cancel are successed";
                return RedirectToAction("admin", "Profile");
            }
            else
            {
                TempData["CheckFailed"] = "Currency not found";
                return RedirectToAction("admin", "Profile");
            }
        }
        else
        {
            TempData["CheckFailed"] = "Cancel is failed";
            return RedirectToAction("admin", "Profile");
        }
    }

    [HttpPost]
    public IActionResult b2bWithdrawConfirm(WithdrawModel withdrawModel)
    {
        var targetUser = _dbContext.UserData.FirstOrDefault(x => x.userId == withdrawModel.userId);
        if (ModelState.IsValid)
        {
            if (withdrawModel.currency == "USD")
            {
                // MAKE IN UPDATE TRANSFER TO THE BANK
                _dbContext.Withdraw.FirstOrDefault(x => x.comment == withdrawModel.comment).comment = "B2BSUCCESS";
                _dbContext.SaveChanges();
                TempData["CheckSuccess"] = "Withdraw are successed";
                return RedirectToAction("admin", "Profile");
            }
            if (withdrawModel.currency == "EUR")
            {
                _dbContext.Withdraw.FirstOrDefault(x => x.comment == withdrawModel.comment).comment = "B2BSUCCESS";
                _dbContext.SaveChanges();
                TempData["CheckSuccess"] = "Withdraw are successed";
                return RedirectToAction("admin", "Profile");
            }
            if (withdrawModel.currency == "PLN")
            {
                _dbContext.Withdraw.FirstOrDefault(x => x.comment == withdrawModel.comment).comment = "B2BSUCCESS";
                _dbContext.SaveChanges();
                TempData["CheckSuccess"] = "Withdraw are successed";
                return RedirectToAction("admin", "Profile");
            }
            if (withdrawModel.currency == "RUB")
            {
                _dbContext.Withdraw.FirstOrDefault(x => x.comment == withdrawModel.comment).comment = "B2BSUCCESS";
                _dbContext.SaveChanges();
                TempData["CheckSuccess"] = "Withdraw are successed";
                return RedirectToAction("admin", "Profile");
            }
            else
            {
                TempData["CheckFailed"] = "Currency not found";
                return RedirectToAction("admin", "Profile");
            }
        }
        else
        {
            TempData["CheckFailed"] = "Data is incorrect";
            return RedirectToAction("admin", "Profile");
        }
    }

    [HttpPost]
    public IActionResult b2bWithdrawCancel(WithdrawModel withdrawModel)
    {
        var targetUser = _dbContext.UserData.FirstOrDefault(x => x.userId == withdrawModel.userId);
        if (ModelState.IsValid)
        {
            if (withdrawModel.currency == "USD")
            {
                _dbContext.Withdraw.FirstOrDefault(x => x.comment == withdrawModel.comment).comment = "B2BCANCEL";
                Math.Round(targetUser.balance_usd += withdrawModel.summa, 2);
                _dbContext.SaveChanges();
                TempData["CheckSuccess"] = "Cancel are successed";
                return RedirectToAction("admin", "Profile");
            }
            if (withdrawModel.currency == "EUR")
            {
                _dbContext.Withdraw.FirstOrDefault(x => x.comment == withdrawModel.comment).comment = "B2BCANCEL";
                Math.Round(targetUser.balance_eur += withdrawModel.summa, 2);
                _dbContext.SaveChanges();
                TempData["CheckSuccess"] = "Cancel are successed";
                return RedirectToAction("admin", "Profile");
            }
            if (withdrawModel.currency == "PLN")
            {
                _dbContext.Withdraw.FirstOrDefault(x => x.comment == withdrawModel.comment).comment = "B2BCANCEL";
                Math.Round(targetUser.balance_pln += withdrawModel.summa, 2);
                _dbContext.SaveChanges();
                TempData["CheckSuccess"] = "Cancel are successed";
                return RedirectToAction("admin", "Profile");
            }
            if (withdrawModel.currency == "RUB")
            {
                _dbContext.Withdraw.FirstOrDefault(x => x.comment == withdrawModel.comment).comment = "B2BCANCEL";
                Math.Round(targetUser.balance_rub += withdrawModel.summa, 2);
                _dbContext.SaveChanges();
                TempData["CheckSuccess"] = "Cancel are successed";
                return RedirectToAction("admin", "Profile");
            }
            else
            {
                TempData["CheckFailed"] = "Currency not found";
                return RedirectToAction("admin", "Profile");
            }
        }
        else
        {
            TempData["CheckFailed"] = "Cancel is failed";
            return RedirectToAction("admin", "Profile");
        }
    }

    [HttpPost]
    public IActionResult giveMoney(AddBalance addBalance)
    {
        var targetUser = _dbContext.UserData.FirstOrDefault(x => x.userId == addBalance.userId);
        var findByStatus = _dbContext.AddBalanceRequest.Where(x => x.status == addBalance.status);
        foreach (var line in findByStatus)
        {
            if (line.userId == addBalance.userId && line.summa == addBalance.summa && line.currency == addBalance.currency)
            {
                if (line.currency == "USD")
                {
                    Math.Round(targetUser.balance_usd += addBalance.summa, 2);
                    line.status = "SUCCESS";
                }
                if (addBalance.currency == "EUR")
                {
                    Math.Round(targetUser.balance_eur += addBalance.summa, 2);
                    line.status = "SUCCESS";
                }
                if (addBalance.currency == "PLN")
                {
                    Math.Round(targetUser.balance_pln += addBalance.summa, 2);
                    line.status = "SUCCESS";
                }
                if (addBalance.currency == "RUB")
                {
                    Math.Round(targetUser.balance_rub += addBalance.summa, 2);
                    line.status = "SUCCESS";
                }
                line.status = "SUCCESS";
                line.comment = addBalance.comment;
            }
        }
        _dbContext.SaveChanges();
        TempData["CheckSuccess"] = $"Money to {targetUser.email} sended";
        return RedirectToAction("admin", "Profile");
        // TempData["CheckFailed"] = "Give a money is failed";
        // return RedirectToAction("admin", "Profile");
    }

    [HttpPost]
    public IActionResult takeMoney(AddBalance addBalance)
    {
        var targetUser = _dbContext.UserData.FirstOrDefault(x => x.userId == addBalance.userId);
        var findByStatus = _dbContext.AddBalanceRequest.Where(x => x.status == addBalance.status);
        foreach (var line in findByStatus)
        {
            if (line.userId == addBalance.userId && line.summa == addBalance.summa && line.currency == addBalance.currency)
            {
                line.status = "CANCELLED";
                line.comment = addBalance.comment;
                TempData["CheckSuccess"] = $"Money to {targetUser.email} TAKEN";
            }
            // else
            // {
            //     TempData["CheckFailed"] = "Take a money is failed";
            //     return RedirectToAction("admin", "Profile");
            // }
        }
        _dbContext.SaveChanges();
        return RedirectToAction("admin", "Profile");
    }

    [HttpPost]
    public IActionResult bankChange(Wallets wallets)
    {
        var targetBank = _dbContext.Wallets.FirstOrDefault(x => x.Id == 0);
        targetBank.PKOBankPolski = wallets.PKOBankPolski;
        targetBank.TinkoffBank = wallets.TinkoffBank;
        _dbContext.SaveChanges();
        TempData["CheckSuccess"] = "Banks is changed";
        return RedirectToAction("admin", "Profile");
    }

    [HttpPost]
    public IActionResult userControl(UserData userData)
    {
        return View();
    }

    [HttpPost]
    public IActionResult userRemove(UserData userData)
    {
        return View();
    }

    [Route("/logout")]
    public async Task<IActionResult> logout()
    {
        await base.HttpContext.SignOutAsync();
        return RedirectToAction("index", "Page");
    }
}