using DataAccsessLayer.Abstract;
using DataAccsessLayer.Concrete;
using DataAccsessLayer.Repositories;
using EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccsessLayer.EFrameworkCore
{
    public class EFMonthlyPaymentRepository : GenericRepository<MonthlyPayment>, IMonthlyPaymentDAL
    {
        
    
        public MonthlyPayment PayCredit(int id)
        {
            using (var db=new AppDbContext())
            {
                return  db.MonthlyPayment.Include(x => x.Credit).ThenInclude(x => x.CreditDetail).Include(x => x.Credit).
                             ThenInclude(x => x.CustomerAccount).ThenInclude(x => x.Valutas).Include(x => x.Credit).
                             ThenInclude(x => x.CustomerAccount).ThenInclude(x => x.AppUser).Where(x => x.Id == id).FirstOrDefault();
            }
           
        }
    }
}
