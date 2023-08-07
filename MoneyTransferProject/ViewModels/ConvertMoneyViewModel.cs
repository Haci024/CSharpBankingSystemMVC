namespace MoneyTransferProject.ViewModels
{
    public class ConvertMoneyViewModel
    {
        public string FromCurrency { get; set; }
        public string ToCurrency { get; set; }
        public double Amount { get; set; }
        public double? ConvertedAmount { get; set; }
        public List<string> CurrencyOptions { get; set; } // CurrencyOptions özelliği eklendi

        public ConvertMoneyViewModel()
        {
            CurrencyOptions = new List<string> { "AZN", "USD", "EUR", "GBP", "TRY" }; // Örnek para birimleri ekleniyor
        }
    }
}
