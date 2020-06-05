using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        [Required]
        [MinLength(2, ErrorMessage = "Minimum name length is 2")]
        public string Name { get; set; }
        [Required]
        public double Rate { get; set; }

        [Required]
        public ElementTypeDto ElementType { get; set; }
        [Required]
        public ElementCategoryDto ElementCategory { get; set; }

        public ICollection<ElementFieldDto> ElementFields { get; set; }
    }
}
