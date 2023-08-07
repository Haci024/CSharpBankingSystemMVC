using DTOLayer.DTOS.CustomerAccountProcessDTO;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.Abstract
{
    public interface IMoneyConvertService
    {
        public void ConvertMoney(string valuta,CustomerAccount SenderAccount,CustomerAccount ReceiverAccount, SendMoneyProcessDTO sendMoneyProcessDTO,string ReceiverCurrency, decimal fess);
        public void ConvertMoneyRS(string receiverCurrency, string SenderCurrency, CustomerAccount SenderAccount, CustomerActionProcess customerActionProcess, CustomerAccount ReceiverAccount, SendMoneyProcessDTO sendMoneyProcessDTO,decimal fess);
        public decimal ConvertorForReceiverMail(CustomerAccount ReceiverAccount, string valuta, CustomerActionProcess customerActionProcess);

    }
}
