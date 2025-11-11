using Blogy.Business.DTOs.BlogDtos;
using FluentValidation;

namespace Blogy.Business.Validators.BlogValidators
{
    public class UpdateBlogValidator : AbstractValidator<UpdateBlogDto>
    {
        public UpdateBlogValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Başlık boş bırakılamaz");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Açıklama boş bırakılamaz");
            RuleFor(x => x.CoverImage).NotEmpty().WithMessage("Kapak Resmi boş bırakılamaz");
            RuleFor(x => x.BlogImage1).NotEmpty().WithMessage("Blog Resmi 1 boş bırakılamaz");
            RuleFor(x => x.BlogImage2).NotEmpty().WithMessage("Blog Resmi 2 boş bırakılamaz");
            RuleFor(x => x.CategoryId).NotEmpty().WithMessage("Kategori boş bırakılamaz");
        }
    }
}