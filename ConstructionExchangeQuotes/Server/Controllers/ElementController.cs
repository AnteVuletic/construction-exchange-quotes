using ConstructionExchangeQuotes.Server.Repositories;
using ConstructionExchangeQuotes.Shared;
using Microsoft.AspNetCore.Mvc;

namespace ConstructionExchangeQuotes.Server.Controllers
{
    [ApiController]
    [Route("element")]
    public class ElementController : ControllerBase
    {
        private readonly ElementRepository _elementRepository;

        public ElementController(ElementRepository elementRepository)
        {
            _elementRepository = elementRepository;
        }

        [HttpGet("get-categories")]
        public IActionResult GetCategories()
        {
            return Ok(_elementRepository.GetElementCategories());
        }

        [HttpGet("get-element-types")]
        public IActionResult GetElementTypes()
        {
            return Ok(_elementRepository.GetElementTypes());
        }

        [HttpPost("add-element")]
        public IActionResult AddElement(ElementDto elementDto)
        {
            var isValid = _elementRepository.AddElement(elementDto);
            if (!isValid)
                return BadRequest("Element invalid");
            return Ok();
        }

        [HttpPost("add-category")]
        public IActionResult AddCategory(ElementCategoryDto elementCategoryDto)
        {
            _elementRepository.AddElementCategory(elementCategoryDto);
            return Ok();
        }

        [HttpPost("add-type")]
        public IActionResult AddType(ElementTypeDto elementTypeDto)
        {
            _elementRepository.AddElementType(elementTypeDto);
            return Ok();
        }

        [HttpDelete("delete-element/{elementId:int}")]
        public IActionResult DeleteElement(int elementId)
        {
            _elementRepository.DeleteElement(elementId);
            return Ok();
        }

        [HttpDelete("delete-category/{categoryId:int}")]
        public IActionResult DeleteCategory(int categoryId)
        {
            var isSuccessful = _elementRepository.DeleteElementCategory(categoryId);
            if (!isSuccessful)
                return BadRequest("Category used on elements");
            return Ok();
        }

        [HttpDelete("delete-type/{typeId:int}")]
        public IActionResult DeleteElementType(int typeId)
        {
            var isSuccessful = _elementRepository.DeleteElementType(typeId);
            if (!isSuccessful)
                return BadRequest("Element type used on elements");
            return Ok();
        }

        [HttpPost("edit-element")]
        public IActionResult EditElement(ElementDto elementDto)
        {
            var isValid = _elementRepository.EditElement(elementDto);
            if (!isValid)
                return BadRequest("Element invalid");
            return Ok();
        }

        [HttpPost("edit-category")]
        public IActionResult UpdateCategory(ElementCategoryDto elementCategoryDto)
        {
            _elementRepository.EditElementCategory(elementCategoryDto);
            return Ok();
        }

        [HttpPost("edit-type")]
        public IActionResult UpdateType(ElementTypeDto elementTypeDto)
        {
            _elementRepository.EditElementType(elementTypeDto);
            return Ok();
        }
    }
}
