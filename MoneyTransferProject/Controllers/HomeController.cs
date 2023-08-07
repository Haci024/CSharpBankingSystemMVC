using DataAccsessLayer.Concrete;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using MoneyTransferProject.Models;
using MoneyTransferProject.ViewModels;
using System.Diagnostics;

namespace MoneyTransferProject.Controllers
{
    public class HomeController : Controller
    {
    
        private readonly LanguageService _localizer;
        private readonly AppDbContext _db;

        public HomeController(LanguageService localizer, AppDbContext db)
        {
            _localizer = localizer;
            _db = db;
        }
        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM
            {
                AboutUs = _db.AboutUs.FirstOrDefault(),
                Feedbacks = _db.FeedBacks.ToList(),
                SLiders = _db.Slider.ToList(),
                SliderImage = _db.SliderImages.FirstOrDefault(),
                BankCards = _db.BankCards.ToList(),
                CardSkills = _db.CardSkills.ToList(),
                FrequentlyQuestions = _db.FrequentlyQuestions.ToList(),
                OurServices = _db.OurServices.ToList(),
                BlogNews = _db.BlogNews.ToList(),


            };
            return View(homeVM);
        }
        public IActionResult Error()
        {
            return View();
        }
        public IActionResult LoadAmount()
        {
            return View();
        }
        public IActionResult ChangeLanguage(string culture)
        {
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions() { Expires = DateTimeOffset.UtcNow.AddYears(1) });

            return Redirect(Request.Headers["Referer"].ToString());
        }
        public IActionResult VisaCard()
        {
            return View();
        }
   

    
    }
}