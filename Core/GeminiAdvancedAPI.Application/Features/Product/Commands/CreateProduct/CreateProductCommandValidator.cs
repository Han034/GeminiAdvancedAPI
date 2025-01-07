using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.Features.Product.Commands.CreateProduct
{
	public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
	{
		public CreateProductCommandValidator()
		{
			RuleFor(p => p.Name).NotEmpty().WithMessage("{PropertyName} is required.").NotNull().MaximumLength(50)
				.WithMessage("{PropertyName} must not exceed 50 characters.");
			RuleFor(p => p.Description).MaximumLength(500);
			RuleFor(p => p.Price).GreaterThan(0).WithMessage("{PropertyName} should be greater than zero.");
		}
	}
}
