using Infobip.Api.Client.Api;
using Infobip.Api.Client.Model;
using Infobip.Api.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using EntityLayer.Concrete;
using DTOLayer.DTOS.CustomerAccountDTOS;

namespace BussinessLayer.Abstract
{
    public interface ISMSService
    {

        public void SendVerifySMS(int random, string PhoneNumber);

    }
}

