using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using UFOPay.Models;

public class UFODbContext : DbContext
{
    public DbSet<UserData> UserData { get; set; }
    public DbSet<TransferModel> Transactions { get; set; }
    public DbSet<AddBalance> AddBalanceRequest { get; set; }
    public DbSet<Wallets> Wallets { get; set; }
    public DbSet<ConvertModel> Convertation { get; set; }
    public DbSet<WithdrawModel> Withdraw { get; set; }
    public DbSet<B2Bwithdraw> B2Bwithdraw { get; set; }
    public DbSet<B2Bkassa> B2BKassa { get; set; }
    public DbSet<B2BBalance> B2BBalance { get; set; }
    public DbSet<BusinessBills> BusinessBills { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string projectPath = AppDomain.CurrentDomain.BaseDirectory.Split(new string[1] { "bin\\" }, StringSplitOptions.None)[0];
        IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(projectPath).AddJsonFile("appsettings.json").Build();
        string connectionString = configuration.GetConnectionString("DefaultConnection");
        optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        base.OnConfiguring(optionsBuilder);
    }
}