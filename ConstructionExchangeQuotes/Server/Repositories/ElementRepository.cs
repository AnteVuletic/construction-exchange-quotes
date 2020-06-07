using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using ConstructionExchangeQuotes.Server.Models;
using ConstructionExchangeQuotes.Server.Utils;
using ConstructionExchangeQuotes.Shared;
using Microsoft.EntityFrameworkCore;

namespace ConstructionExchangeQuotes.Server.Repositories
{
    public class ElementRepository
    {
        private readonly QuotesDbContext _quotesDbContext;

        public ElementRepository(QuotesDbContext quotesDbContext)
        {
            _quotesDbContext = quotesDbContext;
        }

        public IEnumerable<ElementCategoryDto> GetElementCategories(string name)
        {
            Expression<Func<ElementCategory, bool>> baseExpression = x => true;

            if (!string.IsNullOrWhiteSpace(name))
                baseExpression = baseExpression.AndAlso(x => x.Name.ToLower().Contains(name.ToLower()));

            return _quotesDbContext.ElementCategories
                .Where(baseExpression)
                .Select(x => new ElementCategoryDto
                {
                    Id = x.Id,
                    Name = x.Name
                });
        }

        public IEnumerable<ElementTypeDto> GetElementTypes(string name)
        {
            Expression<Func<ElementType, bool>> baseExpression = x => true;

            if (!string.IsNullOrWhiteSpace(name))
                baseExpression = baseExpression.AndAlso(x => x.Name.ToLower().Contains(name.ToLower()));

            return _quotesDbContext.ElementTypes
                .Where(baseExpression)
                .Select(x => new ElementTypeDto
                {
                    Id = x.Id,
                    Name = x.Name
                });
        }

        public IEnumerable<ElementDto> GetElements(string name, int categoryId, int typeId)
        {
            Expression<Func<Element, bool>> baseExpression = x => true;

            if (!string.IsNullOrWhiteSpace(name))
                baseExpression = baseExpression.AndAlso((x) => x.Name.ToLower().Contains(name.ToLower()));

            if (categoryId != 0)
                baseExpression = baseExpression.AndAlso((x) => x.ElementCategory.Id == categoryId);

            if (typeId != 0)
                baseExpression = baseExpression.AndAlso((x) => x.ElementType.Id == typeId);

            return _quotesDbContext.Elements
                .Where(baseExpression)
                .Select(x => new ElementDto
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

            var element = _quotesDbContext.Elements
                .AsQueryable()
                .Include(x => x.ElementFields)
                .Single(x => x.Id == elementDto.Id.Value);
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

            var fieldsAdded = elementDto.ElementFields.Where(x => !x.Id.HasValue).Select(x => new ElementField
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

        public bool DeleteElement(int elementId)
        {
            var isUsedOnQuotes = _quotesDbContext.Elements.Any(x => x.QuoteElements.Any());
            if (isUsedOnQuotes) 
            {
                return false;
            }
            var element = _quotesDbContext.Elements.Find(elementId);
            _quotesDbContext.Elements.Remove(element);
            _quotesDbContext.SaveChanges();
            return true;
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
