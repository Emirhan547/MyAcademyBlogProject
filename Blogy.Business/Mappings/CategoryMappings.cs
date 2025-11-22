using AutoMapper;
using Blogy.Business.DTOs.CategoryDtos;
using Blogy.Entity.Entities;

namespace Blogy.Business.Mappings
{
    public class CategoryMappings : Profile
    {
        public CategoryMappings()
        {

            //source=> kaynak destination=> hedef
            CreateMap<Category, ResultCategoryDto>().ReverseMap();
    
            CreateMap<Category, UpdateCategoryDto>().ReverseMap();
            CreateMap<Category, CreateCategoryDto>().ReverseMap();
        }
    }
}