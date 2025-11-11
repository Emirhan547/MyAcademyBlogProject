using Blogy.Business.DTOs.DashboardDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogy.Business.Services.DashboardServices
{
    public interface IDashboardService
    {
        Task<DashboardStatisticsDto> GetStatisticsAsync();
    }
}
