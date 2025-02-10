using AutoMapper;
using GeminiAdvancedAPI.Application.DTOs;
using GeminiAdvancedAPI.Application.Features.Cart.Commands;
using GeminiAdvancedAPI.Application.Interfaces;
using GeminiAdvancedAPI.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;


namespace GeminiAdvancedAPI.Application.Features.Product.Commands.CreateProduct
{
    public class AddProductToCartCommandHandler : IRequestHandler<AddProductToCartCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public AddProductToCartCommandHandler(ICartRepository cartRepository, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, IProductRepository productRepository)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
            _productRepository = productRepository;
            _cartRepository = cartRepository;
            _httpContextAccessor = httpContextAccessor; // Inject etmeyi unutmayın!

        }

        public async Task Handle(AddProductToCartCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext.User.Identity.Name;
            if (string.IsNullOrEmpty(userId))
            {
                // Giriş yapmış kullanıcı yoksa, cookie'den sepet ID'sini al
                if (_httpContextAccessor.HttpContext.Request.Cookies.TryGetValue("MyShoppingCart", out string cartId))
                {
                    userId = cartId;
                }
                else
                {
                    // Cookie'de sepet ID'si yoksa, yeni bir tane oluştur ve cookie'ye ekle
                    userId = Guid.NewGuid().ToString();
                    _httpContextAccessor.HttpContext.Response.Cookies.Append("MyShoppingCart", userId, new CookieOptions { Expires = DateTimeOffset.Now.AddDays(7) });
                }
            }
            var product = await _productRepository.GetByIdAsync(request.ProductId);
            if (product == null)
            {
                throw new KeyNotFoundException("Product not found."); // Veya uygun bir exception
            }

            var cartDto = await _cartRepository.GetByUserIdAsync(request.UserId);
            if (cartDto == null)
            {
                // Yeni bir CartDto oluştur
                cartDto = new CartDto { UserId = request.UserId, CartItems = new List<CartItemDto>() };

            }

            var cartItem = cartDto.CartItems.FirstOrDefault(ci => ci.ProductId == request.ProductId);

            if (cartItem == null)
            {
                cartItem = new CartItemDto
                {
                    ProductId = request.ProductId,
                    ProductName = product.Name, // Ürün adını DTO'ya ekliyoruz.
                    Quantity = request.Quantity,
                    Price = product.Price
                };
                cartDto.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity += request.Quantity;
            }

            await _cartRepository.AddOrUpdateCartAsync(cartDto);

        }
    }
}
