using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogy.Business.DTOs.ContactDtos
{
    public class GetContactMessageDetailDto
    {
        public int Id { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string DetectedLanguage { get; set; }
        public string AutoReply { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsReplied { get; set; }
        public DateTime? RepliedDate { get; set; }
    }
}
