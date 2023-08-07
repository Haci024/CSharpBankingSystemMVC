using BussinessLayer.Abstract;
using DataAccsessLayer.Concrete;
using DTOLayer.DTOS.CustomerAccountDTOS;
using EntityLayer.Concrete;
using Infobip.Api.Client.Api;
using Infobip.Api.Client.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using MoneyTransferProject.ViewModels;
using NuGet.Protocol.Plugins;
using Org.BouncyCastle.Crypto.Generators;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using Twilio;
using Twilio.AspNet.Common;
using Twilio.AspNet.Mvc;
using Twilio.Rest.Api.V2010.Account;
using Twilio.TwiML;

namespace MoneyTransferProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CardProcessController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ICustomerAccountService _ICustomerAccount;
        private readonly IEmailService _IEmailService;
        private readonly ISMSService _ISMSService;
        private readonly AppDbContext _db;

        public CardProcessController(UserManager<AppUser> userManager, AppDbContext appDbContext, ISMSService SMSService, IEmailService EmailService, SignInManager<AppUser> signInManager, ICustomerAccountService customerAccountService)
        {
            _userManager = userManager;
            _ICustomerAccount = customerAccountService;
            _signInManager = signInManager;
            _db = appDbContext;
            _ISMSService = SMSService;
            _IEmailService = EmailService;
        }
        #region OrderNewCard
        [HttpGet]
        public async Task<IActionResult> BuyNewCards()
        {

            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);

            ViewBag.Valuta = _db.Valuta.Include(x => x.CustomerAccounts).ToList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BuyNewCards(NewCardDTO newCardDTO)
        {

            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);

            ViewBag.Valuta = _db.Valuta.Include(x => x.CustomerAccounts).ToList();
            int ValutaID = await _db.Valuta.Where(x => x.Currency == newCardDTO.Currency).Select(x => x.Id).FirstOrDefaultAsync();
            CustomerAccount newCard = new CustomerAccount();
            newCard.AppUserId = user.Id;
            newCard.BankBranch = "Bakı";
            newCard.CCV = newCardDTO.CCV;
            newCardDTO.ExploryMonth = newCard.ExploryMonth;
            newCardDTO.ExploryYear=newCard.ExploryYear;
            newCard.AccountNumber = newCardDTO.AccountNumber;
            newCard.Balance = 0;
           // newCard.ExploryDate = newCardDTO.ExploryDate;
            newCard.ValutaID = ValutaID;




            _ICustomerAccount.TCreate(newCard);
            return RedirectToAction("PhoneNumberForSMS", "CardProcess");
        }
        #endregion
        #region ActivateCodeWithPhoneNumber
        [HttpGet]
        public async Task<IActionResult> PhoneNumberForSMS()
        {
            AppUser appUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (appUser == null)
            {
                return View("Error");

            }

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PhoneNumberForSMS(CardInfoVerify cardInfoVerify)
        {


            Random random = new Random();
            int activateCode = random.Next(100000, 999999);
            AppUser appUser = await _userManager.FindByNameAsync(User.Identity.Name);
            CustomerAccount customerAccount = await _db.CustomerAccount.Where(x => x.Active == false).FirstOrDefaultAsync();
            customerAccount.ActivateCode = activateCode;


            if (appUser == null)
            {
                return View("Error");
            }
            if (cardInfoVerify.PhoneNumber == null)
            {
                ModelState.AddModelError("", "Zəhmət olmasa telefon nömrəsini daxil edin!");
                return View();
            }
            if (appUser.PhoneNumber != cardInfoVerify.PhoneNumber)
            {
                ModelState.AddModelError("", "Zəhmət olmasa qeydiyyat zamanı istifadə etdiyiniz telefon nömrəsini  daxil edin!");
                return View();
            }
            _ICustomerAccount.TUpdate(customerAccount);
            _ISMSService.SendVerifySMS(activateCode, cardInfoVerify.PhoneNumber);
            return RedirectToAction("VerifySMSCode", "CardProcess", new { cardInfoVerify.PhoneNumber });
        }
        [HttpGet]
        public async Task<IActionResult> VerifySMSCode(string PhoneNumber)
        {
            if (PhoneNumber == null)
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
        public async Task<IActionResult> VerifySMSCode(CardInfoVerify cardInfoVerify)
        {

            AppUser appUser = await _userManager.FindByNameAsync(User.Identity.Name);

            if (appUser == null)
            {
                return View("Error");
            }
            CustomerAccount customerAccount = await _db.CustomerAccount.Include(x => x.AppUser).Include(x => x.Valutas).Where(x => x.Active == false).FirstOrDefaultAsync();



            if (cardInfoVerify.PhoneNumber != appUser.PhoneNumber)
            {
                ModelState.AddModelError("", "Qeydiyyatdan keçdiyiniz telefon nömrəsini daxil edin!");
                return View();
            }

            if (cardInfoVerify.ActivateCode == null)
            {
                ModelState.AddModelError("", "Aktivləşdirmə kodunu daxil edin!");

                return View();
            }
            if (customerAccount.ActivateCode != cardInfoVerify.ActivateCode)
            {
                ModelState.AddModelError("", "Aktivləşdirmə kodu yalnışdır!.Zəhmət olmasa göndərilən şifrəni diqqətlə nəzərdən keçirin");

                customerAccount.ActivateAttempts++;
                _ICustomerAccount.TUpdate(customerAccount);
                return View();
            }
            if (customerAccount.ActivateAttempts >= 3)
            {
                _ICustomerAccount.TDelete(customerAccount);
                return RedirectToAction();
            }
            if (customerAccount.Active == true)
            {
                ModelState.AddModelError("", "Bu kart artıq aktivləşdirilib");
                return View();
            }
            customerAccount.Active = true;
            _ICustomerAccount.TUpdate(customerAccount);
            _IEmailService.MessageForNewCustomer(customerAccount);
            return RedirectToAction("AllCards", "MyAccount");
        }
        #endregion
        #region ActivateCodeWithEmail
        [HttpGet]
        public async Task<IActionResult> EmailForVerify()
        {

            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EmailForVerify(CardInfoVerify cardInfoVerify)
        {
            Random random = new Random();
            int activateCode = random.Next(100000, 999999);
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);


            //string accountSid = Environment.GetEnvironmentVariable("61cb89f4d9ba668aaca5cc423a5076af-9b56adc7-8384-4e7a-8f1f-6c69dc16582a");
            //string authToken = Environment.GetEnvironmentVariable("r5djxe.api.infobip.com");

            //TwilioClient.Init(accountSid, authToken);

            //var message = MessageResource.Create(
            //    body: "Təsdiq kodunuz:"+activateCode,
            //    from: new Twilio.Types.PhoneNumber("+15017122661"),
            //    to: new Twilio.Types.PhoneNumber(user.PhoneNumber)
            //);
            CustomerAccount customerAccount = await _db.CustomerAccount.Include(x => x.AppUser).Include(x => x.Valutas).Where(x => x.Active == false).FirstOrDefaultAsync();
            customerAccount.ActivateCode = activateCode;
            cardInfoVerify.Email = user.Email;
            if (cardInfoVerify.Email == null)
            {
                ModelState.AddModelError("", "Zəhmət olmasa email ünvanınızı daxil edin!");
                return View();
            }
            if (cardInfoVerify.Email != user.Email)
            {
                ModelState.AddModelError("", "Zəhmət olmasa qeydiyyatdan keçdiyiniz elektron ünvandan istifadə edin!");
                return View();
            }

            _ICustomerAccount.TUpdate(customerAccount);
            _IEmailService.SendActivateCode(cardInfoVerify, activateCode);

            return RedirectToAction("VerifyEmailCode", "CardProcess", new { email = user.Email });
        }

        [HttpGet]
        public async Task<IActionResult> VerifyEmailCode(string email)
        {
            if (email == null)
            {
                return View("Error");
            }
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyEmailCode(CardInfoVerify cardInfoVerify, string email)
        {
            if (email == null)
            {
                return View("Error");
            }
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            CustomerAccount customerAccount = await _db.CustomerAccount.Include(x => x.Valutas).Include(x => x.AppUser).Where(x => x.Active == false).FirstOrDefaultAsync();
            if (customerAccount.ActivateAttempts >= 3)
            {
                _ICustomerAccount.TDelete(customerAccount);

                return RedirectToAction("AllCards", "MyAccount");
            }
            if (customerAccount.ActivateCode != cardInfoVerify.ActivateCode)
            {
                ModelState.AddModelError("", "Aktivləşdirmə kodu yalnışdır!.Zəhmət olmasa göndərilən şifrəni diqqətlə nəzərdən keçirin");
                customerAccount.ActivateAttempts++;
                _ICustomerAccount.TUpdate(customerAccount);

                return View();
            }

            customerAccount.Active = true;
            _ICustomerAccount.TUpdate(customerAccount);
            _IEmailService.MessageForNewCustomer(customerAccount);

            return RedirectToAction("AllCards", "MyAccount");

        }

        #endregion
    }

}


