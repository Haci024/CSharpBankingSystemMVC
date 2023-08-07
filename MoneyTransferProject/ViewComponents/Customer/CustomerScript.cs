using Microsoft.AspNetCore.Mvc;

namespace MoneyTransferProject.ViewComponents.Customer
{
    public class CustomerScript:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
