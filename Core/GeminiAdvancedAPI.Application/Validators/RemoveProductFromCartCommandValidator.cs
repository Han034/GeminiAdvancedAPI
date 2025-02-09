using FluentValidation;
using GeminiAdvancedAPI.Application.Features.Cart.Commands.RemoveProductFromCart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.Validators
{
    public class RemoveProductFromCartCommandValidator : AbstractValidator<RemoveProductFromCartCommand>
    {
        public RemoveProductFromCartCommandValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("Product ID is required.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0."); // Miktar 0'dan büyük olmalı
        }
    }
}
