using GeminiAdvancedAPI.Application.Features.Product.Commands.CreateProduct;
using GeminiAdvancedAPI.Application.Features.Product.Commands.DeleteProduct;
using GeminiAdvancedAPI.Application.Features.Product.Commands.UpdateProduct;
using GeminiAdvancedAPI.Application.Features.Product.Dtos;
using GeminiAdvancedAPI.Application.Features.Product.Queries.GetProduct;
using GeminiAdvancedAPI.Application.Features.Product.Queries.GetProducts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GeminiAdvancedAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ProductController : ControllerBase
	{
		private readonly IMediator _mediator;

		public ProductController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<ProductDto>> Get(Guid id)
		{
			var query = new GetProductQuery(id);
			var product = await _mediator.Send(query);
			return Ok(product);
		}

		[HttpGet]
		public async Task<ActionResult<List<ProductDto>>> GetList()
		{
			var query = new GetProductsQuery();
			var products = await _mediator.Send(query);
			return Ok(products);
		}

		[HttpPost]
		public async Task<ActionResult<Guid>> Create([FromBody] CreateProductCommand command)
		{
			var id = await _mediator.Send(command);
			return Ok(id);
		}

		[HttpPut]
		public async Task<ActionResult> Update([FromBody] UpdateProductCommand command)
		{
			await _mediator.Send(command);
			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult> Delete(Guid id)
		{
			var command = new DeleteProductCommand(id);
			await _mediator.Send(command);
			return NoContent();
		}
	}
}
