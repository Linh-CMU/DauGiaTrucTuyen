using Azure.Core;
using BusinessObject.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace BusinessObject.Context
{
    public class ConnectDB : IdentityDbContext<Account>
    {
        public ConnectDB() { }
        public ConnectDB(DbContextOptions<ConnectDB> options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot configuration = builder.Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("Capstone"));
        }
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<AccountDetail> AccountDetails { get; set; }
        public virtual DbSet<PlacingABid> Bets { get; set; }
        public virtual DbSet<Category> Categorys { get; set; }
        public virtual DbSet<Feedback> Feedbacks { get; set; }
        public virtual DbSet<DigitalSignature> FileAttachments { get; set; }
        public virtual DbSet<ListAuction> ListAuctions { get; set; }
        public virtual DbSet<Notification> Notications { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<RegistAuction> RegistAuctioneers { get; set; }
        public virtual DbSet<TImage> TImages { get; set; }
        public virtual DbSet<AuctionDetail> AuctionDetails { get; set; }
        public virtual DbSet<Deposit> Deposits { get; set; }
        public virtual DbSet<UserOtp> UserOtp { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<IdentityUserLogin<string>>()
                .HasKey(l => new { l.LoginProvider, l.ProviderKey, l.UserId });
            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.RegistAuctions)
                .WithOne(ra => ra.Feedbacks)
                .HasForeignKey<Feedback>(f => f.RAID)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<PlacingABid>()
                .HasOne(b => b.RegistAuctioneer)
                .WithMany(r => r.Bets)
                .HasForeignKey(b => b.RAID);
            modelBuilder.Entity<RegistAuction>()
                .HasOne(b => b.ListAuctions)
                .WithMany(r => r.RegistAuctions)
                .HasForeignKey(b => b.ListAuctionID);
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.RegistAuctions)
                .WithMany(r => r.Payments)
                .HasForeignKey(p => p.RAID)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<DigitalSignature>()
                .HasOne(p => p.AuctionDetails)
                .WithMany(r => r.FileAttachments)
                .HasForeignKey(p => p.ListAuctionID)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<ListAuction>()
                .HasOne(p => p.CreatorAccount)
                .WithMany(r => r.CreatedAuctions)
                .HasForeignKey(p => p.Creator)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<ListAuction>()
                .HasOne(p => p.ManagerAccount)
                .WithMany(r => r.ManagedAuctions)
                .HasForeignKey(p => p.Manager)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Deposit>()
                .HasOne(p => p.RegistAuctions)
                .WithMany(r => r.Deposits)
                .HasForeignKey(p => p.RAID)
                .OnDelete(DeleteBehavior.NoAction);
            var adminRoleId = Guid.NewGuid().ToString();
            //modelBuilder.Entity<IdentityRole>().HasData(
            //  new IdentityRole { Id = adminRoleId, Name = "admin", NormalizedName = "ADMIN" },
            //  new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "user", NormalizedName = "USER" }
            //  );
        }
    }
}
