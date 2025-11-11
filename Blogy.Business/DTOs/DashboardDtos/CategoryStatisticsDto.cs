using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogy.Business.DTOs.DashboardDtos
{
    public class CategoryStatisticsDto
    {
        public string CategoryName { get; set; }
        public int BlogCount { get; set; }
    }
}
