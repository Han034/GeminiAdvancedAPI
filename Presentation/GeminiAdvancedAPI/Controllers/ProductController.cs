using AutoMapper;
using AutoMapper.QueryableExtensions;
using GeminiAdvancedAPI.Application.DTOs.PagedResult;
using GeminiAdvancedAPI.Application.Extensions;
using GeminiAdvancedAPI.Application.Features.Product.Commands.CreateProduct;
using GeminiAdvancedAPI.Application.Features.Product.Commands.DeleteProduct;
using GeminiAdvancedAPI.Application.Features.Product.Commands.UpdateProduct;
using GeminiAdvancedAPI.Application.Features.Product.Dtos;
using GeminiAdvancedAPI.Application.Features.Product.Queries.GetProduct;
using GeminiAdvancedAPI.Application.Features.Product.Queries.GetProducts;
using GeminiAdvancedAPI.Application.Interfaces;
using GeminiAdvancedAPI.Application.Services;
using GeminiAdvancedAPI.Extensions;
using GeminiAdvancedAPI.Persistence;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace GeminiAdvancedAPI.Controllers
{
    [ApiController]
	[Route("api/[controller]")]
	public class ProductController : ControllerBase
	{
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IQRCodeService _qrCodeService; // Ekledik
        private readonly IMemoryCache _memoryCache; // IMemoryCache'i ekleyin
        private readonly IUnitOfWork _unitOfWork; // IUnitOfWork field'ını ekleyin

        public ProductController(IMediator mediator, IMapper mapper, IQRCodeService qrCodeService, IMemoryCache memoryCache, IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _mapper = mapper;
            _qrCodeService = qrCodeService; // Ekledik
            _memoryCache = memoryCache; // IMemoryCache'i atayın
            _unitOfWork = unitOfWork;
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

        [HttpGet("GetProducts")] 
        //Cache mekanizmalı GET METODU
        public async Task<ActionResult<List<ProductDto>>> GetProducts()
        {
            const string cacheKey = "productsList";

            // Önbellekte veri var mı diye kontrol et
            if (_memoryCache.TryGetValue(cacheKey, out List<ProductDto> cachedProducts))
            {
                return Ok(cachedProducts); // Önbellekte varsa, önbellekteki veriyi dön
            }

            // Önbellekte yoksa, veritabanından çek
            var products = await _mediator.Send(new GetProductsQuery());
            var productDtos = _mapper.Map<List<ProductDto>>(products);

            // Önbelleğe ekle
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(10)) // 10 dakika sonra önbellekten silinsin (örnek)
                .SetAbsoluteExpiration(TimeSpan.FromHours(1))   // En fazla 1 saat sonra önbellekten silinsin (örnek)
                .SetPriority(CacheItemPriority.Normal)        // Önbellek dolduğunda öncelik sırasına göre silinir
                .SetSize(1024);
            _memoryCache.Set(cacheKey, productDtos, cacheEntryOptions);

            return Ok(productDtos);
        }

        [HttpGet("GetProductsPaged")]
        public async Task<ActionResult<PagedResult<ProductDto>>> GetProductsPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = _unitOfWork.Products.GetAll();
            var pagedResult = await query.ProjectTo<ProductDto>(_mapper.ConfigurationProvider).ToPagedResultAsync(pageNumber, pageSize);
            // Link header'ı ekle (eğer HttpResponseExtensions'ı oluşturduysanız)
            Response.AddPaginationHeader(pagedResult, Url, nameof(GetProductsPaged));
            return Ok(pagedResult);
        }

        [HttpPost]
		public async Task<ActionResult<Guid>> Create([FromBody] CreateProductCommand command)
		{
			var id = await _mediator.Send(command);
            _memoryCache.Remove("productsList"); // Önbelleği temizle
            return Ok(id);
		}

		[HttpPut]
		public async Task<ActionResult> Update([FromBody] UpdateProductCommand command)
		{
			await _mediator.Send(command);
            _memoryCache.Remove("productsList"); // Önbelleği temizle
            return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult> Delete(Guid id)
		{
			var command = new DeleteProductCommand(id);
			await _mediator.Send(command);
            _memoryCache.Remove("productsList"); // Önbelleği temizle
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
