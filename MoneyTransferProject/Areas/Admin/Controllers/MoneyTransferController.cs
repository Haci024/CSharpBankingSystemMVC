using BussinessLayer.Abstract;
using DataAccsessLayer.Concrete;
using DTOLayer.DTOS.CustomerAccountProcessDTO;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using NuGet.Protocol;
using BussinessLayer.ValidationRules.AppUserValidation;
using DTOLayer.DTOS.AppUserDTOS;
using Microsoft.AspNetCore.Localization;
using System.ComponentModel.DataAnnotations;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NuGet.Protocol.Plugins;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Routing;
using NETCore.MailKit.Core;
using IEmailService = BussinessLayer.Abstract.IEmailService;
using MoneyTransferProject.ViewModels;
using Twilio.Rest.Trunking.V1;

namespace MoneyTransferProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MoneyTransferController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _db;
        private readonly ICustomerActionProcessService _customerActionProcessService;
        private readonly ICustomerAccountService _customerAccountService;
        private readonly IEmailService _emailService;
        private readonly IMoneyConvertService _moneyConvertService;


        public MoneyTransferController(UserManager<AppUser> userManager, IMoneyConvertService moneyConvertService, IEmailService emailService, AppDbContext db, ICustomerAccountService customerAccountService, ICustomerActionProcessService customerActionProcessService)
        {
            _userManager = userManager;
            _db = db;
            _customerActionProcessService = customerActionProcessService;
            _customerAccountService = customerAccountService;
            _emailService = emailService;
            _moneyConvertService = moneyConvertService;
        }


        [HttpGet]

        public async Task<IActionResult> SendMoney(string valuta)
        {
            ViewBag.Valuta = valuta;
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);


            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendMoney(SendMoneyProcessDTO sendMoneyProcessDTO, string valuta)
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.Valuta = valuta;

            CustomerAccount SenderAccount = _customerAccountService.TSender(user.Id, valuta);
            int receiverId = _db.CustomerAccount.Where(x => x.AccountNumber == sendMoneyProcessDTO.ReceiverCardNumber).Select(x => x.Id).FirstOrDefault();

            CustomerActionProcess customerAccountProcess = new CustomerActionProcess();
            //int SenderID = _db.CustomerAccount.Where(x=>x.AppUserId==user.Id).Where(x=>x.Currency==valuta).Select(x=>x.Id).FirstOrDefault();
            customerAccountProcess.SenderID = SenderAccount.Id;//SenderID;

            customerAccountProcess.ProcessType = "Kartdan-Karta";
            customerAccountProcess.Amount = sendMoneyProcessDTO.Amount;
            customerAccountProcess.ReceiverID = receiverId;
            decimal fess = customerAccountProcess.Amount * 1m / 100;
            customerAccountProcess.Amount = customerAccountProcess.Amount - fess;
            customerAccountProcess.fees = fess;
            customerAccountProcess.ProcessDate = DateTime.Now;

            CustomerAccount ReceiverAccount = _customerAccountService.TReceiver(receiverId);

            string ReceiverCurrency = _db.CustomerAccount.Where(x => x.AccountNumber == sendMoneyProcessDTO.ReceiverCardNumber).Select(x => x.Valutas.Currency).FirstOrDefault();

            if (ReceiverAccount == null)
            {
                ModelState.AddModelError("", "Belə bank kartı mövcud deyil!");

                return View();
            }
            if (SenderAccount.Balance <= sendMoneyProcessDTO.Amount)
            {
                ModelState.AddModelError("", "Kartınızda olan məbləğ göndərmək istədiyiniz məbləğdən azdır!");

                return View();
            }
            if (SenderAccount.AccountNumber == sendMoneyProcessDTO.ReceiverCardNumber)
            {
                ModelState.AddModelError("", "Öz kart hesabınıza yalnız başqa valyuta və başqa kart nömrəsilə  pul göndərə bilərsiniz!");

                return View();
            }
            if (sendMoneyProcessDTO.ReceiverCardNumber == null)
            {
                ModelState.AddModelError("", "Pul göndərmək istədiyiniz şəxsin hesab nömrəsini daxil edin");

                return View();
            }
            if (sendMoneyProcessDTO.Amount == null)
            {
                ModelState.AddModelError("", "Məbləği daxil edin!");

                return View();
            }

            _moneyConvertService.ConvertMoney(valuta, SenderAccount, ReceiverAccount, sendMoneyProcessDTO, ReceiverCurrency, fess);
            _customerAccountService.TUpdate(SenderAccount);
            _customerAccountService.TUpdate(ReceiverAccount);
            _customerActionProcessService.TCreate(customerAccountProcess);
            _emailService.SendMessageForSender(customerAccountProcess, SenderAccount, user);
            _emailService.SendMessageForReceiver(customerAccountProcess, ReceiverAccount.Valutas.Currency, ReceiverAccount, user, fess, valuta, sendMoneyProcessDTO);
            decimal value = _moneyConvertService.ConvertorForReceiverMail(ReceiverAccount, valuta, customerAccountProcess);
            //ViewBag.ConvertForReceiver = value;
            return RedirectToAction("MyLastProcessListForOneCard", "MoneyTransfer", new { valuta });
        }
        /*Pul göndərən tərəfin kim olduğunu tapmaq üçün İstifadəçiHesabı(CustomerAccount)-dan İstifadəçi ID si
                    AppUser modelindəki istifadəçi İD-sinə bərabər olan istifadəçinin hesablarını gətirir və tək səfərdə
                    ancaq 1 hesabdan pul göndərmək mümkün olmasını nəzərə alaraq (Currency) yəni valyuta dəyəri AZN olan kartdan,Id nömrəsini select edir.*/

        [HttpGet]

        public async Task<IActionResult> MyLastProcess()
        {

            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            List<CustomerActionProcess> customerActionProcess = await _customerActionProcessService.TMyLastActionList(user.Id);

            return View(customerActionProcess);
        }

        [HttpGet]

        public async Task<IActionResult> MyLastProcessListForOneCard(string valuta)
        {
            if (valuta == null)
            {
                return NotFound();
            }
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.Valuta = valuta;
            CustomerAccount customerAccount = await _db.CustomerAccount.Include(x => x.AppUser).Include(x => x.Valutas).Where(x => x.Valutas.Currency == valuta).FirstOrDefaultAsync();

            List<CustomerActionProcess> customerActionProcess = _customerActionProcessService.TMyCardListForOneCard(customerAccount.Valutas.Currency);

            return View(customerActionProcess);
        }
        [HttpGet]
        public async Task<IActionResult> SendMoneyWithAnyCards()
        {

            AppUser appUser = await _userManager.FindByNameAsync(User.Identity.Name);

            List<CustomerAccount> SenderAccount = _db.CustomerAccount.Include(x => x.Receiver).Include(x => x.Sender).Include(x => x.AppUser).Where(x => x.AppUserId == appUser.Id).ToList();
            ViewBag.SenderAccount = _db.CustomerAccount.Include(x => x.Receiver).Include(x => x.Valutas).Include(x => x.Sender).Include(x => x.AppUser).Where(x => x.AppUserId == appUser.Id).ToList();

            ViewBag.SenderAccount = SenderAccount;


            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendMoneyWithAnyCards(SendMoneyProcessDTO sendMoneyProcessDTO)
        {
            AppUser appUser = await _userManager.FindByNameAsync(User.Identity.Name);

            ViewBag.SenderAccount = _db.CustomerAccount.Include(x => x.Receiver).Include(x => x.Valutas).Include(x => x.Sender).Include(x => x.AppUser).Where(x => x.AppUserId == appUser.Id).ToList();
            CustomerAccount OneSender = _db.CustomerAccount.Include(x => x.Receiver).Include(x => x.Valutas).Include(x => x.Sender).Include(x => x.AppUser).Where(x => x.AccountNumber == sendMoneyProcessDTO.SenderCard).FirstOrDefault(x => x.AppUserId == appUser.Id);

            int ReceiverId = _db.CustomerAccount.Where(x => x.AccountNumber == sendMoneyProcessDTO.ReceiverCardNumber).Select(x => x.Id).FirstOrDefault();

            CustomerAccount ReceiverAccount = _db.CustomerAccount.Include(x => x.AppUser).FirstOrDefault(x => x.Id == ReceiverId);
            string ReceiverCurrency = _db.CustomerAccount.Include(x => x.Valutas).Where(x => x.AccountNumber == sendMoneyProcessDTO.ReceiverCardNumber).Select(x => x.Valutas.Currency).FirstOrDefault();
            string SenderCurrency = _db.CustomerAccount.Include(x => x.Valutas).Where(x => x.Id == OneSender.Id).Select(x => x.Valutas.Currency).FirstOrDefault();
            CustomerActionProcess customerActionProcess = new CustomerActionProcess();

            customerActionProcess.ReceiverID = ReceiverAccount.Id;
            customerActionProcess.SenderID = OneSender.Id;
            customerActionProcess.ProcessType = "Kartdan-karta";
            customerActionProcess.Amount = sendMoneyProcessDTO.Amount;
            decimal fess = customerActionProcess.Amount * 1 / 100;
            customerActionProcess.fees = fess;
            customerActionProcess.ProcessDate = DateTime.Now;
            if (ReceiverAccount == null)
            {

                ModelState.AddModelError("", "Balansınızdakı pul göndərmək istədiyiniz puldan azdır!");
                return View();
            }
            if (sendMoneyProcessDTO.ReceiverCardNumber == null)
            {

                return View();
            }


            if (customerActionProcess.Amount >= OneSender.Balance)
            {
                ModelState.AddModelError("", "Balansınızdakı pul göndərmək istədiyiniz puldan azdır!");
                return View();
            }
            if (customerActionProcess.Amount == 0)
            {
                ModelState.AddModelError("", $"Göndərmək istədiyiniz 0 {_customerActionProcessService.TSelectValuta(OneSender.Valutas.Currency)} məbləğ  ola  bilməz!");

                return View();
            }
            if (customerActionProcess.Amount == null)
            {
                ModelState.AddModelError("", "Zəhmət olmasa göndərmək istədiyiniz məbləğ  daxil edin!");

                return View();
            }
            if (customerActionProcess.ReceiverID == null)
            {
                ModelState.AddModelError("", "Zəhmət olmasa pul göndərmək istədiyiniz şəxsin hesab nömrəsini  daxil  edin!");

                return View();
            }
            if (customerActionProcess.SenderID == null)
            {
                ModelState.AddModelError("", "Zəhmət olmasa kart hesabınızı  seçin!");

                return View();
            }
            if (customerActionProcess.SenderID == customerActionProcess.ReceiverID)
            {
                ModelState.AddModelError("", "Öz kart hesabınıza yalnız başqa valyuta və başqa kart nömrəsilə  pul göndərə bilərsiniz!");

                return View();
            }



            _moneyConvertService.ConvertMoneyRS(ReceiverCurrency, SenderCurrency, OneSender, customerActionProcess, ReceiverAccount, sendMoneyProcessDTO, fess);
            _customerAccountService.TUpdate(OneSender);
            _customerAccountService.TUpdate(ReceiverAccount);
            _customerActionProcessService.TCreate(customerActionProcess);
            _emailService.SendMessageForSender(customerActionProcess, OneSender, appUser);
            _emailService.SendMessageForReceiver(customerActionProcess, OneSender.Valutas.Currency, ReceiverAccount, appUser, fess, OneSender.Valutas.Currency, sendMoneyProcessDTO);


            return RedirectToAction("MyLastProcessListForOneCard", "MoneyTransfer", new { valuta = SenderCurrency });
        }


        public async Task<IActionResult> SenderCardInfo(string selectedCard)
        {


            CustomerAccountViewModel CardInfo = GetCardInfo(selectedCard);
            return PartialView("~/Areas/Admin/Views/Shared/_CardInfoPartialView.cshtml", CardInfo);
        }
        //private CustomerAccount GetCardInfo(string SelectCard)
        //{

        //    CustomerAccount OneSender = _db.CustomerAccount.Include(x => x.Receiver).Include(x => x.Sender).Include(x=>x.Valutas).Include(x => x.AppUser).Where(x => x.AccountNumber == SelectCard).FirstOrDefault(x => x.AppUser.UserName == User.Identity.Name);

        //    CustomerAccount Info = new CustomerAccount
        //    {

        //        Balance = OneSender.Balance,



        //};
        //    return Info;
        //}
        private CustomerAccountViewModel GetCardInfo(string SelectCard)
        {
            CustomerAccount OneSender = _db.CustomerAccount
                .Include(x => x.Receiver)
                .Include(x => x.Sender)
                .Include(x => x.Valutas)
                .Include(x => x.AppUser)
                .Where(x => x.AccountNumber == SelectCard)
                .FirstOrDefault(x => x.AppUser.UserName == User.Identity.Name);


            CustomerAccountViewModel Info = new CustomerAccountViewModel
            {

                FullName = OneSender.AppUser.Name + " " + OneSender.AppUser.Surname,
                Balance = OneSender.Balance,
                Valuta = OneSender.Valutas.Currency
            };
            return Info;
        }
    }

}

