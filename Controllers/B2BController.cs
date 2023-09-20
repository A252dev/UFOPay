using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UFOPay.Models;

namespace UFOPay.Controllers;

[Authorize]
public class B2BController : Controller
{
    private readonly UFODbContext _dbContext;
    public B2BController(UFODbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public string GenerateComment()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        Random random = new Random();
        string comment = new string(Enumerable.Repeat(chars, 10) // 10 this is number of chars in comment
        .Select(s => s[random.Next(s.Length)]).ToArray());
        while (_dbContext.B2BKassa.Any(x => x.comment == comment))
            continue;
        return comment;
    }

    public int GenerateKassaId()
    {
        Random random = new Random();
        int kassaId = random.Next(Int32.MinValue, Int32.MaxValue);
        while (_dbContext.B2BKassa.Any(x => x.kassaId == kassaId))
            continue;
        return kassaId;
    }

    public string GenerateApiKey()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        Random random = new Random();
        string apiKey = new string(Enumerable.Repeat(chars, 32) // 32 this is number of chars in apikey
        .Select(s => s[random.Next(s.Length)]).ToArray());
        while (_dbContext.B2BKassa.Any(x => x.apiKey == apiKey))
            continue;
        return apiKey;
    }

    [Route("/profile/business")]
    public IActionResult b2b()
    {
        return View();
    }

    [HttpPost]
    public IActionResult createNewB2B(B2Bkassa b2bKassa)
    {
        if (b2bKassa.website != null && b2bKassa.description != null && b2bKassa.contactEmail != null)
        {
            bool targetKassa = _dbContext.B2BKassa.Any(x => x.website.Contains(b2bKassa.website));
            if (targetKassa == false)
            {
                if (_dbContext.B2BKassa.FirstOrDefault(x => x.userId == b2bKassa.userId) != null)
                {
                    TempData["B2BError"] = "You are have kassa!";
                    return RedirectToAction("b2b", "B2B");
                }
                else
                {
                    int kassaId = GenerateKassaId();
                    B2Bkassa kassa = new B2Bkassa()
                    {
                        userId = b2bKassa.userId,
                        kassaId = kassaId,
                        website = b2bKassa.website,
                        description = b2bKassa.description,
                        contactEmail = b2bKassa.contactEmail,
                        status = "WAITING",
                        comment = GenerateComment(),
                    };

                    B2BBalance balance = new B2BBalance()
                    {
                        userId = b2bKassa.userId,
                        kassaId = kassaId,
                        b2bAccess = true,
                        b2b_balance_usd = 0,
                        b2b_balance_eur = 0,
                        b2b_balance_pln = 0,
                        b2b_balance_rub = 0,
                    };

                    _dbContext.B2BKassa.Update(kassa);
                    _dbContext.B2BBalance.Update(balance);
                    _dbContext.SaveChanges();
                    TempData["B2BSuccess"] = "Invoice for open a kassa is created";
                    return RedirectToAction("b2b", "B2B");
                }
            }
            else
            {
                TempData["B2BError"] = "Website is use right now";
                return RedirectToAction("b2b", "B2B");
            }
        }
        else
        {
            TempData["B2BError"] = "Data is incorrect";
            return RedirectToAction("b2b", "B2B");
        }
    }

    [HttpPost]
    public IActionResult confirmNewB2B(B2Bkassa B2BKassa)
    {
        var targetKassa = _dbContext.B2BKassa.FirstOrDefault(x => x.comment == B2BKassa.comment);
        if (targetKassa != null)
        {
            targetKassa.status = "CONFIRMED";
            targetKassa.apiKey = GenerateApiKey();
            _dbContext.SaveChanges();
            return RedirectToAction("admin", "Profile");
        }
        else
        {
            TempData["CheckFailed"] = "Data is incorrect";
            return RedirectToAction("admin", "Profile");
        }
    }

    [HttpPost]
    public IActionResult cancelNewB2B(B2Bkassa B2BKassa)
    {
        var targetKassa = _dbContext.B2BKassa.FirstOrDefault(x => x.comment == B2BKassa.comment);
        if (targetKassa != null)
        {
            targetKassa.status = "CANCELLED";
            _dbContext.SaveChanges();
            return RedirectToAction("admin", "Profile");
        }
        else
        {
            TempData["CheckFailed"] = "Data is incorrect";
            return RedirectToAction("admin", "Profile");
        }
    }

    [HttpPost]
    public IActionResult withdrawB2B(B2Bwithdraw b2Bwithdraw)
    {
        if (b2Bwithdraw.bank != null && b2Bwithdraw.currency != null && b2Bwithdraw.summa != 0)
        {
            var user = _dbContext.UserData.FirstOrDefault(x => x.email == User.Identity.Name);
            WithdrawModel model = new WithdrawModel()
            {
                userId = user.userId,
                currency = b2Bwithdraw.currency,
                bank = b2Bwithdraw.bank,
                summa = b2Bwithdraw.summa,
                comment = GenerateComment() + " B2B",
                withdrawData = DateTime.Now,
            };

            var targetKassa = _dbContext.B2BKassa.FirstOrDefault(x => x.userId == user.userId);
            var targetBalance = _dbContext.B2BBalance.FirstOrDefault(x => x.kassaId == targetKassa.kassaId);

            if (b2Bwithdraw.currency == "USD")
            {
                if (targetBalance.b2b_balance_usd > b2Bwithdraw.summa)
                {
                    Math.Round(targetBalance.b2b_balance_usd -= b2Bwithdraw.summa, 2);
                    _dbContext.Withdraw.Update(model);
                    _dbContext.SaveChanges();
                    TempData["B2BSuccess"] = "Invoice is created";
                    return RedirectToAction("b2b", "B2B");
                }
                else
                {
                    TempData["B2BError"] = "Not enouth money";
                    return RedirectToAction("b2b", "B2B");
                }
            }
            if (b2Bwithdraw.currency == "EUR")
            {
                if (targetBalance.b2b_balance_eur > b2Bwithdraw.summa)
                {
                    Math.Round(targetBalance.b2b_balance_eur -= b2Bwithdraw.summa, 2);
                    _dbContext.Withdraw.Update(model);
                    _dbContext.SaveChanges();
                    TempData["B2BSuccess"] = "Invoice is created";
                    return RedirectToAction("b2b", "B2B");
                }
                else
                {
                    TempData["B2BError"] = "Not enouth money";
                    return RedirectToAction("b2b", "B2B");
                }
            }
            if (b2Bwithdraw.currency == "PLN")
            {
                if (targetBalance.b2b_balance_pln > b2Bwithdraw.summa)
                {
                    Math.Round(targetBalance.b2b_balance_pln -= b2Bwithdraw.summa, 2);
                    _dbContext.Withdraw.Update(model);
                    _dbContext.SaveChanges();
                    TempData["B2BSuccess"] = "Invoice is created";
                    return RedirectToAction("b2b", "B2B");
                }
                else
                {
                    TempData["B2BError"] = "Not enouth money";
                    return RedirectToAction("b2b", "B2B");
                }
            }
            if (b2Bwithdraw.currency == "RUB")
            {
                if (targetBalance.b2b_balance_rub > b2Bwithdraw.summa)
                {
                    Math.Round(targetBalance.b2b_balance_rub -= b2Bwithdraw.summa, 2);
                    _dbContext.Withdraw.Update(model);
                    _dbContext.SaveChanges();
                    TempData["B2BSuccess"] = "Invoice is created";
                    return RedirectToAction("b2b", "B2B");
                }
                else
                {
                    TempData["B2BError"] = "Not enouth money";
                    return RedirectToAction("b2b", "B2B");
                }
            }
            TempData["B2BError"] = "Data is incorrect";
            return RedirectToAction("b2b", "B2B");
        }
        else
        {
            TempData["B2BError"] = "Withdraw error";
            return RedirectToAction("b2b", "B2B");
        }
    }
}