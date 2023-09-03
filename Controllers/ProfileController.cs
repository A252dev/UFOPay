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
                comment = transferModel.comment
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
                    targetForTheTransferUser.balance_usd += transferModel.summa;
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
                    targetForTheTransferUser.balance_eur += transferModel.summa;
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
                    targetForTheTransferUser.balance_pln += transferModel.summa;
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
                    targetForTheTransferUser.balance_rub += transferModel.summa;
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
        if (addBalance.summa != null && addBalance.currency != null)
        {
            var user = _dbContext.UserData.FirstOrDefault(x => x.email == User.Identity.Name);

            AddBalance addNewStatus = new AddBalance()
            {
                userId = user.userId,
                currency = addBalance.currency,
                summa = addBalance.summa,
                status = "PENDING",
                comment = GenerateComment(),
            };
            _dbContext.AddBalanceRequest.Update(addNewStatus);
            _dbContext.SaveChanges();
            TempData["BillCreated"] = "Bill has been created";
            return RedirectToAction("billing", "Page");
        }
        else
        {
            TempData["BillErrorMessage"] = "Data is incorrect";
            return RedirectToAction("profile", "Profile");
        }
    }

    [HttpPost]
    public IActionResult billConfirm(AddBalance addBalance)
    {
        if (ModelState.IsValid)
        {
            var targetUser = _dbContext.AddBalanceRequest.FirstOrDefault(x => x.userId == addBalance.userId);
            targetUser.summa = addBalance.summa;
            targetUser.currency = addBalance.currency;
            targetUser.status = addBalance.status;
            targetUser.comment = addBalance.comment;
            _dbContext.SaveChanges();
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
                    targetUser.balance_usd += addBalance.summa;
                    line.status = "SUCCESS";
                }
                if (addBalance.currency == "EUR")
                {
                    targetUser.balance_eur += addBalance.summa;
                    line.status = "SUCCESS";
                }
                if (addBalance.currency == "PLN")
                {
                    targetUser.balance_pln += addBalance.summa;
                    line.status = "SUCCESS";
                }
                if (addBalance.currency == "RUB")
                {
                    targetUser.balance_rub += addBalance.summa;
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