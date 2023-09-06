using Microsoft.AspNetCore.Authentication.Cookies;
using UFOPay.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.Configure<GoogleCaptchaConfig>(builder.Configuration.GetSection("GoogleReCaptcha"));

builder.Services.AddAuthentication()
    .AddCookie(delegate (CookieAuthenticationOptions option)
    {
        option.LoginPath = "/login";
        option.ExpireTimeSpan = TimeSpan.FromMinutes(20.0);
    });

builder.Services.AddScoped<UFODbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Page}/{action=index}/{id?}");

app.Run();
