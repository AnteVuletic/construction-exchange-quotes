using System.Collections.Generic;

namespace ConstructionExchangeQuotes.Server.Models
{
    public class Element
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Rate { get; set; }

        public int ElementTypeId { get; set; }
        public int ElementCategoryId { get; set; }

        public ElementType ElementType { get; set; }
        public ElementCategory ElementCategory { get; set; }

        public ICollection<ElementField> ElementFields { get; set; }
        public ICollection<QuoteElement> QuoteElements { get; set; }
    }
}
