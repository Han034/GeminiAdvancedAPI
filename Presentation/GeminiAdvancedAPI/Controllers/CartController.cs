using GeminiAdvancedAPI.Application.DTOs;
using GeminiAdvancedAPI.Application.Features.Cart.Commands;
using GeminiAdvancedAPI.Application.Features.Cart.Commands.ClearCart;
using GeminiAdvancedAPI.Application.Features.Cart.Commands.RemoveProductFromCart;
using GeminiAdvancedAPI.Application.Features.Cart.Commands.UpdateCartItemQuantity;
using GeminiAdvancedAPI.Application.Features.Cart.Queries;
using GeminiAdvancedAPI.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeminiAdvancedAPI.Controllers
{
    public class CartController : Controller
    {
        private readonly IMediator _mediator; // Sadece IMediator'ı enjekte ediyoruz

        public CartController(IMediator mediator) // Sadece IMediator'ı alıyoruz
        {
            _mediator = mediator;
        }

        [HttpGet("GetItemCount")]
        [Authorize]
        public async Task<IActionResult> GetItemCount()
        {
            var userId = User.Identity.Name;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }
            var query = new GetCartItemCountQuery(userId);
            var itemCount = await _mediator.Send(query);
            return Ok(itemCount);
        }

        [HttpGet("GetCart")]
        [Authorize]
        public async Task<IActionResult> GetCart()
        {
            var userId = User.Identity.Name;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }
            var query = new GetCartByUserIdQuery(userId);
            var cartDto = await _mediator.Send(query);

            if (cartDto == null)
            {
                return Ok(new CartDto { UserId = userId, CartItems = new List<CartItemDto>() }); // Boş sepet dön
            }

            return Ok(cartDto);
        }

        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProductToCart([FromBody] AddProductToCartRequest request) // DTO kullanalım
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.Identity.Name;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }
            var command = new AddProductToCartCommand(userId, request.ProductId, request.Quantity);
            await _mediator.Send(command);
            return Ok();
        }

        [HttpDelete("RemoveProduct")]
        [Authorize]
        public async Task<IActionResult> RemoveProductFromCart([FromBody] RemoveProductFromCartRequest request)
        {
            if (!ModelState.IsValid) //ileride FluentValidation ile kontrol edeceğiz
            {
                return BadRequest(ModelState);
            }

            var userId = User.Identity.Name;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }
            var command = new RemoveProductFromCartCommand(userId, request.ProductId, request.Quantity);

            await _mediator.Send(command);

            return NoContent(); // 204 No Content döner
        }

        [HttpPut("UpdateQuantity")]
        [Authorize]
        public async Task<IActionResult> UpdateCartItemQuantity([FromBody] UpdateCartItemQuantityRequest request)
        {
            if (!ModelState.IsValid) // İleride FluentValidation ile kontrol edeceğiz.
            {
                return BadRequest(ModelState);
            }

            var userId = User.Identity.Name;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }
            var command = new UpdateCartItemQuantityCommand(userId, request.ProductId, request.Quantity);
            await _mediator.Send(command);
            return NoContent(); // 204 No Content döner
        }

        [HttpDelete("Clear")]
        [Authorize]
        public async Task<IActionResult> ClearCart()
        {
            var userId = User.Identity.Name;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }
            var command = new ClearCartCommand(userId);
            await _mediator.Send(command);
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
