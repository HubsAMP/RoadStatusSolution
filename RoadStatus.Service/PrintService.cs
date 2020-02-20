using RoadStatus.Service.Exceptions;
using RoadStatus.Service.Interfaces;
using System;
using System.Threading.Tasks;

namespace RoadStatus.Service
{
    public class PrintService : IPrintService
    {
        private readonly IRoadStatusService _roadStatusService;

        public PrintService(IRoadStatusService roadStatusService)
        {
            _roadStatusService = roadStatusService ?? throw new ArgumentNullException(nameof(roadStatusService)); ;
        }

        public async Task<int> PrintRoadStatusResponseAsync(string roadId)
        {
            if (string.IsNullOrEmpty(roadId))
            {
                Console.WriteLine("Road id argument has NOT been passed. Command should be RoadStatus.exe [RoadId]");
                return 1;
            }

            try
            {
                var roadStatus = await _roadStatusService.GetRoadStatusAsync(roadId);

                Console.WriteLine($"The status of the {roadStatus.DisplayName} is as follows:");
                Console.WriteLine($"Road Status is {roadStatus.StatusSeverity}");
                Console.WriteLine($"Road Status Description is {roadStatus.StatusSeverityDescription}");

                return 0;
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == 404)
                {
                    Console.WriteLine($"{roadId} is not a valid road");
                }
                else
                {
                    //TODO: LOG with details from Exception
                    Console.WriteLine($"There was an error running the application");
                }

                return 1;
            }
        }
    }
}