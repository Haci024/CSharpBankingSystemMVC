using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using MoneyTransferProject.ViewModels;
using System.Linq;
using System.Net.Http.Headers;
namespace MoneyTransferProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ValutaAndMetalController : Controller
    {
        //public async Task<IActionResult> Valuta()
        //{
        //    var client = new HttpClient();
        //    var request = new HttpRequestMessage
        //    {
        //        Method = HttpMethod.Get,
        //        RequestUri = new Uri($"https://currency-exchange.p.rapidapi.com/exchange?from=USD&to=AZN&q=1.0"),
        //        Headers =
        //{
        //    { "X-RapidAPI-Key", "9cfffd4c72msh0e1b9963ab40a16p124373jsn6eebdc3a9122" },
        //    { "X-RapidAPI-Host", "currency-exchange.p.rapidapi.com" },
        //},
        //    };

        //    using (var response = await client.SendAsync(request))
        //    {
        //        response.EnsureSuccessStatusCode();
        //        var body = await response.Content.ReadAsStringAsync();
        //        ViewBag.ConvertedAmount = body;

        //    }

        //    return View();
        //}
        //[HttpPost]
        //public async Task<IActionResult> Valuta(string fromCurrency, string toCurrency, decimal amount)
        //{
        //    var client = new HttpClient();
        //    var request = new HttpRequestMessage
        //    {
        //        Method = HttpMethod.Get,
        //        RequestUri = new Uri($"https://currency-exchange.p.rapidapi.com/exchange?from={fromCurrency}&to={toCurrency}&q={amount}"),
        //        Headers =
        //{
        //    { "X-RapidAPI-Key", "9cfffd4c72msh0e1b9963ab40a16p124373jsn6eebdc3a9122" },
        //    { "X-RapidAPI-Host", "currency-exchange.p.rapidapi.com" },
        //},
        //    };

        //    using (var response = await client.SendAsync(request))
        //    {
        //        response.EnsureSuccessStatusCode();
        //        var body = await response.Content.ReadAsStringAsync();

        //        ViewBag.FromCurrency = fromCurrency;
        //        ViewBag.ToCurrency = toCurrency;
        //        ViewBag.Amount = amount;
        //        ViewBag.ConvertedAmount = body;
        //    }

        //    return View();
        //}

        [HttpGet]
        public IActionResult Valuta(ConvertMoneyViewModel model)
        {
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Valuta(ConvertMoneyViewModel model, int id)
        {

            var client = new HttpClient();
            var request = new HttpRequestMessage

            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://currency-exchange.p.rapidapi.com/exchange?from={model.FromCurrency}&to={model.ToCurrency}&q={model.Amount}"),
                Headers =
            {
                { "X-RapidAPI-Key", "9cfffd4c72msh0e1b9963ab40a16p124373jsn6eebdc3a9122" },
                { "X-RapidAPI-Host", "currency-exchange.p.rapidapi.com" },
            },
            };

            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                model.ConvertedAmount = double.Parse(body);
            }




            return View(model);

        }


    }




}
