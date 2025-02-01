using AutoMapper;
using GeminiAdvancedAPI.Application.DTOs;
using GeminiAdvancedAPI.Application.Features.Product.Commands.CreateProduct;
using GeminiAdvancedAPI.Application.Features.Product.Commands.DeleteProduct;
using GeminiAdvancedAPI.Application.Features.Product.Commands.UpdateProduct;
using GeminiAdvancedAPI.Application.Features.Product.Dtos;
using GeminiAdvancedAPI.Application.Features.Product.Queries.GetProduct;
using GeminiAdvancedAPI.Application.Features.Product.Queries.GetProducts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GeminiAdvancedAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ProductController : ControllerBase
	{
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public ProductController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
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

        [HttpGet("GetProductsPaged")]
        public async Task<ActionResult<PagedResult<ProductDto>>> GetProductsPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            if (pageNumber <= 0)
            {
                pageNumber = 1;
            }

            if (pageSize <= 0)
            {
                pageSize = 10;
            }

            var products = await _mediator.Send(new GetProductsQuery());
            var totalCount = products.Count;
            var pagedProducts = products.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            var productDtos = _mapper.Map<List<ProductDto>>(pagedProducts);

            var pagedResult = new PagedResult<ProductDto>(productDtos, pageNumber, pageSize, totalCount);

            return Ok(pagedResult);
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
