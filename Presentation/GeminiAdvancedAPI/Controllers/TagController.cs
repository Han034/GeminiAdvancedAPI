using GeminiAdvancedAPI.Application.Features.Tag.Commands.CreateTag;
using GeminiAdvancedAPI.Application.Features.Tag.Commands.DeleteTag;
using GeminiAdvancedAPI.Application.Features.Tag.Commands.UpdateTag;
using GeminiAdvancedAPI.Application.Features.Tag.Queries.GetTagById;
using GeminiAdvancedAPI.Application.Features.Tag.Queries.GetTags;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GeminiAdvancedAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TagController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TagController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetTagsQuery();
            var tags = await _mediator.Send(query);
            return Ok(tags);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetTagByIdQuery(id);
            var tag = await _mediator.Send(query);
            if (tag == null)
            {
                return NotFound(); // 404 Not Found
            }
            return Ok(tag);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTagCommand command)
        {
            var tagId = await _mediator.Send(command);
            return Ok(tagId); // Veya CreatedAtAction kullanabilirsiniz
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTagCommand command)
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
            var command = new DeleteTagCommand(id);
            await _mediator.Send(command);
            return NoContent(); // 204 No Content
        }

    }
}
