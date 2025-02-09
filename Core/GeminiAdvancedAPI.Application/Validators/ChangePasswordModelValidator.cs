using FluentValidation;
using GeminiAdvancedAPI.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.Validators
{
    public class ChangePasswordModelValidator : AbstractValidator<ChangePasswordModel>
    {
        public ChangePasswordModelValidator()
        {
            RuleFor(x => x.CurrentPassword).NotEmpty().WithMessage("Current password is required.");
            RuleFor(x => x.NewPassword).NotEmpty().WithMessage("New password is required.")
                                        .MinimumLength(6).WithMessage("Password must be at least 6 characters.")
                                        .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                                        .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                                        .Matches("[0-9]").WithMessage("Password must contain at least one digit.")
                                        .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one non-alphanumeric character.");
        }
    }
}
