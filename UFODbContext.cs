using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UFOPay.Models;

public class UFODbContext : DbContext
{
    public DbSet<UserData> UserData { get; set; }
    public DbSet<TransferModel> Transactions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string projectPath = AppDomain.CurrentDomain.BaseDirectory.Split(new string[1] { "bin\\" }, StringSplitOptions.None)[0];
        IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(projectPath).AddJsonFile("appsettings.json").Build();
        string connectionString = configuration.GetConnectionString("DefaultConnection");
        optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        base.OnConfiguring(optionsBuilder);
    }
}