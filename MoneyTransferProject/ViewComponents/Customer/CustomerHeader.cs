using Microsoft.AspNetCore.Mvc;


namespace MoneyTransferProject.ViewComponents.Customer
{
    public class CustomerHeader : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
