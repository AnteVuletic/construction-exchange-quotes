namespace ConstructionExchangeQuotes.Shared
{
    public class QuoteElementDto
    {
        public int Amount { get; set; }

        public int ElementId { get; set; }
        public int QuoteId { get; set; }

        public ElementDto Element { get; set; }
    }
}
