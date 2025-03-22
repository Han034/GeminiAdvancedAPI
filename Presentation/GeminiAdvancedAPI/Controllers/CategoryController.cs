using GeminiAdvancedAPI.Application.Features.Category.Commands.CreateCategory;
using GeminiAdvancedAPI.Application.Features.Category.Commands.DeleteCategory;
using GeminiAdvancedAPI.Application.Features.Category.Commands.UpdateCategory;
using GeminiAdvancedAPI.Application.Features.Category.Queries.GetCategories;
using GeminiAdvancedAPI.Application.Features.Category.Queries.GetCategoryById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GeminiAdvancedAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetCategoriesQuery();
            var categories = await _mediator.Send(query);
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetCategoryByIdQuery(id);
            var category = await _mediator.Send(query);
            if (category == null)
            {
                return NotFound(); // 404 Not Found
            }
            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryCommand command)
        {
            var categoryId = await _mediator.Send(command);
            return Ok(categoryId); // Veya CreatedAtAction kullanabilirsiniz
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCategoryCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("ID mismatch");
            }
            await _mediator.Send(command);
            return NoContent(); // 204 No Content
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteCategoryCommand(id);
            await _mediator.Send(command);
            return NoContent(); // 204 No Content
        }
    }
}
