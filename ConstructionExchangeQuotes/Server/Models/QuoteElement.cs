namespace ConstructionExchangeQuotes.Server.Models
{
    public class QuoteElement
    {
        public int Amount { get; set; }

        public int ElementId { get; set; }
        public int QuoteId { get; set; }

        public Element Element { get; set; }
        public Quote Quote { get; set; }
    }
}
