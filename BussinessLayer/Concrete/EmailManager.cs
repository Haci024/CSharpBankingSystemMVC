using BussinessLayer.Abstract;
using DTOLayer.DTOS.CustomerAccountDTOS;
using DTOLayer.DTOS.CustomerAccountProcessDTO;
using EntityLayer.Concrete;
using MimeKit;
using System.Diagnostics;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;


namespace BussinessLayer.Concrete
{
   
    public class EmailManager : IEmailService
    {
        private readonly IMoneyConvertService _moneyConvertService;
        public EmailManager(IMoneyConvertService moneyConvertService) { 
            
            _moneyConvertService = moneyConvertService;
        }

        public void SendMessageForSender(CustomerActionProcess customerActionProcess, CustomerAccount SenderAccount,AppUser user)
        {
            MimeMessage mimeMessage = new MimeMessage();
            MailboxAddress ConfirmAddressFrom = new MailboxAddress("MoneyTransport", "odisseybanks024@gmail.com");
            MailboxAddress ConfirmAdressTo = new MailboxAddress("User", user.Email);
            mimeMessage.From.Add(ConfirmAddressFrom);
            mimeMessage.To.Add(ConfirmAdressTo);
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.TextBody =
                SenderAccount.AppUser.Name + " " + SenderAccount.AppUser.Surname
                + "\n" +
                "Göndərilən məbləğ: " + customerActionProcess.Amount +SenderAccount.Valutas.Currency
                + "\n" +
                "Hesabızdakı balans: " + SenderAccount.Balance+SenderAccount.Valutas.Currency
                + "\n"+
               "Tarix: " + (customerActionProcess.ProcessDate).ToString("g") 
               + "\n"; 
            mimeMessage.Body = bodyBuilder.ToMessageBody();
            mimeMessage.Subject = "Hesab Əməliyyatı: " + customerActionProcess.ProcessType;
            SmtpClient client = new SmtpClient();

            using (var client1 = new MailKit.Net.Smtp.SmtpClient())
            {
                client1.Connect("smtp.gmail.com", 587, false);
                client1.Authenticate("odisseybanks024@gmail.com", "voxryimidhytyjot");
                client1.Send(mimeMessage);
                client1.Disconnect(true);
            }
        }

        public void SendMessageForReceiver(CustomerActionProcess customerAccountProcess, string Currency, CustomerAccount ReceiverAccount, AppUser appUser, decimal fess, string valuta, SendMoneyProcessDTO sendMoneyProcessDTO)
        {
            MimeMessage mimeMessage1 = new MimeMessage();
            MailboxAddress ConfirmAddressFrom1 = new MailboxAddress("MoneyTransport", "odisseybanks024@gmail.com");
            MailboxAddress ConfirmAdressTo1 = new MailboxAddress("User", ReceiverAccount.AppUser.Email);
            mimeMessage1.From.Add(ConfirmAddressFrom1);
            mimeMessage1.To.Add(ConfirmAdressTo1);
            var bodyBuilder1 = new BodyBuilder();
            bodyBuilder1.TextBody = ReceiverAccount.AppUser.Name + " " + ReceiverAccount.AppUser.Surname
                 + "\n" +
                 "Köçən məbləğ: " + (_moneyConvertService.ConvertorForReceiverMail(ReceiverAccount,valuta,customerAccountProcess)).ToString("0.00")+ ReceiverAccount.Valutas.Currency
                 + "\n"+
                 "Hesabızdakı balans: " + (ReceiverAccount.Balance).ToString("0.00") +ReceiverAccount.Valutas.Currency
                 + "\n"+
                 "Tarix: " + (customerAccountProcess.ProcessDate).ToString("g")
                 + "\n";
            mimeMessage1.Body = bodyBuilder1.ToMessageBody();
            mimeMessage1.Subject = "Hesab Əməliyyatı: " + customerAccountProcess.ProcessType;
            using (var client1 = new MailKit.Net.Smtp.SmtpClient())
            {
                client1.Connect("smtp.gmail.com", 587, false);
                client1.Authenticate("odisseybanks024@gmail.com", "voxryimidhytyjot");
                client1.Send(mimeMessage1);
                client1.Disconnect(true);
            }
        }

