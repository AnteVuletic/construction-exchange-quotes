using System.Collections.Generic;

namespace ConstructionExchangeQuotes.Server.Models
{
    public class ElementType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Element> Elements { get; set; }
    }
}
