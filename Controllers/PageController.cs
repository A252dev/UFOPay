using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using UFOPay.Models;

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace UFOPay.Controllers;

public class PageController : Controller
{
    private readonly ILogger<PageController> _logger;
    private readonly UFODbContext _dbContext;

    public PageController(ILogger<PageController> logger, UFODbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public int GenerateUserId()
    {
        Random random = new Random();
        int userId = random.Next(Int32.MinValue, Int32.MaxValue);
        while (_dbContext.UserData.Any(x => x.userId == userId))
            continue;
        return userId;
    }

    public IActionResult index()
    {
        return View();
    }

    [Route("/register")]
    public IActionResult register()
    {
        return View();
    }

    [Route("/register")]
    [HttpPost]
    public async Task<IActionResult> register(UserData userData)
    {
        if (User.Identity.Name == null)
        {
            if (userData.firstName != null && userData.secondName != null && userData.email != null && userData.password != null && userData.repeatPassword != null && userData.birthday != null)
            {
                if (userData.password == userData.repeatPassword)
                {
                    if (userData.AgreeWithDocs == true)
                    {
                        List<Claim> claims = new List<Claim>(){
                            new Claim(ClaimTypes.NameIdentifier, userData.email),
                        };

                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                            CookieAuthenticationDefaults.AuthenticationScheme);

                        AuthenticationProperties properties = new AuthenticationProperties()
                        {
                            AllowRefresh = true,
                            IsPersistent = userData.KeepLoggedIn
                        };

                        bool haveTargetUser = _dbContext.UserData.Any(x => x.email == userData.email);
                        if (haveTargetUser)
                        {
                            ViewData["ValidateMessage"] = "The user is registered";
                            return RedirectToAction("login", "Page");
                        }
                        else
                        {
                            userData.userId = GenerateUserId();
                            userData.registrationData = DateTime.Now;
                            userData.balance_eur = 0;
                            userData.balance_usd = 0;
                            userData.balance_pln = 0;
                            userData.balance_rub = 0;
                            userData.passport = "NULL";
                            _dbContext.Add(userData);
                            _dbContext.SaveChanges();

                            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                                        new ClaimsPrincipal(claimsIdentity), properties);

                            TempData["ValidateSuccessMessage"] = "The user has been successfully registered";
                            return RedirectToAction("login", "Page");
                        }
                    }
                    else
                    {
                        TempData["RequirementsErrorMessage"] = "You need to aceept the requirements";
                        return RedirectToAction("register", "Page");
                    }

                }
                else
                {
                    TempData["ValidateMessage"] = "Passwords is incorrect";
                    return RedirectToAction("register", "Page");
                }
            }
            else
            {
                TempData["ValidateMessage"] = "Data is incorrect";
                return RedirectToAction("register", "Page");
            }
        }
        else
        {
            TempData["ValidateMessage"] = "Data is incorrect";
            return RedirectToAction("register", "Page");
        }
    }

    [Route("/login")]
    public IActionResult login(APILogin login)
    {
        if (login.apiKey != null)
        {
            return RedirectToAction("index", "Page");
            // make a transfer to the API responce page
        }
        return View();
    }

    [HttpPost]
    [Route("/login")]
    public async Task<IActionResult> login(UserData userData)
    {
        if (User.Identity.Name == null)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, userData.email));
            var id = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true,
                IsPersistent = userData.KeepLoggedIn
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(id), properties);

            if (userData.email != null && userData.password != null)
            {
                var targetUser = _dbContext.UserData.FirstOrDefault(x => x.email == userData.email);
                if (targetUser != null)
                {
                    if (targetUser.password == userData.password)
                    {
                        // if password in db ok so redirect to profile
                        return RedirectToAction("profile", "Profile");
                    }
                    else
                    {
                        // if password not ok make a msg on page
                        TempData["ValidateMessage"] = "Password is incorrect";
                        return View();
                    }
                }
                else
                {
                    TempData["ValidateMessage"] = "User not found";
                    return View();
                }
            }
            else
            {
                TempData["ValidateMessage"] = "Data is incorrect";
                return View();
            }
        }
        else
        {
            TempData["ValidateMessage"] = "Data is incorrect";
            return View();
        }
    }

    [Route("/billing")]
    public IActionResult billing()
    {
        return View();
    }

    [Route("/user_agreement")]
    public IActionResult user_agreement()
    {
        return View();
    }

    [Route("/privacy_policy")]
    public IActionResult privacy_policy()
    {
        return View();
    }

    [Route("/data_protection")]
    public IActionResult data_protection()
    {
        return View();
    }

    [Route("/contacts")]
    public IActionResult contacts()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
