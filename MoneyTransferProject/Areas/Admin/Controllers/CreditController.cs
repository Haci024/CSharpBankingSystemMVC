using BussinessLayer.Abstract;
using DataAccsessLayer.Concrete;
using DTOLayer.DTOS.AppUserDTOS;
using DTOLayer.DTOS.CreditDTOS;
using DTOLayer.DTOS.CustomerAccountDTOS;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using MoneyTransferProject.Extension;
using Twilio.Rest.Trunking.V1;

namespace MoneyTransferProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CreditController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly ICreditService _ICreditService;
        private readonly ICreditDetailService _ICreditDetailService;
        private readonly ISMSService _IsmsService;
        private readonly IEmailService _IemailService;
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        private readonly ICustomerActionProcessService _ICustomerActionProcessService;
        private readonly ICustomerAccountService _ICustomerAccountService;
        private readonly IMonthlyPaymentService _IMonthlyPaymentService;
        private readonly IKassaService _IKassaService;
        public CreditController(UserManager<AppUser> userManager, IKassaService kassaService, IMonthlyPaymentService monthlyPaymentService, ICustomerActionProcessService CustomerActionProcessService, IWebHostEnvironment env, ICustomerAccountService CustomerAccountService, ICreditDetailService ICreditDetailService, RoleManager<AppRole> roleManager, AppDbContext db, ICreditService creditService, IEmailService emailService, ISMSService ISmsService)
        {
            _ICreditService = creditService;
            _roleManager = roleManager;
            _userManager = userManager;
            _IemailService = emailService;
            _ICreditService = creditService;
            _ICreditDetailService = ICreditDetailService;
            _ICustomerAccountService = CustomerAccountService;
            _ICustomerActionProcessService = CustomerActionProcessService;
            _env = env;
            _db = db;
            _IKassaService = kassaService;
            _IMonthlyPaymentService = monthlyPaymentService;
        }
        [HttpGet]
        public async Task<IActionResult> GetCredit()
        {
            AppUser appUser = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.CustomerAccount = _db.CustomerAccount.Include(x => x.AppUser).Include(x => x.Valutas).Include(x => x.Credits).Where(x => x.AppUser.UserName == User.Identity.Name).ToList();

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> GetCredit(GetNewCreditDTO newCreditDTO)
        {

            AppUser appUser = await _userManager.FindByNameAsync(User.Identity.Name);
            int ReceiverID = _db.CustomerAccount.Include(x => x.AppUser).Where(x => x.AppUser.UserName == "Odissey").Select(x => x.Id).FirstOrDefault();
            ViewBag.CustomerAccount = _db.CustomerAccount.Include(x => x.AppUser).Include(x => x.Valutas).Include(x => x.Credits).Where(x => x.AppUser.UserName == User.Identity.Name).ToList();
            Credits newcredits = new Credits();
            int CustomerAccountID = _db.CustomerAccount.Include(x => x.Valutas).Where(x => x.AccountNumber == newCreditDTO.AccountNumber).Select(x => x.Id).FirstOrDefault();
            newcredits.Percent = newCreditDTO.Percent;
            newcredits.Period = newCreditDTO.Period;
            newcredits.CreditAmount = newCreditDTO.CreditAmount;
            newcredits.StartTime = DateTime.Now;
            newcredits.EndTime = DateTime.Now.AddMonths((int)newcredits.Period);
            newcredits.CustomerAccountID = CustomerAccountID;
            newcredits.IsActive = false;
            newcredits.Document = "sss";
            decimal monthlyInterestRate = (decimal)newCreditDTO.Percent / 100 / 12;
            CreditDetail creditDetail = new CreditDetail();
            CustomerActionProcess customerActionProcess = new CustomerActionProcess();
            customerActionProcess.fees = 0;
            customerActionProcess.SenderID = CustomerAccountID;
            customerActionProcess.ReceiverID = ReceiverID;
            customerActionProcess.Amount = (decimal)newcredits.CreditAmount;
            customerActionProcess.ProcessDate = DateTime.Now;
            customerActionProcess.ProcessType = "Kredit götür";


            creditDetail.PaymentDate = DateTime.Now.AddMonths(1);


            creditDetail.MonthlyPayment = Math.Round((double)(newCreditDTO.CreditAmount * monthlyInterestRate *
                    (decimal)Math.Pow((double)(1 + monthlyInterestRate), newCreditDTO.Period) /
                    ((decimal)Math.Pow((double)(1 + monthlyInterestRate), newCreditDTO.Period) - 1)), 2);
            newcredits.TotalPayment = Math.Round(creditDetail.MonthlyPayment * newcredits.Period, 2);
            creditDetail.RemainingPayment = (decimal)newcredits.TotalPayment;
            _ICreditService.TCreate(newcredits);
            creditDetail.CreditID = newcredits.Id;
            for (int month = 1; month <= newcredits.Period; month++)
            {
                MonthlyPayment monthlyPayment = new MonthlyPayment
                {
                    Payment = (decimal)creditDetail.MonthlyPayment,
                    PaymentDate = DateTime.Now.AddMonths(month),
                    Document = "sss",
                    CreditID = newcredits.Id
                };
                _db.MonthlyPayment.Add(monthlyPayment);
            }
            _db.SaveChanges();
            _ICreditDetailService.TCreate(creditDetail);
            _ICustomerActionProcessService.TCreate(customerActionProcess);
            return RedirectToAction("AgreementAndFax", "Credit", new { id = newcredits.Id });
        }
        [HttpGet]
        public async Task<IActionResult> AgreementAndFax(int id)
        {
            if (id == null)
            {
                return View("Error");
            }
            AppUser appUser = await _userManager.FindByNameAsync(User.Identity.Name);
            Credits credits = await _db.Credits.Include(x => x.CreditDetail).Include(x => x.MonthlyPayment).Include(x => x.CustomerAccount).
       ThenInclude(x => x.AppUser).Include(x => x.CustomerAccount).ThenInclude(x => x.Valutas).Where(x => x.Id == id).FirstOrDefaultAsync();

            if (credits == null)
            {
                return View("Error");
            }

            return View(credits);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AgreementAndFax(int id, GetNewCreditDTO getNewCreditDTO)
        {
            if (id == null)
            {
                return View("Error");
            }
            AppUser appUser = await _userManager.FindByNameAsync(User.Identity.Name);
            Credits credits = await _db.Credits.Include(x => x.CreditDetail).Include(x => x.MonthlyPayment).Include(x => x.CustomerAccount).
                ThenInclude(x => x.AppUser).Include(x => x.CustomerAccount).ThenInclude(x => x.Valutas).Where(x => x.Id == id).FirstOrDefaultAsync();
            CustomerAccount customerAccount = await _db.CustomerAccount.Where(x => x.Id == credits.CustomerAccountID).FirstOrDefaultAsync();



            if (credits == null)
            {
                return View("Error");
            }
            credits.IsActive = true;
            _ICreditService.TUpdate(credits);
            _ICustomerAccountService.TUpdate(customerAccount);

            return RedirectToAction("UploadFax", "Credit", new { id = credits.Id });
        }
        [HttpGet]
        public async Task<IActionResult> UploadFax(int id)
        {
            if (id == null)
            {
                return View("Error");
            }
            AppUser appUser = await _userManager.FindByNameAsync(User.Identity.Name);

            if (appUser == null)
            {
                return View("Error");
            }

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadFax(int id, Credits newCredits)
        {

            if (id == null)
            {
                return View("Error");
            }

            AppUser appUser = await _userManager.FindByNameAsync(User.Identity.Name);
            int SenderID = await _db.CustomerAccount.Where(x => x.AppUser.UserName == "Odissey").Select(x => x.Id).FirstOrDefaultAsync();
            Credits credits = await _db.Credits.Include(x => x.CreditDetail).Include(x => x.MonthlyPayment).Include(x => x.CustomerAccount).
            ThenInclude(x => x.AppUser).Include(x => x.CustomerAccount).ThenInclude(x => x.Valutas).Where(x => x.Id == id).FirstOrDefaultAsync();
            CustomerAccount customerAccount = await _db.CustomerAccount.Where(x => x.Id == credits.CustomerAccountID).FirstOrDefaultAsync();
            CustomerActionProcess customerActionProcess = new CustomerActionProcess();
            customerActionProcess.ProcessType = "Kredit al";
            customerActionProcess.Amount = (decimal)credits.CreditAmount;
            customerActionProcess.ProcessDate = credits.StartTime;
            customerActionProcess.fees = 0;
            customerActionProcess.ProcessDate = DateTime.Now;
            customerActionProcess.ReceiverID = credits.CustomerAccountID;
            customerActionProcess.SenderID = SenderID;

            if (credits.Fax != null)
            {
                if (!newCredits.Fax.IsPDF())
                {
                    ModelState.AddModelError("", "PDF  faylı seçin!");

                    return View();
                }
                if (newCredits.Fax.IsMore5MB())
                {
                    ModelState.AddModelError("", "Şəkil maksimum 5mb ölçüyə sahib ola bilər!");

                    return View();
                }
            }
            string path = Path.Combine(_env.WebRootPath, "Documents");
            credits.Document = await newCredits.Fax.SaveFileAsync(path);
            if (credits == null)
            {
                return View("Error");
            }
            if (credits.IsActive == false)
            {
                _ICreditService.TDelete(credits);
                _IemailService.SendFailCreditEmail(credits);

                return RedirectToAction("MyLastProcess", "MoneyTransfer");

            }
            customerAccount.Balance = customerAccount.Balance + (decimal)credits.CreditAmount;
            _ICreditService.TUpdate(credits);
            _IemailService.SendCreditEmail(credits);
            _ICustomerAccountService.TUpdate(customerAccount);
            _ICustomerActionProcessService.TCreate(customerActionProcess);

            return RedirectToAction("MyLastProcess", "MoneyTransfer");
        }
        [HttpGet]
        public IActionResult MyCredits()
        {
            List<Credits> credits = _ICreditService.ICredits();
            return View(credits);
        }

        public IActionResult Document(int id)
        {
            Credits credits = _db.Credits.Where(x => x.Id == id).FirstOrDefault();
            var pdfPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Documents", credits.Document);

            return PhysicalFile(pdfPath, "application/pdf", credits.Document);
        }

        //public IActionResult DocumentForMonthlyFax(int id)
        //{
        //    MonthlyPayment monthlyPayment = _db.MonthlyPayment.Where(x => x.Id == id).FirstOrDefault();
        //    var pdfPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Documents", "Monthly", monthlyPayment.Document);

        //    return PhysicalFile(pdfPath, "application/pdf", monthlyPayment.Document);
        //}
        [HttpGet]
        public IActionResult MonthlyPayment(int id)
        {

            List<MonthlyPayment> monthlyPayments = _db.MonthlyPayment.Include(x => x.Credit).ThenInclude(x => x.CreditDetail).Include(x => x.Credit).
                   ThenInclude(x => x.CustomerAccount).ThenInclude(x => x.Valutas).Include(x => x.Credit).
                   ThenInclude(x => x.CustomerAccount).ThenInclude(x => x.AppUser).Where(x => x.CreditID == id).ToList();
            CreditDetail creditDetail = _db.CreditDetails.Where(x => x.CreditID == id).FirstOrDefault();
            ViewBag.RemainingMoney = creditDetail.RemainingPayment;
            if (monthlyPayments == null)
            {
                return View("Error");
            }

            return View(monthlyPayments);
        }
        [HttpGet]
        public async Task<IActionResult> PayCredit(int id)
        {

            AppUser appUser = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.CustomerAccount = _db.CustomerAccount.Include(x => x.AppUser).Include(x => x.Valutas).Include(x => x.Credits).Where(x => x.AppUser.UserName == User.Identity.Name).ToList();

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PayCredit(int id, PayCreditDTO payCreditDTO)
        {
            AppUser appUser = await _userManager.FindByNameAsync(User.Identity.Name);
            MonthlyPayment monthlyPayment = _IMonthlyPaymentService.GetMonthlyPayment(id);

            monthlyPayment.ProcessDate = DateTime.Now;
            monthlyPayment.PayMoney = payCreditDTO.Payment;
            ViewBag.CustomerAccount = await _db.CustomerAccount.Include(x => x.AppUser).Include(x => x.Valutas).Include(x => x.Credits).Where(x => x.AppUser.UserName == User.Identity.Name).ToListAsync();
            CustomerAccount customerAccount = await _db.CustomerAccount.Include(x => x.Valutas).Include(x => x.AppUser).Where(x => x.AccountNumber == payCreditDTO.SelectCard).FirstOrDefaultAsync();
            int SenderId = await _db.CustomerAccount.Where(x => x.AccountNumber == payCreditDTO.SelectCard).Select(x => x.Id).FirstOrDefaultAsync();
            customerAccount.AppUserId = appUser.Id;
            customerAccount.Balance = customerAccount.Balance - payCreditDTO.Payment;
            monthlyPayment.Document = "sss";

            Kassa kassa = _db.Kassa.FirstOrDefault();
            kassa.Balance = kassa.Balance + payCreditDTO.Payment;
            CustomerActionProcess customerActionProcess = new CustomerActionProcess();
            customerActionProcess.ReceiverID = null;
            customerActionProcess.SenderID = SenderId;
            customerActionProcess.ProcessDate = DateTime.Now;
            customerActionProcess.fees = 0;
            customerActionProcess.ProcessType = "Kredit ödənişi";
            customerActionProcess.Amount = monthlyPayment.PayMoney;
            Credits credits = await _db.Credits.Where(x => x.Id == monthlyPayment.CreditID).FirstOrDefaultAsync();


            CreditDetail creditDetail = await _db.CreditDetails.Where(x => x.CreditID == credits.Id).FirstOrDefaultAsync();

            creditDetail.RemainingPayment = creditDetail.RemainingPayment - payCreditDTO.Payment;
            monthlyPayment.CreditID = credits.Id;
            creditDetail.CreditID = credits.Id;
            if (monthlyPayment.PaymentDate > monthlyPayment.ProcessDate)
            {
                TimeSpan timeDifference = monthlyPayment.ProcessDate - monthlyPayment.PaymentDate;
                int monthsPassed = timeDifference.Days / 30;



                if (monthlyPayment.Payment > payCreditDTO.Payment)
                {
                    double delayRate = 0;
                    for (int i = 0; i < monthsPassed; i++)
                    {
                        delayRate += 20;
                    }
                    monthlyPayment.Payment = monthlyPayment.Payment + monthlyPayment.Payment * (decimal)delayRate / 100;
                    monthlyPayment.IsPay = true;
                    _IMonthlyPaymentService.TUpdate(monthlyPayment);
                    decimal missingPayment = monthlyPayment.Payment - payCreditDTO.Payment;
                    int remainingMonthss = _db.MonthlyPayment.Count(x => x.CreditID == credits.Id && !x.IsPay);


                    decimal additionalPaymentPerInstallment = missingPayment / remainingMonthss;


                    List<MonthlyPayment> remainingMonthlyPayment = await _db.MonthlyPayment.Include(x => x.Credit).ThenInclude(x => x.CustomerAccount)
                    .Where(x => x.CreditID == credits.Id && !x.IsPay)
                     .OrderBy(x => x.ProcessDate)
                    .ToListAsync();

                    foreach (var item in remainingMonthlyPayment)
                    {
                        item.Payment += additionalPaymentPerInstallment;
                        item.CreditID = credits.Id;
                        creditDetail.MonthlyPayment = (double)item.Payment;
                        _IMonthlyPaymentService.TUpdate(item);

                    }
                    _IKassaService.TUpdate(kassa);
                    _ICustomerAccountService.TUpdate(customerAccount);
                    _ICreditDetailService.TUpdate(creditDetail);
                    _ICreditService.TUpdate(credits);
                    _ICustomerActionProcessService.TCreate(customerActionProcess);
                    _IemailService.SendCompleteCreditMail(credits, customerActionProcess, monthlyPayment);



                    return RedirectToAction("MyCredits", "Credit");
                }
                if (payCreditDTO.Payment == 0)
                {
                    ModelState.AddModelError("", "Ödəniş 0 ola bilməz!");

                    return View();
                }
                if (creditDetail.RemainingPayment == 0)
                {
                    credits.IsCompleted = true;
                    monthlyPayment.IsPay = true;

                    _IKassaService.TUpdate(kassa);
                    _ICustomerAccountService.TUpdate(customerAccount);
                    _ICreditDetailService.TUpdate(creditDetail);
                    _IMonthlyPaymentService.TUpdate(monthlyPayment);
                    _ICustomerActionProcessService.TCreate(customerActionProcess);
                    _IemailService.SendCompleteCreditMail(credits, customerActionProcess, monthlyPayment);
                    return RedirectToAction("MyCredits", "Credit", credits.Id);
                }

                monthlyPayment.IsPay = true;
                _IMonthlyPaymentService.TUpdate(monthlyPayment);
                decimal AddingPayments = payCreditDTO.Payment - monthlyPayment.Payment;
                int remainingMonths = _db.MonthlyPayment.Count(x => x.CreditID == credits.Id && !x.IsPay);
                decimal additionalPaymentPerInstallmentss = AddingPayments / remainingMonths;


                List<MonthlyPayment> remainingMonthlyPaymentss = await _db.MonthlyPayment.Include(x => x.Credit).ThenInclude(x => x.CreditDetail)
                .Where(x => x.CreditID == credits.Id && !x.IsPay).ToListAsync();




                foreach (var item in remainingMonthlyPaymentss)
                {

                    item.Payment -= additionalPaymentPerInstallmentss;
                    item.CreditID = credits.Id;
                    creditDetail.MonthlyPayment = (double)item.Payment;
                    _IMonthlyPaymentService.TUpdate(item);

                }

                _ICreditService.TUpdate(credits);
                _ICreditDetailService.TUpdate(creditDetail);
                _ICustomerAccountService.TUpdate(customerAccount);
                _IKassaService.TUpdate(kassa);
                _ICustomerActionProcessService.TCreate(customerActionProcess);
                _IemailService.SendMonthlyPaymentCreditMail(credits, customerActionProcess, monthlyPayment);
                return RedirectToAction("MyCredits", "Credit");
            }
            if (monthlyPayment.Payment > payCreditDTO.Payment)
            {
                monthlyPayment.IsPay = true;
                _IMonthlyPaymentService.TUpdate(monthlyPayment);
                decimal missingPayment = monthlyPayment.Payment - payCreditDTO.Payment;
                int remainingMonths = _db.MonthlyPayment.Count(x => x.CreditID == credits.Id && !x.IsPay);


                decimal additionalPaymentPerInstallment = missingPayment / remainingMonths;


                List<MonthlyPayment> remainingMonthlyPayment = await _db.MonthlyPayment.Include(x => x.Credit).ThenInclude(x => x.CustomerAccount)
                .Where(x => x.CreditID == credits.Id && !x.IsPay)
                 .OrderBy(x => x.ProcessDate)
                .ToListAsync();

                foreach (var item in remainingMonthlyPayment)
                {
                    item.Payment += additionalPaymentPerInstallment;
                    item.CreditID = credits.Id;
                    creditDetail.MonthlyPayment = (double)item.Payment;
                    _IMonthlyPaymentService.TUpdate(item);

                }
                _IKassaService.TUpdate(kassa);
                _ICustomerAccountService.TUpdate(customerAccount);
                _ICreditDetailService.TUpdate(creditDetail);
                _ICreditService.TUpdate(credits);
                _ICustomerActionProcessService.TCreate(customerActionProcess);
                _IemailService.SendCompleteCreditMail(credits, customerActionProcess, monthlyPayment);



                return RedirectToAction("MyCredits", "Credit");
            }
            if (payCreditDTO.Payment == 0)
            {
                ModelState.AddModelError("", "Ödəniş 0 ola bilməz!");

                return View();
            }
            if (creditDetail.RemainingPayment == 0)
            {
                credits.IsCompleted = true;
                monthlyPayment.IsPay = true;

                _IKassaService.TUpdate(kassa);
                _ICustomerAccountService.TUpdate(customerAccount);
                _ICreditDetailService.TUpdate(creditDetail);
                _IMonthlyPaymentService.TUpdate(monthlyPayment);
                _ICustomerActionProcessService.TCreate(customerActionProcess);
                _IemailService.SendCompleteCreditMail(credits, customerActionProcess, monthlyPayment);
                return RedirectToAction("MyCredits", "Credit", credits.Id);
            }

            monthlyPayment.IsPay = true;
            _IMonthlyPaymentService.TUpdate(monthlyPayment);
            decimal AddingPayment = payCreditDTO.Payment - monthlyPayment.Payment;
            int remainingMonth = _db.MonthlyPayment.Count(x => x.CreditID == credits.Id && !x.IsPay);
            decimal additionalPaymentPerInstallments = AddingPayment / remainingMonth;


            List<MonthlyPayment> remainingMonthlyPayments = await _db.MonthlyPayment.Include(x => x.Credit).ThenInclude(x => x.CreditDetail)
            .Where(x => x.CreditID == credits.Id && !x.IsPay).ToListAsync();




            foreach (var item in remainingMonthlyPayments)
            {

                item.Payment -= additionalPaymentPerInstallments;
                item.CreditID = credits.Id;
                creditDetail.MonthlyPayment = (double)item.Payment;
                _IMonthlyPaymentService.TUpdate(item);

            }

            _ICreditService.TUpdate(credits);
            _ICreditDetailService.TUpdate(creditDetail);
            _ICustomerAccountService.TUpdate(customerAccount);
            _IKassaService.TUpdate(kassa);
            _ICustomerActionProcessService.TCreate(customerActionProcess);
            _IemailService.SendMonthlyPaymentCreditMail(credits, customerActionProcess, monthlyPayment);
            return RedirectToAction("MyCredits", "Credit");
        }
        [HttpGet]
        public async Task<IActionResult> MonthlyFax(int id)
        {
            ViewBag.Id = id;


            Credits Credits = await _db.Credits.
                Include(x => x.MonthlyPayment).
                Include(x => x.CreditDetail).
                Include(x => x.CustomerAccount).
                ThenInclude(x => x.AppUser).
                Include(x => x.CustomerAccount).
                ThenInclude(x => x.Valutas).
                FirstOrDefaultAsync();


            return View(Credits);
        }
    }
}
