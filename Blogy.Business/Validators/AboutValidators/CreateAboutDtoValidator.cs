using Blogy.Business.DTOs.AboutDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogy.Business.Validators.AboutValidators
{
    public class CreateAboutDtoValidator : AbstractValidator<CreateAboutDto>
    {
        public CreateAboutDtoValidator()
        {
            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Hakkımızda içeriği boş bırakılamaz.")
                .MinimumLength(20).WithMessage("Hakkımızda içeriği en az 20 karakter olmalıdır.")
                .MaximumLength(2000).WithMessage("Hakkımızda içeriği en fazla 2000 karakter olabilir.");
        }
    }
}
