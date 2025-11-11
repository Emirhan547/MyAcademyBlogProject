using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogy.Business.Services.ToxicityServices
{
    public interface IToxicityService
    {
        Task<decimal> CheckToxicityAsync(string text);
        Task<bool> IsToxicAsync(string text, decimal threshold = 0.50m);
    }
}
