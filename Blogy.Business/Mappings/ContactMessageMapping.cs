using AutoMapper;
using Blogy.Business.DTOs.CommentDtos;
using Blogy.Business.DTOs.ContactDtos;
using Blogy.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogy.Business.Mappings
{
    public class ContactMessageMapping:Profile
    {
        public ContactMessageMapping()
        {
            CreateMap<ContactMessage, ResultContactMessageDto>().ReverseMap();
            CreateMap<ContactMessage, GetContactMessageDetailDto>().ReverseMap();
            CreateMap<ContactMessage, CreateContactMessageDto>().ReverseMap();
        }
    }
}
