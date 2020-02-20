using RoadStatus.Service.Exceptions;
using RoadStatus.Service.Interfaces;
using System;
using System.Text;
using System.Threading.Tasks;

namespace RoadStatus.Service
{
    public class PrintService : IPrintService
    {
        private readonly IRoadStatusService _roadStatusService;
        private readonly IConsoleWrapper _consoleWrapper;
        private StringBuilder _OutputMessage = new StringBuilder();

        public PrintService(IRoadStatusService roadStatusService, IConsoleWrapper consoleWrapper)
        {
            _roadStatusService = roadStatusService ?? throw new ArgumentNullException(nameof(roadStatusService));
            _consoleWrapper = consoleWrapper ?? throw new ArgumentNullException(nameof(consoleWrapper));
        }

        public async Task<int> PrintRoadStatusResponseAsync(string roadId)
        {
            if (string.IsNullOrEmpty(roadId))
            {
                _consoleWrapper.Write("Road id argument has NOT been passed. Command should be RoadStatus.exe [RoadId]");
                _OutputMessage.AppendLine("Road id argument has NOT been passed. Command should be RoadStatus.exe [RoadId]");
                return 1;
            }

            try
            {
                var roadStatus = await _roadStatusService.GetRoadStatusAsync(roadId);

                _consoleWrapper.Write($"The status of the {roadStatus.DisplayName} is as follows:");
                _consoleWrapper.Write($"Road Status is {roadStatus.StatusSeverity}");
                _consoleWrapper.Write($"Road Status Description is {roadStatus.StatusSeverityDescription}");

                _OutputMessage.AppendLine($"The status of the {roadStatus.DisplayName} is as follows:");
                _OutputMessage.AppendLine($"Road Status is {roadStatus.StatusSeverity}");
                _OutputMessage.AppendLine($"Road Status Description is {roadStatus.StatusSeverityDescription}");

                return 0;
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == 404)
                {
                    _consoleWrapper.Write($"{roadId} is not a valid road");
                    _OutputMessage.AppendLine($"{roadId} is not a valid road");
                }
                else
                {
                    //TODO: LOG with details from Exception
                    _consoleWrapper.Write($"There was an error running the application");
                    _OutputMessage.AppendLine($"There was an error running the application");
                }

                return 1;
            }
        }

        public void PrintOutPut()
        {
            _consoleWrapper.Write(_OutputMessage.ToString());
        }

        public string GetOutputMessage()
        {
            return _OutputMessage.ToString();
        }
    }
}