using DataAccsessLayer.Abstract;
using DataAccsessLayer.Concrete;
using DataAccsessLayer.Repositories;
using EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DataAccsessLayer.EFrameworkCore
{

    public class EFCustomerAccountRepository:GenericRepository<CustomerAccount>, ICustomerAccountDAL
    {

        public List<CustomerAccount> MyCardsList(int id)
        {
    
            using (var context = new AppDbContext())
            {   

                return context.CustomerAccount.Include(x=>x.AppUser).Include(x=>x.Valutas).Where(x=>x.AppUserId==id).ToList();
            }
        } 
        public CustomerAccount Sender(int id,string valuta)
        {

            using (var context = new AppDbContext())
            {

                return context.CustomerAccount.Include(x => x.AppUser).Include(x=>x.Valutas).Where(x => x.Valutas.Currency == valuta).FirstOrDefault(x => x.AppUserId == id);
            }

        }
        public CustomerAccount Receiver(int ReceiverId)
        {
            using (var context = new AppDbContext())
            {
                return context.CustomerAccount.Include(x => x.AppUser).Include(x=>x.Valutas).FirstOrDefault(x => x.Id == ReceiverId);
            }

         
        }
 

    }
}
