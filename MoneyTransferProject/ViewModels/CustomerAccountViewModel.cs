namespace MoneyTransferProject.ViewModels
{
    public class CustomerAccountViewModel
    {
      
        public string FullName { get; set; }
        public decimal Balance { get; set; }
        public string Valuta { get; set; } // Valuta'ya ait Currency değerini taşıyan yeni özellik
    }

}
