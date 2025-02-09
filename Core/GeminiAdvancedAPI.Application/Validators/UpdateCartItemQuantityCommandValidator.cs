using FluentValidation;
using GeminiAdvancedAPI.Application.Features.Cart.Commands.UpdateCartItemQuantity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.Validators
{
    public class UpdateCartItemQuantityCommandValidator : AbstractValidator<UpdateCartItemQuantityCommand>
    {
        public UpdateCartItemQuantityCommandValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("Product ID is required.");

            RuleFor(x => x.Quantity)
                .GreaterThanOrEqualTo(0).WithMessage("Quantity must be greater than or equal to 0."); // Miktar 0 veya daha büyük olmalı
        }
    }
}
