using Blogy.Business.DTOs.CategoryDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogy.Business.Validators.CategoryValidators
{
    public class UpateCategoryValidator:AbstractValidator<UpdateCategoryDto>
    {
        public UpateCategoryValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Kategori Adı boş bırakılamaz.")
                .MinimumLength(3).WithMessage("Kategori Adı en az 3 karakterli olmalı")
                .MaximumLength(50).WithMessage("Kategori Adı en fazla 50 karakter olmalıdır");
        }
    }
}
