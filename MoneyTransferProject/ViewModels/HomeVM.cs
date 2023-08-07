using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyTransferProject.ViewModels
{
    public class HomeVM
    {
        public AboutUs AboutUs { get; set; }
        public List<Feedbacks> Feedbacks { get; set; }
        public ContactUs ContactUs { get; set; }
        public List<BankCards> BankCards { get; set; }
        public List<CardSkills> CardSkills { get; set; }
        public List<Slider> SLiders { get; set; }
        public List<BlogNews> BlogNews { get; set; }
        public List<OurServices> OurServices { get; set; }
        public SliderImage SliderImage { get; set; }
        public List<FrequentlyQuestions> FrequentlyQuestions { get; set; }
    }
}
