using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MimeKit;
using DTOLayer.DTOS.CustomerAccountProcessDTO;
using DTOLayer.DTOS.CustomerAccountDTOS;

namespace BussinessLayer.Abstract
{
    public interface IEmailService
    {
       public void SendMessageForSender(CustomerActionProcess customerActionProcess, CustomerAccount senderAccount,AppUser appUser);
       public void SendMessageForReceiver(CustomerActionProcess customerAccountProcess,string currency, CustomerAccount receiverAccount,AppUser appUser,decimal fess,string valuta,SendMoneyProcessDTO send);
       public  void MessageForNewCustomer(CustomerAccount customerAccount);
        public void SendActivateCode(CardInfoVerify cardInfoVerify,int activateCode);
        public void SendCreditEmail(Credits credits);
        public void SendFailCreditEmail(Credits credits);
        public void SendCompleteCreditMail(Credits credits,CustomerActionProcess customerActionProcess,MonthlyPayment monthlyPayment);
        public void SendMonthlyPaymentCreditMail(Credits credits, CustomerActionProcess customerActionProcess, MonthlyPayment monthlyPayment);
    }
}
