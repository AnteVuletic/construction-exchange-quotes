using System;
using System.Collections.Generic;

namespace ConstructionExchangeQuotes.Server.Models
{
    public class Quote
    {
        public int Id { get; set; }
        public double SubTotal { get; set; }
        public double TaxRatePercentage { get; set; }
        public DateTime DateCreated { get; set; }
        public string CustomerEmail { get; set; }

        public ICollection<QuoteElement> QuoteElements { get; set; }
    }
}
