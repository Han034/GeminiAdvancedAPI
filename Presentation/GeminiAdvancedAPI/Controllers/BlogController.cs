using GeminiAdvancedAPI.Application.Features.Blog.Commands.CreateBlog;
using GeminiAdvancedAPI.Application.Features.Blog.Commands.DeleteBlog;
using GeminiAdvancedAPI.Application.Features.Blog.Commands.UpdateBlog;
using GeminiAdvancedAPI.Application.Features.Blog.Queries.GetBlogById;
using GeminiAdvancedAPI.Application.Features.Blog.Queries.GetBlogs;
using GeminiAdvancedAPI.Application.Features.Blog.Queries.GetBlogsByCategory;
using GeminiAdvancedAPI.Application.Features.Blog.Queries.GetBlogsByTag;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GeminiAdvancedAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlogController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BlogController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBlogCommand command)
        {
            var blogId = await _mediator.Send(command);
            return Ok(blogId); // Veya CreatedAtAction kullanabilirsiniz
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var blogs = await _mediator.Send(new GetBlogsQuery());
            return Ok(blogs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var blog = await _mediator.Send(new GetBlogByIdQuery(id));
            if (blog == null)
            {
                return NotFound();
            }
            return Ok(blog);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateBlogCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("ID mismatch");
            }
            await _mediator.Send(command);
            return NoContent(); // 204 No Content
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteBlogCommand(id));
            return NoContent(); // 204 No Content
        }

        [HttpGet("category/{categoryId}")] // Yeni eklenen
        public async Task<IActionResult> GetByCategory(int categoryId)
        {
            var blogs = await _mediator.Send(new GetBlogsByCategoryQuery(categoryId));
            return Ok(blogs);
        }

        [HttpGet("tag/{tagId}")] // Yeni eklenen
        public async Task<IActionResult> GetByTag(int tagId)
        {
            var blogs = await _mediator.Send(new GetBlogsByTagQuery(tagId));
            return Ok(blogs);
        }
    }
}
