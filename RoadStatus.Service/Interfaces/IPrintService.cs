using System.Threading.Tasks;

namespace RoadStatus.Service.Interfaces
{
    public interface IPrintService
    {
        Task<int> PrintRoadStatusResponseAsync(string roadId);

        void PrintOutPut();

        string GetOutputMessage();
    }
}