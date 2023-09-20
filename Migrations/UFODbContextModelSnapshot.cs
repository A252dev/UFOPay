﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace UFOPay.Migrations
{
    [DbContext(typeof(UFODbContext))]
    partial class UFODbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("TransferModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("comment")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("currency")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("reciever")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("sender")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<long>("summa")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("transferData")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("UFOPay.Models.AddBalance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("comment")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("currency")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("status")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<long>("summa")
                        .HasColumnType("bigint");

                    b.Property<int>("userId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("AddBalanceRequest");
                });

            modelBuilder.Entity("UFOPay.Models.B2BBalance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("b2bAccess")
                        .HasColumnType("tinyint(1)");

                    b.Property<double>("b2b_balance_eur")
                        .HasColumnType("double");

                    b.Property<double>("b2b_balance_pln")
                        .HasColumnType("double");

                    b.Property<double>("b2b_balance_rub")
                        .HasColumnType("double");

                    b.Property<double>("b2b_balance_usd")
                        .HasColumnType("double");

                    b.Property<int>("kassaId")
                        .HasColumnType("int");

                    b.Property<int>("userId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("B2BBalance");
                });

            modelBuilder.Entity("UFOPay.Models.B2Bkassa", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("apiKey")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("comment")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("contactEmail")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("kassaId")
                        .HasColumnType("int");

                    b.Property<string>("status")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("userId")
                        .HasColumnType("int");

                    b.Property<string>("website")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("B2BKassa");
                });

            modelBuilder.Entity("UFOPay.Models.B2Bwithdraw", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("bank")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("currency")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<double>("summa")
                        .HasColumnType("double");

                    b.Property<int>("userId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("B2Bwithdraw");
                });

            modelBuilder.Entity("UFOPay.Models.BusinessBills", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("apiKey")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("billingId")
                        .HasColumnType("int");

                    b.Property<string>("currency")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("kassaId")
                        .HasColumnType("int");

                    b.Property<int>("paidUserId")
                        .HasColumnType("int");

                    b.Property<DateTime>("paymentData")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("status")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("summa")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("BusinessBills");
                });

            modelBuilder.Entity("UFOPay.Models.ConvertModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("comment")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("currencyFrom")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("currencyTo")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("status")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("summa")
                        .HasColumnType("int");

                    b.Property<int>("userId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Convertation");
                });

            modelBuilder.Entity("UFOPay.Models.UserData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("AgreeWithDocs")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("KeepLoggedIn")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("admin")
                        .HasColumnType("tinyint(1)");

                    b.Property<double>("balance_eur")
                        .HasColumnType("double");

                    b.Property<double>("balance_pln")
                        .HasColumnType("double");

                    b.Property<double>("balance_rub")
                        .HasColumnType("double");

                    b.Property<double>("balance_usd")
                        .HasColumnType("double");

                    b.Property<string>("birthday")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("firstName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("passport")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("registrationData")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("repeatPassword")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("secondName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("userId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("UserData");
                });

            modelBuilder.Entity("UFOPay.Models.Wallets", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("PKOBankPolski")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("TinkoffBank")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Wallets");
                });

            modelBuilder.Entity("UFOPay.Models.WithdrawModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("bank")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("comment")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("currency")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<double>("summa")
                        .HasColumnType("double");

                    b.Property<int>("userId")
                        .HasColumnType("int");

                    b.Property<DateTime>("withdrawData")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.ToTable("Withdraw");
                });
#pragma warning restore 612, 618
        }
    }
}
