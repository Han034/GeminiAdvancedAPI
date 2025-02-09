﻿using AutoMapper;
using GeminiAdvancedAPI.Application.DTOs;
using GeminiAdvancedAPI.Application.Features.Product.Commands.CreateProduct;
using GeminiAdvancedAPI.Application.Features.Product.Commands.DeleteProduct;
using GeminiAdvancedAPI.Application.Features.Product.Commands.UpdateProduct;
using GeminiAdvancedAPI.Application.Features.Product.Dtos;
using GeminiAdvancedAPI.Application.Features.Product.Queries.GetProduct;
using GeminiAdvancedAPI.Application.Features.Product.Queries.GetProducts;
using GeminiAdvancedAPI.Application.Interfaces;
using GeminiAdvancedAPI.Application.Services;
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
        private readonly IQRCodeService _qrCodeService; // Ekledik

        public ProductController(IMediator mediator, IMapper mapper, IQRCodeService qrCodeService)
        {
            _mediator = mediator;
            _mapper = mapper;
            _qrCodeService = qrCodeService; // Ekledik
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

        [HttpGet("GetProductWithQrCode/{id}")]
        public async Task<IActionResult> GetProductWithQrCode(Guid id)
        {
            var productDto = await _mediator.Send(new GetProductQuery(id));

            if (productDto == null)
            {
                return NotFound();
            }

            // Ürün bilgilerini içeren bir string oluştur (örneğin, JSON formatında)
            var qrText = $"ProductId: {productDto.Id}, Name: {productDto.Name}, Price: {productDto.Price}";
            // İsterseniz, daha fazla bilgiyi JSON formatında ekleyebilirsiniz:
            // var qrText = System.Text.Json.JsonSerializer.Serialize(productDto);

            var qrCodeBytes = _qrCodeService.GenerateQrCode(qrText);

            // QR kodu ve ürün bilgilerini içeren bir obje dön
            return Ok(new
            {
                Product = productDto,
                QrCode = Convert.ToBase64String(qrCodeBytes) // Base64'e çevirerek dönüyoruz
            });

            // Alternatif olarak, QR kodu direkt olarak bir image olarak da dönebilirsiniz:
            // return File(qrCodeBytes, "image/png");
        }
        [HttpGet("GetProductWithQrCode2/{id}")]
        public async Task<IActionResult> GetProductWithQrCode2(Guid id)
        {
            var productDto = await _mediator.Send(new GetProductQuery(id));

            if (productDto == null)
            {
                return NotFound();
            }

            // Ürün bilgilerini içeren bir string oluştur (örneğin, JSON formatında)
            var qrText = $"ProductId: {productDto.Id}, Name: {productDto.Name}, Price: {productDto.Price}";

            var qrCodeBytes = _qrCodeService.GenerateQrCode(qrText);

            // QR kodu bir resim olarak dön
            return File(qrCodeBytes, "image/png");
        }
    }
}
