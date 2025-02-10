using GeminiAdvancedAPI.Application.DTOs;
using GeminiAdvancedAPI.Application.Features.Cart.Commands;
using GeminiAdvancedAPI.Application.Features.Cart.Commands.ClearCart;
using GeminiAdvancedAPI.Application.Features.Cart.Commands.RemoveProductFromCart;
using GeminiAdvancedAPI.Application.Features.Cart.Commands.UpdateCartItemQuantity;
using GeminiAdvancedAPI.Application.Features.Cart.Queries;
using GeminiAdvancedAPI.Application.Interfaces.Repositories;
using GeminiAdvancedAPI.Persistence.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeminiAdvancedAPI.Controllers
{
    public class CartController : Controller
    {
        private readonly IMediator _mediator; // Sadece IMediator'ı enjekte ediyoruz
        private readonly ICartRepository _cartRepository;
        private const string CartCookieName = "MyShoppingCart"; // Cookie için bir isim

        public CartController(IMediator mediator, ICartRepository cartRepository) // Sadece IMediator'ı alıyoruz
        {
            _mediator = mediator;
            _cartRepository = cartRepository;
        }

        [HttpGet("GetItemCount")]
        [Authorize]
        public async Task<IActionResult> GetItemCount()
        {
            var userId = User.Identity.Name;
            if (string.IsNullOrEmpty(userId))
            {
                if (Request.Cookies.TryGetValue(CartCookieName, out string cartId))
                {
                    userId = cartId;
                }
                else
                {
                    return Ok(0);  // Cookie'de sepet yoksa, 0 döndür.
                }
            }
            var itemCount = await _cartRepository.GetCartItemCountAsync(userId);
            return Ok(itemCount);
        }

        [HttpPost("AddProduct")]
        [AllowAnonymous] // Giriş yapmamış kullanıcılar da sepete ürün ekleyebilsin
        public async Task<IActionResult> AddProductToCart([FromBody] AddProductToCartRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string userId = User.Identity.Name; // Önce giriş yapmış kullanıcıyı kontrol et

            // Eğer kullanıcı giriş yapmamışsa, cookie'den cartId'yi al
            if (string.IsNullOrEmpty(userId))
            {
                if (Request.Cookies.TryGetValue(CartCookieName, out string cartId))
                {
                    userId = cartId; // Cookie'deki cartId'yi userId olarak kullan
                }
                else
                {
                    // Cookie'de cartId yoksa, yeni bir cartId oluştur ve cookie'ye ekle
                    userId = Guid.NewGuid().ToString();
                    var cookieOptions = new CookieOptions { Expires = DateTime.Now.AddDays(7), HttpOnly = true, Secure = true, SameSite = SameSiteMode.Strict };
                    Response.Cookies.Append(CartCookieName, userId, cookieOptions);
                }
            }

            // Sepete ekleme command'ini oluştur (userId, productId, quantity)
            var command = new AddProductToCartCommand(userId, request.ProductId, request.Quantity);
            await _mediator.Send(command); // MediatR ile command'i işle

            return Ok();
        }

        [HttpGet("GetCart")]
        [AllowAnonymous] //Artık GetCart'ta yetkilendirme istemiyoruz.
        public async Task<IActionResult> GetCart()
        {
            var userId = User.Identity.Name;

            if (string.IsNullOrEmpty(userId))
            {
                // Cookie'den oku
                if (Request.Cookies.TryGetValue(CartCookieName, out string cartId))
                {
                    userId = cartId;
                }
                else
                {
                    // Cookie'de sepet yoksa boş sepet dön
                    return Ok(new CartDto { CartItems = new List<CartItemDto>() });
                }
            }

            var cartDto = await _cartRepository.GetByUserIdAsync(userId);

            if (cartDto == null)
            {
                return Ok(new CartDto { CartItems = new List<CartItemDto>() });
            }

            return Ok(cartDto);
        }

        [HttpDelete("RemoveProduct")]
        [Authorize]
        public async Task<IActionResult> RemoveProductFromCart([FromBody] RemoveProductFromCartRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userId = User.Identity.Name;
            if (string.IsNullOrEmpty(userId))
            {
                if (Request.Cookies.TryGetValue(CartCookieName, out string cartId))
                {
                    userId = cartId;
                }
                else
                {
                    return NotFound("Cart Not Found");
                }
            }

            try
            {
                await _cartRepository.RemoveItemFromCartAsync(userId, request.ProductId, request.Quantity);
                return NoContent(); // 204 No Content
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("UpdateQuantity")]
        [Authorize]
        public async Task<IActionResult> UpdateCartItemQuantity([FromBody] UpdateCartItemQuantityRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userId = User.Identity.Name;
            if (string.IsNullOrEmpty(userId))
            {
                if (Request.Cookies.TryGetValue(CartCookieName, out string cartId))
                {
                    userId = cartId;
                }
                else
                {
                    return NotFound("Cart Not Found");
                }
            }

            try
            {
                await _cartRepository.UpdateCartItemQuantityAsync(userId, request.ProductId, request.Quantity);
                return NoContent(); // 204 No Content
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("Clear")]
        [Authorize]
        public async Task<IActionResult> ClearCart()
        {
            var userId = User.Identity.Name;
            if (string.IsNullOrEmpty(userId))
            {
                if (Request.Cookies.TryGetValue(CartCookieName, out string cartId))
                {
                    userId = cartId;
                }
                else
                {
                    return NotFound("Cart Not Found"); // Cookie'de sepet yoksa 404 dön
                }
            }
            await _cartRepository.ClearCartAsync(userId);
            return NoContent(); // 204 No Content döner
        }

        // DTO'yu tanımlayın:
        public class UpdateCartItemQuantityRequest
        {
            public Guid ProductId { get; set; }
            public int Quantity { get; set; }
        }

        public class RemoveProductFromCartRequest
        {
            public Guid ProductId { get; set; }
            public int Quantity { get; set; }
        }

        // AddProductToCartRequest DTO'sunu tanımlayın:
        public class AddProductToCartRequest
        {
            public Guid ProductId { get; set; }
            public int Quantity { get; set; }
        }

        
    }
}
