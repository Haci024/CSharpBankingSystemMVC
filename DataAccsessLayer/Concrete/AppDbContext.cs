using DataAccsessLayer.Concrete;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Twilio.Jwt;

namespace DataAccsessLayer.Concrete
{
    public class AppDbContext:IdentityDbContext<AppUser,AppRole,int>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=Odissey;initial catalog=MoneyTransferProject;integrated Security=true");
        }
      public DbSet<CustomerAccount> CustomerAccount { get; set;}
      public DbSet<CustomerActionProcess> CustomerActionProcess { get; set; }
      public DbSet<Valuta> Valuta { get; set; }
        public DbSet<Credits> Credits { get; set; }
        public DbSet<CreditDetail>CreditDetails { get; set; }
        public DbSet<MonthlyPayment> MonthlyPayment { get; set; }
        public DbSet<Kassa> Kassa { get; set; }
        public DbSet<AboutUs> AboutUs { get; set; }
        public DbSet<ContactUs> ContactUs { get; set; }
        public DbSet<Feedbacks> FeedBacks { get; set; }
        public DbSet<BankCards> BankCards { get; set; }
        public DbSet<CardSkills> CardSkills { get; set; }
        public DbSet<OurServices> OurServices { get; set; }
        public DbSet<BlogNews> BlogNews { get; set; }
        public DbSet<FrequentlyQuestions> FrequentlyQuestions { get; set; }
        public DbSet<Slider> Slider { get; set; }
        public DbSet<SliderImage> SliderImages { get; set; }
        //public DbSet<Employee> Employers { get; set; }
        //public DbSet<Position> Positions { get; set; }
        //public DbSet<EmployeePosition> EmployeePositions { get; set; }
        //public DbSet<HistoryEmployee> HistoryEmployees { get; set; }
        //public DbSet<History> Histories { get; set; }
        //public DbSet<Income> Incomes { get; set; }
        //public DbSet<Expenditure> Expenditures { get; set; }
        //public DbSet<Kassa> Kassas { get; set; }
        //public DbSet<PaidedSalary> PaidedSalaries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MonthlyPayment>()
                .HasOne(x=>x.Credit).
                WithMany(x=>x.MonthlyPayment).HasForeignKey(x=>x.CreditID);

            modelBuilder.Entity<CustomerAccount>()
           .HasOne(c => c.Valutas)
           .WithMany(v => v.CustomerAccounts)
           .HasForeignKey(c => c.ValutaID)
           .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Credits>()
          .HasOne(c => c.CustomerAccount)
          .WithMany(v => v.Credits)
          .HasForeignKey(c => c.CustomerAccountID)
          .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CustomerActionProcess>()
                .HasOne(x => x.SendAction).
                WithMany(y => y.Sender).HasForeignKey(x => x.SenderID).
                OnDelete(DeleteBehavior.ClientSetNull);


			modelBuilder.Entity<CustomerActionProcess>()
			   .HasOne(x => x.ReceiveAction).
			   WithMany(y => y.Receiver).
               HasForeignKey(x => x.ReceiverID).
			   OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Credits>()
           .HasOne(k => k.CreditDetail)
           .WithOne(kd => kd.Credits)
           .HasForeignKey<CreditDetail>(kd => kd.CreditID);


            base.OnModelCreating(modelBuilder);
		}

    }
    

}

