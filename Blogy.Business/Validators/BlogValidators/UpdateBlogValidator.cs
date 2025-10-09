using Blogy.Business.DTOs.BlogDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogy.Business.Validators.BlogValidators
{
    public class UpdateBlogValidator:AbstractValidator<UpdateBlogDto>
    {
        public UpdateBlogValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Başlık Boş bırakılamaz");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Açıklama Boş bırakılamaz");
            RuleFor(x => x.CoverImage).NotEmpty().WithMessage("Kapak Resmi Boş bırakılamaz");
            RuleFor(x => x.BlogImage).NotEmpty().WithMessage("Blog Resmi1 Boş bırakılamaz");
            RuleFor(x => x.BlogImage2).NotEmpty().WithMessage("Blog Resmi2 bırakılamaz");
            RuleFor(x => x.CategoryId).NotEmpty().WithMessage("Kategori Boş bırakılamaz");
        }
    }
}
