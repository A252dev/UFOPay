using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using UFOPay.Models;

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace UFOPay.Controllers;

public class PageController : Controller
{
    private readonly ILogger<PageController> _logger;
    private readonly UFODbContext _dbContext;
    private readonly GoogleCaptchaService _captchaService;

    public PageController(ILogger<PageController> logger, UFODbContext dbContext, GoogleCaptchaService captchaService)
    {
        _logger = logger;
        _dbContext = dbContext;
        _captchaService = captchaService;
    }

    public int GenerateUserId()
    {
        Random random = new Random();
        int userId = random.Next(Int32.MinValue, Int32.MaxValue);
        while (_dbContext.UserData.Any(x => x.userId == userId))
            continue;
        return userId;
    }

    public int GenerateBillingId()
    {
        Random random = new Random();
        int billingId = random.Next(Int32.MinValue, Int32.MaxValue);
        while (_dbContext.BusinessBills.Any(x => x.billingId == billingId))
            continue;
        return billingId;
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
    public async Task<IActionResult> register(UserData userData, GoogleCaptchaConfig googleCaptchaConfig)
    {
        var captchaResult = await _captchaService.VerifyToken(googleCaptchaConfig.captchaToken);
        if (!captchaResult)
        {
            TempData["ValidateMessage"] = "captcha...";
            return View();
        }
        else
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
    public async Task<IActionResult> login(UserData userData, GoogleCaptchaConfig googleCaptchaConfig)
    {
        var captchaResult = await _captchaService.VerifyToken(googleCaptchaConfig.captchaToken);

        if (!captchaResult)
        {
            TempData["ValidateMessage"] = "captcha...";
            return View();
        }
        else
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

                if (userData.email != null && userData.password != null)
                {
                    var targetUser = _dbContext.UserData.FirstOrDefault(x => x.email == userData.email);
                    if (targetUser != null)
                    {
                        if (targetUser.password == userData.password)
                        {
                            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                new ClaimsPrincipal(id), properties);
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
                TempData["ValidateMessage"] = "You are logged";
                return View();
            }
        }
    }

    [HttpGet]
    [Route("/billing")]
    public IActionResult billing(int billingId)
    {
        var billInfo = _dbContext.BusinessBills.FirstOrDefault(x => x.billingId == billingId);
        if (billInfo != null)
        {
            billingId = billInfo.billingId;
            TempData["billingId"] = billingId;
            return View();
        }
        else if (TempData["BillCreated"] != null)
        {
            return View();
        }
        else
        {
            return RedirectToAction("profile", "Profile");
        }
    }

    [HttpGet]
    [Route("/api/billing")]
    public IActionResult bill_create(BusinessBills businessBills)
    {
        if (businessBills.apiKey != null)
        {
            var targetKey = _dbContext.B2BKassa.FirstOrDefault(x => x.apiKey == businessBills.apiKey);
            if (targetKey != null)
            {
                if (businessBills.summa == 0 && businessBills.currency == null)
                {
                    var authSuccess = new
                    {
                        authorization = "success",
                        kassaId = $"{targetKey.kassaId}",
                        apiKey = $"{targetKey.apiKey}"
                    };
                    var authSuccessMessage = JsonConvert.SerializeObject(authSuccess);
                    TempData["json"] = authSuccessMessage;
                    return View();
                }
                if (businessBills.currency == "USD")
                {
                    if (businessBills.summa != 0)
                    {
                        int billingId = GenerateBillingId();
                        BusinessBills bill = new BusinessBills()
                        {
                            kassaId = targetKey.kassaId,
                            billingId = billingId,
                            paidUserId = 0,
                            status = "CREATED",
                            summa = businessBills.summa,
                            currency = businessBills.currency,
                            apiKey = businessBills.apiKey,
                        };

                        _dbContext.BusinessBills.Update(bill);
                        _dbContext.SaveChanges();

                        var targetBill = _dbContext.BusinessBills.FirstOrDefault(x => x.billingId == billingId);
                        var billCreate = new
                        {
                            bill_status = $"{targetBill.status}",
                            billing_id = $"{targetBill.billingId}",
                            apiKey = $"{targetKey.apiKey}"
                        };

                        var billCreateString = JsonConvert.SerializeObject(billCreate);

                        TempData["json"] = billCreateString;
                        return View();
                    }
                    else
                    {
                        var summaIsNull = new
                        {
                            message = "summa is null",
                            billing_id = $"{targetKey.kassaId}",
                            apiKey = $"{targetKey.apiKey}"
                        };
                        var summaIsNullMessage = JsonConvert.SerializeObject(summaIsNull);

                        TempData["json"] = summaIsNullMessage;
                        return View();
                    }
                }
                else if (businessBills.currency == "EUR")
                {
                    if (businessBills.summa != 0)
                    {
                        int billingId = GenerateBillingId();
                        BusinessBills bill = new BusinessBills()
                        {
                            kassaId = targetKey.kassaId,
                            billingId = billingId,
                            paidUserId = 0,
                            status = "CREATED",
                            summa = businessBills.summa,
                            currency = businessBills.currency,
                            apiKey = businessBills.apiKey,
                        };

                        _dbContext.BusinessBills.Update(bill);
                        _dbContext.SaveChanges();

                        var targetBill = _dbContext.BusinessBills.FirstOrDefault(x => x.billingId == billingId);
                        var billCreate = new
                        {
                            bill_status = $"{targetBill.status}",
                            billing_id = $"{targetBill.billingId}",
                            apiKey = $"{targetKey.apiKey}"
                        };

                        var billCreateString = JsonConvert.SerializeObject(billCreate);

                        TempData["json"] = billCreateString;
                        return View();
                    }
                    else
                    {
                        var summaIsNull = new
                        {
                            message = "summa is null",
                            billing_id = $"{targetKey.kassaId}",
                            apiKey = $"{targetKey.apiKey}"
                        };
                        var summaIsNullMessage = JsonConvert.SerializeObject(summaIsNull);

                        TempData["json"] = summaIsNullMessage;
                        return View();
                    }
                }
                else if (businessBills.currency == "PLN")
                {
                    if (businessBills.summa != 0)
                    {
                        int billingId = GenerateBillingId();
                        BusinessBills bill = new BusinessBills()
                        {
                            kassaId = targetKey.kassaId,
                            billingId = billingId,
                            paidUserId = 0,
                            status = "CREATED",
                            summa = businessBills.summa,
                            currency = businessBills.currency,
                            apiKey = businessBills.apiKey,
                        };

                        _dbContext.BusinessBills.Update(bill);
                        _dbContext.SaveChanges();

                        var targetBill = _dbContext.BusinessBills.FirstOrDefault(x => x.billingId == billingId);
                        var billCreate = new
                        {
                            bill_status = $"{targetBill.status}",
                            billing_id = $"{targetBill.billingId}",
                            apiKey = $"{targetKey.apiKey}"
                        };

                        var billCreateString = JsonConvert.SerializeObject(billCreate);

                        TempData["json"] = billCreateString;
                        return View();
                    }
                    else
                    {
                        var summaIsNull = new
                        {
                            message = "summa is null",
                            billing_id = $"{targetKey.kassaId}",
                            apiKey = $"{targetKey.apiKey}"
                        };
                        var summaIsNullMessage = JsonConvert.SerializeObject(summaIsNull);

                        TempData["json"] = summaIsNullMessage;
                        return View();
                    }
                }
                else if (businessBills.currency == "RUB")
                {
                    if (businessBills.summa != 0)
                    {
                        int billingId = GenerateBillingId();
                        BusinessBills bill = new BusinessBills()
                        {
                            kassaId = targetKey.kassaId,
                            billingId = billingId,
                            paidUserId = 0,
                            status = "CREATED",
                            summa = businessBills.summa,
                            currency = businessBills.currency,
                            apiKey = businessBills.apiKey,
                        };

                        _dbContext.BusinessBills.Update(bill);
                        _dbContext.SaveChanges();

                        var targetBill = _dbContext.BusinessBills.FirstOrDefault(x => x.billingId == billingId);
                        var billCreate = new
                        {
                            bill_status = $"{targetBill.status}",
                            billing_id = $"{targetBill.billingId}",
                            apiKey = $"{targetKey.apiKey}"
                        };

                        var billCreateString = JsonConvert.SerializeObject(billCreate);

                        TempData["json"] = billCreateString;
                        return View();
                    }
                    else
                    {
                        var summaIsNull = new
                        {
                            message = "summa is null",
                            billing_id = $"{targetKey.kassaId}",
                            apiKey = $"{targetKey.apiKey}"
                        };
                        var summaIsNullMessage = JsonConvert.SerializeObject(summaIsNull);

                        TempData["json"] = summaIsNullMessage;
                        return View();
                    }
                }
                else
                {
                    var currencyError = new
                    {
                        message = "currency error",
                    };
                    var currencyErrorMessage = JsonConvert.SerializeObject(currencyError);
                    TempData["json"] = currencyErrorMessage;
                    return View();
                }
            }
            else
            {
                var authFailed = new
                {
                    authorization = "failed",
                    apiKey = $"{targetKey.apiKey}"
                };
                var authFailedMessage = JsonConvert.SerializeObject(authFailed);

                TempData["json"] = authFailedMessage;
                return View();
            }
        }
        else
        {
            return RedirectToAction("index", "Page");
        }
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
