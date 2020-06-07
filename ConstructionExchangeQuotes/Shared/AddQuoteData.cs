using System.Collections.Generic;

namespace ConstructionExchangeQuotes.Shared
{
    public class AddQuoteData
    {
        public double TaxRatePercentage { get; set; }
        public string CustomerEmail { get; set; }
        public List<QuoteElementDto> QuoteElements { get; set; }
        public bool ShouldNotifyByEmail { get; set; }
    }
}
