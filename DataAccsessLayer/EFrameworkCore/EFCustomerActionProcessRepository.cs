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
    public class EFCustomerActionProcessRepository : GenericRepository<CustomerActionProcess>, ICustomerActionProcessDAL
    {
        public async Task<List<CustomerActionProcess>> MyLastActionList(int id)
        {
            using (var context = new AppDbContext())
            {
                return context.CustomerActionProcess.Include(b => b.SendAction).ThenInclude(b => b.AppUser).Include(b => b.ReceiveAction).ThenInclude(x=>x.Valutas).Include(b => b.ReceiveAction).ThenInclude(x => x.AppUser).Where(x => x.SendAction.AppUser.Id == id).OrderByDescending(x=>x.ProcessDate).ToList();
            }

        }
        public List<CustomerActionProcess> MyCardListForOneCard(string valuta)
        {

            using (var context = new AppDbContext())
            {

                return context.CustomerActionProcess.Include(b => b.SendAction).ThenInclude(b => b.AppUser).Include(b => b.ReceiveAction).
                    ThenInclude(x => x.AppUser).Include(x=>x.ReceiveAction).ThenInclude(x=>x.Valutas).Where(x => x.SendAction.Valutas.Currency == valuta).OrderByDescending(x=>x.ProcessDate).ToList();
            }

        }

        public string SelectValuta(string currency)
        {
            switch (currency)
            {
                case "AZN": return "₼";
                case "TRL": return "₺";
                case "EUR": return "€";
                case "USD": return "$";
                default: return currency;
                    
            }
        }

    }
}






