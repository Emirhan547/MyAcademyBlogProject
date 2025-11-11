using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogy.Business.DTOs.DashboardDtos
{
    public class MonthlyBlogStatisticsDto
    {
        public string Month { get; set; }
        public int BlogCount { get; set; }
    }
}
