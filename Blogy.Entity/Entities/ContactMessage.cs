using Blogy.Entity.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogy.Entity.Entities
{
    public class ContactMessage : BaseEntity
    {
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string DetectedLanguage { get; set; }
        public string AutoReply { get; set; }
        public DateTime RepliedDate { get; set; }
        public bool IsReplied { get; set; }
    }
}
