using EntityLayer.Concrete;
using Infobip.Api.Client.Api;
using Infobip.Api.Client.Model;
using Infobip.Api.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BussinessLayer.Abstract;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace BussinessLayer.Concrete
{

    public class SMSManager:ISMSService
    {
        private static readonly string BASE_URL = "https://r5djxe.api.infobip.com";
        private static readonly string API_KEY = "61cb89f4d9ba668aaca5cc423a5076af-9b56adc7-8384-4e7a-8f1f-6c69dc16582a";
        private static readonly string SENDER = "OdisseyBank024";
        private static readonly string RECIPIENT = "994555515345";
        public void SendVerifySMS(int random, string phoneNumber)
        {
            var configuration = new Configuration()
            {
                BasePath = BASE_URL,
                ApiKeyPrefix = "App",
                ApiKey = API_KEY
            };

            var sendSmsApi = new SendSmsApi(configuration);

            var smsMessage = new SmsTextualMessage()
            {
                From = SENDER,
                Destinations = new List<SmsDestination>()
                {
                    new SmsDestination(to: phoneNumber)
                },
                Text = "Aktivləşdirmə şifrəniz:" + random.ToString()
            };

            var smsRequest = new SmsAdvancedTextualRequest()
            {
                Messages = new List<SmsTextualMessage>() { smsMessage }
            };

            try
            {
                var smsResponse = sendSmsApi.SendSmsMessage(smsRequest);


            }
            catch (ApiException apiException)
            {

            }


        }
       
    }
}
