using Blogy.Business.DTOs.BlogDtos;
using Blogy.Business.DTOs.Common;
using Blogy.Business.DTOs.UserDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogy.Business.DTOs.CommentDtos
{
    public class UpdateCommentDto:BaseDto
    {
        public string? Content { get; set; }
        public int BlogId { get; set; }
        public int UserId { get; set; }
        public bool IsToxic { get; set; }
    }
}
