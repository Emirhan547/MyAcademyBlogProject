using Blogy.Entity.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogy.Entity.Entities
{
    public class BlogTag: BaseEntity
    {
        public int TagId { get; set; }
        public int BlogId { get; set; }
        public Blog Blog { get; set; }
        public Tag Tag { get; set; }
       
    }
}
