using Blogy.Business.DTOs.CategoryDtos;
using FluentValidation;

namespace Blogy.Business.Validators.CategoryValidators
{
    public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryDto>
    {
        public UpdateCategoryValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Kategori Adı boş bırakılamaz")
                               .MinimumLength(3).WithMessage("Kategori Adı en az 3 karakter olmalıdır.")
                               .MaximumLength(50).WithMessage("Kategori Adı en fazla 50 karakter olmalıdır.");
        }
    }
}