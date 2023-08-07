using DataAccsessLayer.Abstract;
using DataAccsessLayer.Concrete;
using DataAccsessLayer.Repositories;
using EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DataAccsessLayer.EFrameworkCore
{

    public class EFCreditRepository:GenericRepository<Credits>, ICreditDAL
    {
        public List<Credits> ICredits()
        {
            using(var db=new AppDbContext())
            {
                return db.Credits.Include(x=>x.CreditDetail).Include(x=>x.MonthlyPayment).Include(x=>x.CustomerAccount).ThenInclude(x=>x.AppUser).Include(x => x.CustomerAccount).ThenInclude(x=>x.Valutas).ToList();
            }
           
        }

       
 

    }
}
