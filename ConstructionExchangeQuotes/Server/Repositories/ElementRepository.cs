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
            return true;
        }

        public bool EditElement(ElementDto elementDto)
        {
            var isElementValid = IsElementValid(elementDto);
            if (!isElementValid)
                return false;

            var element = _quotesDbContext.Elements.Find(elementDto.Id);
            element.Name = elementDto.Name;
            element.Rate = elementDto.Rate;
            var elementFieldsDeleted =
                element.ElementFields.Where(x => elementDto.ElementFields.All(ef => (ef.Id ?? 0) != x.Id));
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
            var elementCategory = _quotesDbContext.ElementCategories.Find(elementCategoryDto.Id);
            elementCategory.Name = elementCategory.Name;

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
            var elementType = _quotesDbContext.ElementTypes.Find(elementTypeDto.Id);
            elementType.Name = elementType.Name;

            _quotesDbContext.SaveChanges();
        }
    }
}
