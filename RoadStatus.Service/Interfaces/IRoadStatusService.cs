using RoadStatus.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadStatus.Service.Interfaces
{
    public interface IRoadStatusService
    {
        Task<RoadStatusDto> GetRoadStatusAsync(string roadId);
    }
}