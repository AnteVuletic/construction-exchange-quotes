using System;
using System.ComponentModel.DataAnnotations;

namespace ConstructionExchangeQuotes.Shared
{
    public class ElementCategoryDto
    {
        public int? Id { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Minimum category name is 3 characters")]
        public string Name { get; set; }

        public ElementCategoryDto CloneObject()
        {
            return (ElementCategoryDto)this.MemberwiseClone();
        }
    }
}
