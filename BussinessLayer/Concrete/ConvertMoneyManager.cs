using BussinessLayer.Abstract;
using DTOLayer.DTOS.CustomerAccountProcessDTO;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.Concrete
{
    public class ConvertMoneyManager:IMoneyConvertService
    {
        public void ConvertMoney(string valuta,CustomerAccount SenderAccount,CustomerAccount ReceiverAccount,SendMoneyProcessDTO sendMoneyProcessDTO,string ReceiverCurrency, decimal fess)
        {
            if (valuta == "AZN")
            {

                switch (ReceiverCurrency)
                {
                    case "AZN":
                        SenderAccount.Balance = SenderAccount.Balance - sendMoneyProcessDTO.Amount;
                        ReceiverAccount.Balance = ReceiverAccount.Balance + (sendMoneyProcessDTO.Amount - fess) * 1.0m;
                        break;
                    case "USD":
                        SenderAccount.Balance = SenderAccount.Balance - sendMoneyProcessDTO.Amount;
                        ReceiverAccount.Balance = ReceiverAccount.Balance + (sendMoneyProcessDTO.Amount - fess) / 1.7m;
                        break;
                    case "EUR":
                        SenderAccount.Balance = SenderAccount.Balance - sendMoneyProcessDTO.Amount;
                        ReceiverAccount.Balance = ReceiverAccount.Balance + (sendMoneyProcessDTO.Amount - fess) / 1.84m;
                        break;
                    case "TRL":
                        SenderAccount.Balance = SenderAccount.Balance - sendMoneyProcessDTO.Amount;
                        ReceiverAccount.Balance = ReceiverAccount.Balance + (sendMoneyProcessDTO.Amount - fess) * 15.35m;
                        break;
                    default:
                        break;
                }

            }
            else if (valuta == "USD")
            {
                switch (ReceiverCurrency)
                {
                    case "AZN":
                        SenderAccount.Balance = SenderAccount.Balance - sendMoneyProcessDTO.Amount;
                        ReceiverAccount.Balance = ReceiverAccount.Balance + (sendMoneyProcessDTO.Amount - fess) * 1.7m;
                        break;
                    case "USD":
                        SenderAccount.Balance = SenderAccount.Balance - sendMoneyProcessDTO.Amount;
                        ReceiverAccount.Balance = ReceiverAccount.Balance + (sendMoneyProcessDTO.Amount - fess) * 1.0m;
                        break;
                    case "EUR":
                        SenderAccount.Balance = SenderAccount.Balance - sendMoneyProcessDTO.Amount;
                        ReceiverAccount.Balance = ReceiverAccount.Balance + (sendMoneyProcessDTO.Amount - fess) / 1.08m;
                        break;
                    case "TRL":
                        SenderAccount.Balance = SenderAccount.Balance - sendMoneyProcessDTO.Amount;
                        ReceiverAccount.Balance = ReceiverAccount.Balance + (sendMoneyProcessDTO.Amount - fess) * 28.41m;
                        break;
                    default:
                        break;
                }
            }
            else if (valuta == "TRL")
            {
                switch (ReceiverCurrency)
                {
                    case "AZN":
                        SenderAccount.Balance = SenderAccount.Balance - sendMoneyProcessDTO.Amount;
                        ReceiverAccount.Balance = ReceiverAccount.Balance + (sendMoneyProcessDTO.Amount - fess) / 15.35m;
                        break;
                    case "USD":
                        SenderAccount.Balance = SenderAccount.Balance - sendMoneyProcessDTO.Amount;
                        ReceiverAccount.Balance = ReceiverAccount.Balance + (sendMoneyProcessDTO.Amount - fess) / 26.1m;
                        break;
                    case "EUR":
                        SenderAccount.Balance = SenderAccount.Balance - sendMoneyProcessDTO.Amount;
                        ReceiverAccount.Balance = ReceiverAccount.Balance + (sendMoneyProcessDTO.Amount - fess) / 28.41m;
                        break;
                    case "TRL":
                        SenderAccount.Balance = SenderAccount.Balance - sendMoneyProcessDTO.Amount;
                        ReceiverAccount.Balance = ReceiverAccount.Balance + (sendMoneyProcessDTO.Amount - fess) * 1.0m;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (ReceiverCurrency)
                {
                    case "AZN":
                        SenderAccount.Balance = SenderAccount.Balance - sendMoneyProcessDTO.Amount;
                        ReceiverAccount.Balance = ReceiverAccount.Balance + (sendMoneyProcessDTO.Amount - fess) * 1.84m;
                        break;
                    case "USD":
                        SenderAccount.Balance = SenderAccount.Balance - sendMoneyProcessDTO.Amount;
                        ReceiverAccount.Balance = ReceiverAccount.Balance + (sendMoneyProcessDTO.Amount - fess) * 1.08m;
                        break;
                    case "EUR":
                        SenderAccount.Balance = SenderAccount.Balance - sendMoneyProcessDTO.Amount;
                        ReceiverAccount.Balance = ReceiverAccount.Balance + (sendMoneyProcessDTO.Amount - fess) * 1.0m;
                        break;
                    case "TRL":
                        SenderAccount.Balance = SenderAccount.Balance - sendMoneyProcessDTO.Amount;
                        ReceiverAccount.Balance = ReceiverAccount.Balance + (sendMoneyProcessDTO.Amount - fess) * 28.41m;
                        break;
                    default:
                      
                        break;
                }
            }
        }

        public void ConvertMoneyRS(string ReceiverCurrency, string SenderCurrency, CustomerAccount OneSender,CustomerActionProcess customerActionProcess, CustomerAccount ReceiverAccount, SendMoneyProcessDTO sendMoneyProcessDTO, decimal fess)
        {
            if (SenderCurrency == "AZN")
            {
                switch (ReceiverCurrency)
                {
                    case "AZN":
                        OneSender.Balance = OneSender.Balance - sendMoneyProcessDTO.Amount;
                        ReceiverAccount.Balance = ReceiverAccount.Balance + (sendMoneyProcessDTO.Amount) * 1.0m;
                        break;
                    case "USD":
                        OneSender.Balance = OneSender.Balance - sendMoneyProcessDTO.Amount;
                        ReceiverAccount.Balance = ReceiverAccount.Balance + (sendMoneyProcessDTO.Amount) / 1.7m;
                        break;
                    case "EUR":
                        OneSender.Balance = OneSender.Balance - sendMoneyProcessDTO.Amount;
                        ReceiverAccount.Balance = ReceiverAccount.Balance + (sendMoneyProcessDTO.Amount) / 1.84m;
                        break;
                    case "TRL":
                        OneSender.Balance = OneSender.Balance - sendMoneyProcessDTO.Amount;
                        ReceiverAccount.Balance = ReceiverAccount.Balance + (sendMoneyProcessDTO.Amount) * 15.35m;
                        break;
                    default:
                        break;
                }

            }
            else if (SenderCurrency == "USD")
            {
                switch (ReceiverCurrency)
                {
                    case "AZN":
                        OneSender.Balance = OneSender.Balance - sendMoneyProcessDTO.Amount;
                        ReceiverAccount.Balance = ReceiverAccount.Balance + (sendMoneyProcessDTO.Amount) * 1.7m;
                        break;
                    case "USD":
                        OneSender.Balance = OneSender.Balance - sendMoneyProcessDTO.Amount;
                        ReceiverAccount.Balance = ReceiverAccount.Balance + (sendMoneyProcessDTO.Amount) * 1.0m;
                        break;
                    case "EUR":
                        OneSender.Balance = OneSender.Balance = OneSender.Balance - sendMoneyProcessDTO.Amount;
                        ReceiverAccount.Balance = ReceiverAccount.Balance + (sendMoneyProcessDTO.Amount) / 1.08m;
                        break;
                    case "TRL":
                        OneSender.Balance = OneSender.Balance - sendMoneyProcessDTO.Amount;
                        ReceiverAccount.Balance = ReceiverAccount.Balance + (sendMoneyProcessDTO.Amount) * 28.41m;
                        break;
                    default:
                        break;
                }
            }
            else if (SenderCurrency == "TRL")
            {
                switch (ReceiverCurrency)
                {
                    case "AZN":
                        OneSender.Balance = OneSender.Balance - sendMoneyProcessDTO.Amount;
                        ReceiverAccount.Balance = ReceiverAccount.Balance + (sendMoneyProcessDTO.Amount) / 15.35m;
                        break;
                    case "USD":
                        OneSender.Balance = OneSender.Balance - sendMoneyProcessDTO.Amount;
                        ReceiverAccount.Balance = ReceiverAccount.Balance + (sendMoneyProcessDTO.Amount) / 26.1m;
                        break;
                    case "EUR":
                        OneSender.Balance = OneSender.Balance - sendMoneyProcessDTO.Amount;
                        ReceiverAccount.Balance = ReceiverAccount.Balance + (sendMoneyProcessDTO.Amount) / 28.41m;
                        break;
                    case "TRL":
                        OneSender.Balance = OneSender.Balance - sendMoneyProcessDTO.Amount;
                        ReceiverAccount.Balance = ReceiverAccount.Balance + (sendMoneyProcessDTO.Amount) * 1.0m;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (ReceiverCurrency)
                {
                    case "AZN":
                        OneSender.Balance = OneSender.Balance - sendMoneyProcessDTO.Amount;
                        ReceiverAccount.Balance = ReceiverAccount.Balance + (sendMoneyProcessDTO.Amount) * 1.84m;
                        break;
                    case "USD":
                        OneSender.Balance = OneSender.Balance - sendMoneyProcessDTO.Amount;
                        ReceiverAccount.Balance = ReceiverAccount.Balance + (sendMoneyProcessDTO.Amount) * 1.08m;
                        break;
                    case "EUR":
                        OneSender.Balance = OneSender.Balance - sendMoneyProcessDTO.Amount;
                        ReceiverAccount.Balance = ReceiverAccount.Balance + (sendMoneyProcessDTO.Amount) * 1.0m;
                        break;
                    case "TRL":
                        OneSender.Balance = OneSender.Balance - sendMoneyProcessDTO.Amount;
                        ReceiverAccount.Balance = ReceiverAccount.Balance + (sendMoneyProcessDTO.Amount) * 28.41m;
                        break;
                    default:
                       
                        break;
                }
                
            }

        }

        public  decimal ConvertorForReceiverMail(CustomerAccount ReceiverAccount,string valuta,CustomerActionProcess customerActionProcess)
        {
            decimal value =decimal.Zero;
            if (valuta == "USD")
            {
                switch (ReceiverAccount.Valutas.Currency)
                {
                    case "AZN":

                        value= (customerActionProcess.Amount) * 1.7m;
                        break;

                    case "USD":

                        value= (customerActionProcess.Amount) * 1.0m;
                        break;

                    case "EUR":

                        value= (customerActionProcess.Amount)/ 1.08m;
                        break;

                    case "TRL":

                        value= (customerActionProcess.Amount) * 26.1m;
                        break;

                    default:
                        break;

                }
            }
            else if (valuta == "AZN")
            {
                switch (ReceiverAccount.Valutas.Currency)
                {
                    case "AZN":

                        value= (customerActionProcess.Amount) * 1.0m;
                        break;

                    case "USD":

                        value= (customerActionProcess.Amount) / 1.7m;
                        break;

                    case "EUR":

                        value= (customerActionProcess.Amount) / 1.84m;
                        break;

                    case "TRL":

                        value= (customerActionProcess.Amount) * 15.35m;
                        break;

                    default:
                        break;

                }
            }
            else if (valuta=="TRL")
            {
                switch (ReceiverAccount.Valutas.Currency)
                {
                    case "AZN":
                        
                     value= (customerActionProcess.Amount) / 15.35m;
                        break;
                        
                    case "USD":
                      
                        value=  (customerActionProcess.Amount) / 26.1m;
                        break;
                       
                    case "EUR":
                        
                        value= (customerActionProcess.Amount) / 28.41m;
                        break;
                        
                    case "TRL":
                       
                        value=(customerActionProcess.Amount) * 1.0m;
                        break;
                       
                    default:
                        break;
                }
            }
            else
            {
                switch (ReceiverAccount.Valutas.Currency)
                {
                    case "AZN":

                        value= (customerActionProcess.Amount) * 1.84m;
                        break;
                       
                        
                    case "USD":

                        value= (customerActionProcess.Amount) * 1.08m;
                        break;
                        
                    case "EUR":

                        value= (customerActionProcess.Amount) / 1.0m;
                        break;
                        
                    case "TRL":

                        value= (customerActionProcess.Amount) * 28.41m;
                        break;
                        
                    default:
                        break;
                }
                
            }
            return value;
            
            
            
            


          
        }
    }
}
