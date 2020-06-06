using System.Collections.Generic;
using System.Linq;
using ConstructionExchangeQuotes.Server.Models;
using ConstructionExchangeQuotes.Shared;

namespace ConstructionExchangeQuotes.Server.Repositories
{
    public class ElementRepository
    {
        private readonly QuotesDbContext _quotesDbContext;

        public ElementRepository(QuotesDbContext quotesDbContext)
        {
            _quotesDbContext = quotesDbContext;
        }

        public IEnumerable<ElementCategoryDto> GetElementCategories()
        {
            return _quotesDbContext.ElementCategories.Select(x => new ElementCategoryDto
            {
                Id = x.Id,
                Name = x.Name
            });
        }

        public IEnumerable<ElementTypeDto> GetElementTypes()
        {
            return _quotesDbContext.ElementTypes.Select(x => new ElementTypeDto
            {
                Id = x.Id,
                Name = x.Name
            });
        }

        public IEnumerable<ElementDto> GetElements()
        {
            return _quotesDbContext.Elements.Select(x => new ElementDto
            {
                Id = x.Id,
                Name = x.Name,
                Rate = x.Rate,
                ElementCategory = new ElementCategoryDto { 
                    Id = x.ElementCategory.Id,
                    Name = x.ElementCategory.Name
                },
                ElementType = new ElementTypeDto { 
                    Id = x.ElementType.Id,
                    Name = x.ElementType.Name
                },
                ElementFields = x.ElementFields.Select(ef => new ElementFieldDto { 
                    Id = ef.Id,
                    Name = ef.Name,
                    Value = ef.Value
                }).ToList()
            });
        }

        public bool AddElement(ElementDto elementDto)
        {
            var isElementValid = IsElementValid(elementDto);
            if (!isElementValid)
                return false;

            var element = new Element
            {
                Name = elementDto.Name,
                Rate = elementDto.Rate,
                ElementTypeId = elementDto.ElementType.Id.Value,
                ElementCategoryId = elementDto.ElementCategory.Id.Value,
                ElementFields = elementDto.ElementFields.Select(x => new ElementField
                {
                    Name = x.Name,
                    Value = x.Value
                }).ToList()
            };

            _quotesDbContext.Elements.Add(element);
            _quotesDbContext.SaveChanges();
            return true;
        }

        public bool EditElement(ElementDto elementDto)
        {
            var isElementValid = IsElementValid(elementDto);
            if (!isElementValid)
                return false;

            var element = _quotesDbContext.Elements.Find(elementDto.Id.Value);
            element.Name = elementDto.Name;
            element.Rate = elementDto.Rate;
            element.ElementCategoryId = elementDto.ElementCategory.Id.Value;
            element.ElementTypeId = elementDto.ElementType.Id.Value;

            var elementFieldsDeleted =
                element.ElementFields.Where(x => elementDto.ElementFields.All(ef => (ef.Id ?? 0) != x.Id));
            var elementsEdited =
                element.ElementFields.Where(x => elementDto.ElementFields.Any(ef => (ef.Id ?? 0) == x.Id));

            foreach (var elementField in elementsEdited)
            {
                var fieldEdited = elementDto.ElementFields.First(x => x.Id == elementField.Id);
                elementField.Name = fieldEdited.Name;
                elementField.Value = fieldEdited.Value;
            }

            var fieldsAdded = elementDto.ElementFields.Where(x => x.Id == 0).Select(x => new ElementField
            {
                Name = x.Name,
                Value = x.Value,
                Element = element
            });

            _quotesDbContext.ElementFields.AddRange(fieldsAdded);
            _quotesDbContext.ElementFields.RemoveRange(elementFieldsDeleted);
            _quotesDbContext.SaveChanges();
            return true;
        }

        public void DeleteElement(int elementId)
        {
            var element = _quotesDbContext.Elements.Find(elementId);
            _quotesDbContext.Elements.Remove(element);
            _quotesDbContext.SaveChanges();
        }

        private bool IsElementValid(ElementDto elementDto)
        { 
            return elementDto.ElementType.Id.HasValue && elementDto.ElementCategory.Id.HasValue;
        }

        public void AddElementCategory(ElementCategoryDto elementCategoryDto)
        {
            var elementCategory = new ElementCategory
            {
                Name = elementCategoryDto.Name
            };

            _quotesDbContext.ElementCategories.Add(elementCategory);
            _quotesDbContext.SaveChanges();
        }

        public bool DeleteElementCategory(int elementCategoryId)
        {
            var isElementCategoryOnElement =
                _quotesDbContext.ElementCategories.Any(x => x.Id == elementCategoryId && x.Elements.Any());

            if (isElementCategoryOnElement)
                return false;

            var elementCategory = _quotesDbContext.ElementCategories.Find(elementCategoryId);
            _quotesDbContext.ElementCategories.Remove(elementCategory);
            _quotesDbContext.SaveChanges();
            return true;
        }

        public void EditElementCategory(ElementCategoryDto elementCategoryDto)
        {
            var elementCategory = _quotesDbContext.ElementCategories.Find(elementCategoryDto.Id.Value);
            elementCategory.Name = elementCategoryDto.Name;

            _quotesDbContext.SaveChanges();
        }

        public void AddElementType(ElementTypeDto elementTypeDto)
        {
            var elementType = new ElementType
            {
                Name = elementTypeDto.Name
            };

            _quotesDbContext.ElementTypes.Add(elementType);
            _quotesDbContext.SaveChanges();
        }

        public bool DeleteElementType(int id)
        {
            var isElementTypeOnElement = _quotesDbContext.ElementTypes.Any(x => x.Id == id && x.Elements.Any());
            if (isElementTypeOnElement)
                return false;

            var elementType = _quotesDbContext.ElementTypes.Find(id);

            _quotesDbContext.ElementTypes.Remove(elementType);
            _quotesDbContext.SaveChanges();
            return true;
        }

        public void EditElementType(ElementTypeDto elementTypeDto)
        {
            var elementType = _quotesDbContext.ElementTypes.Find(elementTypeDto.Id.Value);
            elementType.Name = elementTypeDto.Name;

            _quotesDbContext.SaveChanges();
        }
    }
}
