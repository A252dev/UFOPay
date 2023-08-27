using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

    [Route("/profile")]
    public IActionResult profile()
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
        // will in update
        return RedirectToAction("profile", "Page");
    }

    [Route("/logout")]
    public async Task<IActionResult> logout()
    {
        await base.HttpContext.SignOutAsync();
        return RedirectToAction("index", "Page");
    }
}