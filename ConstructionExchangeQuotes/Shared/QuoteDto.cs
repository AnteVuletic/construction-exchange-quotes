using System;
using System.Collections.Generic;

namespace ConstructionExchangeQuotes.Shared
{
    public class QuoteDto
    {
        public int Id { get; set; }
        public double SubTotal { get; set; }
        public double TaxRatePercentage { get; set; }
        public DateTime DateCreated { get; set; }
        public string CustomerEmail { get; set; }
        public bool IsArchived { get; set; }

        public ICollection<QuoteElementDto> QuoteElements { get; set; }
    }
}
