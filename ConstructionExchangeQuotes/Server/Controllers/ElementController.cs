using ConstructionExchangeQuotes.Server.Repositories;
using ConstructionExchangeQuotes.Server.Utils;
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
        public IActionResult GetCategories([FromQuery] string name)
        {
            return Ok(_elementRepository.GetElementCategories(name));
        }

        [HttpGet("get-element-types")]
        public IActionResult GetElementTypes([FromQuery] string name)
        {
            return Ok(_elementRepository.GetElementTypes(name));
        }

        [HttpGet("get")]
        public IActionResult GetElements([FromQuery] string name, [FromQuery] int categoryId, [FromQuery] int typeId)
        {
            return Ok(_elementRepository.GetElements(name, categoryId, typeId));
        }

        [CrudPermission]
        [HttpPost("add-element")]
        public IActionResult AddElement(ElementDto elementDto)
        {
            var isValid = _elementRepository.AddElement(elementDto);
            if (!isValid)
                return BadRequest("Element invalid");
            return Ok();
        }

        [CrudPermission]
        [HttpPost("add-category")]
        public IActionResult AddCategory(ElementCategoryDto elementCategoryDto)
        {
            _elementRepository.AddElementCategory(elementCategoryDto);
            return Ok();
        }

        [CrudPermission]
        [HttpPost("add-type")]
        public IActionResult AddType(ElementTypeDto elementTypeDto)
        {
            _elementRepository.AddElementType(elementTypeDto);
            return Ok();
        }

        [CrudPermission]
        [HttpDelete("delete-element/{elementId:int}")]
        public IActionResult DeleteElement(int elementId)
        {
            var isSuccessful = _elementRepository.DeleteElement(elementId);
            if (!isSuccessful)
                return BadRequest();
            return Ok();
        }

        [CrudPermission]
        [HttpDelete("delete-category/{categoryId:int}")]
        public IActionResult DeleteCategory(int categoryId)
        {
            var isSuccessful = _elementRepository.DeleteElementCategory(categoryId);
            if (!isSuccessful)
                return BadRequest("Category used on elements");
            return Ok();
        }

        [CrudPermission]
        [HttpDelete("delete-type/{typeId:int}")]
        public IActionResult DeleteElementType(int typeId)
        {
            var isSuccessful = _elementRepository.DeleteElementType(typeId);
            if (!isSuccessful)
                return BadRequest("Element type used on elements");
            return Ok();
        }

        [CrudPermission]
        [HttpPost("edit-element")]
        public IActionResult EditElement(ElementDto elementDto)
        {
            var isValid = _elementRepository.EditElement(elementDto);
            if (!isValid)
                return BadRequest("Element invalid");
            return Ok();
        }

        [CrudPermission]
        [HttpPost("edit-category")]
        public IActionResult UpdateCategory(ElementCategoryDto elementCategoryDto)
        {
            _elementRepository.EditElementCategory(elementCategoryDto);
            return Ok();
        }

        [CrudPermission]
        [HttpPost("edit-type")]
        public IActionResult UpdateType(ElementTypeDto elementTypeDto)
        {
            _elementRepository.EditElementType(elementTypeDto);
            return Ok();
        }
    }
}
