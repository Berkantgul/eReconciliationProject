using Core.Entities;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework.Context
{
    public class ContextDb : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=LAPTOP-UROTH5AA;Database=eRecanciliationDb;Integrated Security=true");
        }
        public DbSet<AccountRecanciliation> AccountRecanciliations { get; set; }
        public DbSet<AccountRecanciliationDetail> AccountRecanciliationDetails { get; set; }
        public DbSet<BaBsRecanciliation> BaBsRecanciliations { get; set; }
        public DbSet<BaBsRecanciliationDetail> BaBsRecanciliationDetails { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<CurrencyAccount> CurrencyAccounts { get; set; }
        public DbSet<MailParameter> MailParameters { get; set; }
        public DbSet<OperationClaim> OperationClaims { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserCompany> UserCompanies { get; set; }
        public DbSet<UserOperationClaim> UserOperationClaims { get; set; }
        public DbSet<MailTemplate> MailTemplates { get; set; }
        public DbSet<TermsAndCondition> TermsAndConditions { get; set; }
        public DbSet<ForgotPassword> ForgotPasswords { get; set; }
        public DbSet<UserReletionShip> UserReletionShips { get; set; }
    }
}
