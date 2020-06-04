namespace ConstructionExchangeQuotes.Server.Models
{
    public class ElementField
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        
        public int ElementId { get; set; }

        public Element Element { get; set; }
    }
}
