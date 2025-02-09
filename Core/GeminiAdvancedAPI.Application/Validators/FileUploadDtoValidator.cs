using FluentValidation;
using GeminiAdvancedAPI.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.Validators
{
    public class FileUploadDtoValidator : AbstractValidator<FileUploadDto>
    {
        public FileUploadDtoValidator()
        {
            RuleFor(x => x.File)
                .NotEmpty().WithMessage("File is not selected");
        }
    }
}
