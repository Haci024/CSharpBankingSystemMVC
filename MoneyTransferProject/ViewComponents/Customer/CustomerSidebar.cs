using DataAccsessLayer.Concrete;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MoneyTransferProject.ViewComponents.Customer
{
    public class CustomerSidebar:ViewComponent
    {
        private readonly AppDbContext _db;
        public CustomerSidebar(AppDbContext db)
        {
            _db =db;
        }
        public  IViewComponentResult Invoke()
        {
            List<CustomerAccount> customerAccounts= _db.CustomerAccount.Include(x=>x.Valutas).Include(x=>x.AppUser).ToList();
           
            return View(customerAccounts);
        }

    }
}
