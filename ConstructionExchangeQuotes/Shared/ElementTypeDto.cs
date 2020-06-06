using System.ComponentModel.DataAnnotations;

namespace ConstructionExchangeQuotes.Shared
{
    public class ElementTypeDto
    {
        public int? Id { get; set; }
        [Required]
        [MinLength(4, ErrorMessage = "Please add element types that are described by more then 4 characters")]
        public string Name { get; set; }

        public ElementTypeDto CloneObject()
        {
            return (ElementTypeDto)this.MemberwiseClone();
        }
    }
}
