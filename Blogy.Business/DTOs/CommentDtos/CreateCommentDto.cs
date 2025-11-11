using Blogy.Business.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogy.Business.DTOs.CommentDtos
{
    public class CreateCommentDto
    {
        public string? Content { get; set; }
        public int? BlogId { get; set; }
        public int UserId { get; set; }
    }
}
