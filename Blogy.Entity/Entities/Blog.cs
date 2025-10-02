using Blogy.Entity.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogy.Entity.Entities
{
    public class Blog:BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string CoverImage { get; set; }
        public string BlogImage { get; set; }
        public string BlogImage2 { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public IList<BlogTag> BlogTags { get; set; }
    }
}
