using Microsoft.AspNetCore.Mvc;

namespace MoneyTransferProject.ViewComponents.Customer
{
     
    public class CustomerFooter:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
