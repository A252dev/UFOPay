using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            userData.secondName = user.secondName;
            userData.birthday = user.birthday;
            userData.password = user.password;
            _dbContext.SaveChanges();
        }
        if (userData.secondName != null)
        {
            user.secondName = userData.secondName;
            userData.firstName = user.firstName;
            userData.birthday = user.birthday;
            userData.password = user.password;
            _dbContext.SaveChanges();
        }
        if (userData.birthday != null)
        {
            user.birthday = userData.birthday;
            userData.secondName = user.secondName;
            userData.firstName = user.firstName;
            userData.password = user.password;
            _dbContext.SaveChanges();
        }
        if (userData.password != null)
        {
            user.password = userData.password;
            userData.birthday = user.birthday;
            userData.secondName = user.secondName;
            userData.firstName = user.firstName;
            _dbContext.SaveChanges();
        }
        _dbContext.SaveChanges();
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
                comment = transferModel.comment
            };
            user.balance -= transferModel.summa;
            targetForTheTransferUser.balance += transferModel.summa;
            _dbContext.Transactions.Update(transfer);
            _dbContext.SaveChanges();
            return RedirectToAction("profile", "Profile");
        }
        return RedirectToAction("profile", "Profile");
    }

    public async Task<IActionResult> logout()
    {
        await base.HttpContext.SignOutAsync();
        return RedirectToAction("index", "Page");
    }
}