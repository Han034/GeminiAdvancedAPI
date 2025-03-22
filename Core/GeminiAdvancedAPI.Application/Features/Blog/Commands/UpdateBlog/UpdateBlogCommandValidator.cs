using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.Features.Blog.Commands.UpdateBlog
{
    public class UpdateBlogCommandValidator : AbstractValidator<UpdateBlogCommand>
    {
        public UpdateBlogCommandValidator()
        {

            RuleFor(x => x.Id)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotNull()
            .NotEqual(Guid.Empty).WithMessage("{PropertyName} is required"); //Guid boş olamaz.


            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(200).WithMessage("{PropertyName} must not exceed 200 characters.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();


            RuleFor(x => x.PublishedDate)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();
        }

    }
}