        public void MessageForNewCustomer(CustomerAccount customerAccount)
        {
            MimeMessage mimeMessage1 = new MimeMessage();
            MailboxAddress ConfirmAddressFrom1 = new MailboxAddress("MoneyTransport", "odisseybanks024@gmail.com");
            MailboxAddress ConfirmAdressTo1 = new MailboxAddress("User", customerAccount.AppUser.Email);
            mimeMessage1.From.Add(ConfirmAddressFrom1);
            mimeMessage1.To.Add(ConfirmAdressTo1);
            var bodyBuilder1 = new BodyBuilder();
            bodyBuilder1.TextBody =  
                  "\n" +"Valuta" +customerAccount.Valutas.Currency+
                  "\n" +
                     "Hesab nömrəniz:" + customerAccount.AccountNumber +
                     "\n" +
                 "Hesabızdakı balans:" +customerAccount.Balance
                 + "\n" +
               "Aktivləşdirmə tarixi:" + (DateTime.Now).ToString("g")
               +"\n"
               +"Zəhmət olmasa gün ərzində ən yaxın filiala yaxınlaşıb kartınızı götürün!"
                 + "\n";
            mimeMessage1.Body = bodyBuilder1.ToMessageBody();
            mimeMessage1.Subject = "Yeni kart sifarişi";
            using (var client1 = new MailKit.Net.Smtp.SmtpClient())
            {
                client1.Connect("smtp.gmail.com", 587, false);
                client1.Authenticate("odisseybanks024@gmail.com", "voxryimidhytyjot");
                client1.Send(mimeMessage1);
                client1.Disconnect(true);
            }
        }

        public void SendActivateCode(CardInfoVerify cardInfoVerify,int activateCode)
        {
            MimeMessage mimeMessage1 = new MimeMessage();
            MailboxAddress ConfirmAddressFrom1 = new MailboxAddress("MoneyTransport", "odisseybanks024@gmail.com");
            MailboxAddress ConfirmAdressTo1 = new MailboxAddress("User", cardInfoVerify.Email);
            mimeMessage1.From.Add(ConfirmAddressFrom1);
            mimeMessage1.To.Add(ConfirmAdressTo1);
            var bodyBuilder1 = new BodyBuilder();
            bodyBuilder1.TextBody =
                 "Aktivləşdirmə kodu:" + activateCode
                 + "\n" +
               "Göndərilmə tarixi:" + (DateTime.Now).ToString("g")
               + "\n";
              
                
            mimeMessage1.Body = bodyBuilder1.ToMessageBody();
            mimeMessage1.Subject = "Alış təsdiqləmə formu";
            using (var client1 = new MailKit.Net.Smtp.SmtpClient())
            {
                client1.Connect("smtp.gmail.com", 587, false);
                client1.Authenticate("odisseybanks024@gmail.com", "voxryimidhytyjot");
                client1.Send(mimeMessage1);
                client1.Disconnect(true);
            }
        }

        public void SendCreditEmail(Credits credits)
        {
            MimeMessage mimeMessage1 = new MimeMessage();
            MailboxAddress ConfirmAddressFrom1 = new MailboxAddress("OdisseyBank", "odisseybanks024@gmail.com");
            MailboxAddress ConfirmAdressTo1 = new MailboxAddress("User", credits.CustomerAccount.AppUser.Email);
            mimeMessage1.From.Add(ConfirmAddressFrom1);
            mimeMessage1.To.Add(ConfirmAdressTo1);
            var bodyBuilder1 = new BodyBuilder();
            bodyBuilder1.TextBody =
                 "Kredit məbləği:" + credits.CreditAmount + credits.CustomerAccount.Valutas.Currency
                 + "\n" +
                  "Aylıq ödəniş:" + credits.CreditDetail.MonthlyPayment +credits.CustomerAccount.Valutas.Currency
                 + "\n" +
                  "İlk ödəniş tarixi:" + ((DateTime.Now).AddMonths(1).ToString("dd.MMM.yyyy")
                 + "\n" +
                   "Ümumi ödəniş:" + credits.TotalPayment
                 + "\n" +
               "Göndərilmə tarixi:" + (DateTime.Now).ToString("g")
               +"\n"+"Pul uğurla hesabıza köçürüldü.Bizi seçdiyiniz üçün təşəkkürlər.Hörmətlə Odissey Bank.Sizin bankınız."
               + "\n");


            mimeMessage1.Body = bodyBuilder1.ToMessageBody();
            mimeMessage1.Subject = " Kredit götürmə əməliyyatı";
            using (var client1 = new MailKit.Net.Smtp.SmtpClient())
            {
                client1.Connect("smtp.gmail.com", 587, false);
                client1.Authenticate("odisseybanks024@gmail.com", "voxryimidhytyjot");
                client1.Send(mimeMessage1);
                client1.Disconnect(true);
            }
        }

