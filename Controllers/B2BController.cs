using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public IActionResult b2b()
    {
        return View();
    }

    [HttpPost]
    public IActionResult createNewB2B(B2Bcreate b2bKassa)
    {
        return View();
    }

    [HttpPost]
    public IActionResult withdrawB2B(B2Bwithdraw b2Bwithdraw)
    {
        return View();
    }
}