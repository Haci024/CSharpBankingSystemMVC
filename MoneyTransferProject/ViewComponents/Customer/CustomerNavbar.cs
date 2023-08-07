using Microsoft.AspNetCore.Mvc;

namespace MoneyTransferProject.ViewComponents.Customer
{
    public class CustomerNavbar:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();  
        }
    }
}
