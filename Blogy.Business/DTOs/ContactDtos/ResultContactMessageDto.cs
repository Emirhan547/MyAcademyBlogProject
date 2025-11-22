using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogy.Business.DTOs.ContactDtos
{
    public class ResultContactMessageDto
    {
        public int Id { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string Subject { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsReplied { get; set; }
    }
}
