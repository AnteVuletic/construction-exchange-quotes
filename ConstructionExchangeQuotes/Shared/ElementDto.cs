using System.Collections.Generic;

namespace ConstructionExchangeQuotes.Shared
{
    public class ElementDto
    {
        public ElementDto()
        {
            ElementType = new ElementTypeDto();
            ElementCategory = new ElementCategoryDto();
            ElementFields = new List<ElementFieldDto>();
        }
        public int? Id { get; set; }
        public string Name { get; set; }
        public double Rate { get; set; }

        public ElementTypeDto ElementType { get; set; }
        public ElementCategoryDto ElementCategory { get; set; }

        public ICollection<ElementFieldDto> ElementFields { get; set; }
    }
}