        public void SendFailCreditEmail(Credits credits)
        {

            MimeMessage mimeMessage1 = new MimeMessage();
            MailboxAddress ConfirmAddressFrom1 = new MailboxAddress("OdisseyBank", "odisseybanks024@gmail.com");
            MailboxAddress ConfirmAdressTo1 = new MailboxAddress("User", credits.CustomerAccount.AppUser.Email);
            mimeMessage1.From.Add(ConfirmAddressFrom1);
            mimeMessage1.To.Add(ConfirmAdressTo1);
            var bodyBuilder1 = new BodyBuilder();
            bodyBuilder1.TextBody =
                 "Kredit məbləği:" + credits.CreditAmount + credits.CustomerAccount.Valutas.Currency
                 + "\n" +
                  "Hörmətli," + credits.CustomerAccount.AppUser.Name +credits.CustomerAccount.AppUser.Surname +" təəsüüf ki siz kredit sənədindəki şərtləri və qaydaları qəbul" +
                  " etmədiyiniz aşkarlandı və buna görə kredit götürmə əməliyyatı ləğv edildi.Bir daha cəhd edin.Əgər problem olarsa bizimlə əlaqə saxlamağınızı xahiş edirik." +
                  "Xoş gün arzu edirik!Hörmətlə Odissey Bank!Sizin bankınız."+ "\n"
                  +"Göndərilmə tarixi:"
                  + ((DateTime.Now).ToString("g")   
               + "\n");


            mimeMessage1.Body = bodyBuilder1.ToMessageBody();
            mimeMessage1.Subject = " Uğursuz əməliyyat!";
            using (var client1 = new MailKit.Net.Smtp.SmtpClient())
            {
                client1.Connect("smtp.gmail.com", 587, false);
                client1.Authenticate("odisseybanks024@gmail.com", "voxryimidhytyjot");
                client1.Send(mimeMessage1);
                client1.Disconnect(true);
            }

        }

        public void SendCompleteCreditMail(Credits credits,CustomerActionProcess customerActionProcess,MonthlyPayment monthlyPayment)
        {
            MimeMessage mimeMessage = new MimeMessage();
            MailboxAddress ConfirmAddressFrom = new MailboxAddress("MoneyTransport", "odisseybanks024@gmail.com");
            MailboxAddress ConfirmAdressTo = new MailboxAddress("User", credits.CustomerAccount.AppUser.Email);
            mimeMessage.From.Add(ConfirmAddressFrom);
            mimeMessage.To.Add(ConfirmAdressTo);
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.TextBody =
               credits.CustomerAccount.AppUser.Name + " " + credits.CustomerAccount.AppUser.Surname
                + "\n" +
                " Köçürülən məbləğ: " + monthlyPayment.Payment +" " +credits.CustomerAccount.Valutas.Currency
                + "\n" +
                "Hesabızdakı balans:" + credits.CustomerAccount.Balance +" "+ credits.CustomerAccount.Valutas.Currency
                 + "\n" +
                "Təbriklər!!!Siz kreditin son aylığını ödədiniz!Artıq növbəti kreditinizi götürə bilərsiniz!Bizi seçdiyiniz üçün təşəkkürlər!Hörmətlə Odissey Bank!"
                 +"\n" +
                 "Göndərilmə tarixi: " + (monthlyPayment.ProcessDate).ToString("g")
                 + "\n";
            mimeMessage.Body = bodyBuilder.ToMessageBody();
            mimeMessage.Subject = "Hesab Əməliyyatı: " +customerActionProcess.ProcessType;
            SmtpClient client = new SmtpClient();

            using (var client1 = new MailKit.Net.Smtp.SmtpClient())
            {
                client1.Connect("smtp.gmail.com", 587, false);
                client1.Authenticate("odisseybanks024@gmail.com", "voxryimidhytyjot");
                client1.Send(mimeMessage);
                client1.Disconnect(true);
            }
        }

        public void SendMonthlyPaymentCreditMail(Credits credits, CustomerActionProcess customerActionProcess, MonthlyPayment monthlyPayment)
        {
            MimeMessage mimeMessage = new MimeMessage();
            MailboxAddress ConfirmAddressFrom = new MailboxAddress("MoneyTransport", "odisseybanks024@gmail.com");
            MailboxAddress ConfirmAdressTo = new MailboxAddress("User", credits.CustomerAccount.AppUser.Email);
            mimeMessage.From.Add(ConfirmAddressFrom);
            mimeMessage.To.Add(ConfirmAdressTo);
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.TextBody =
                credits.CustomerAccount.AppUser.Name + " " + credits.CustomerAccount.AppUser.Surname
                + "\n" +
               " Köçürülən məbləğ: " + monthlyPayment.Payment + " " + credits.CustomerAccount.Valutas.Currency
                + "\n" +
               "Hesabızdakı balans:" + credits.CustomerAccount.Balance + " " + credits.CustomerAccount.Valutas.Currency
                + "\n" +
               "Tarix: " + (monthlyPayment.ProcessDate).ToString("g")
               + "\n";
            mimeMessage.Body = bodyBuilder.ToMessageBody();
            mimeMessage.Subject = "Hesab Əməliyyatı: " + customerActionProcess.ProcessType;
            SmtpClient client = new SmtpClient();

            using (var client1 = new MailKit.Net.Smtp.SmtpClient())
            {
                client1.Connect("smtp.gmail.com", 587, false);
                client1.Authenticate("odisseybanks024@gmail.com", "voxryimidhytyjot");
                client1.Send(mimeMessage);
                client1.Disconnect(true);
            }
        }
    }
}
