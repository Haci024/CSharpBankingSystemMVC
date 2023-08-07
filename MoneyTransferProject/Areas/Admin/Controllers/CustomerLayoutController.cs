using Microsoft.AspNetCore.Mvc;

namespace MoneyTransferProject.Areas.Admin.Controllers
{
    public class CustomerLayoutController : Controller
    {
        public IActionResult Index()
        {

            return View();
        }
    }
}
