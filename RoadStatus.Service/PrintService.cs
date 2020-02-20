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
        private StringBuilder _outputMessage = new StringBuilder();

        public PrintService(IRoadStatusService roadStatusService, IConsoleWrapper consoleWrapper)
        {
            _roadStatusService = roadStatusService ?? throw new ArgumentNullException(nameof(roadStatusService));
            _consoleWrapper = consoleWrapper ?? throw new ArgumentNullException(nameof(consoleWrapper));
        }

        public async Task<int> PrintRoadStatusResponseAsync(string roadId)
        {
            if (string.IsNullOrEmpty(roadId))
            {
                _outputMessage.AppendLine("Road id argument has NOT been passed. Command should be RoadStatus.exe [RoadId]");
                return 1;
            }

            try
            {
                var roadStatus = await _roadStatusService.GetRoadStatusAsync(roadId);

                _outputMessage.AppendLine($"The status of the {roadStatus.DisplayName} is as follows:");
                _outputMessage.AppendLine($"Road Status is {roadStatus.StatusSeverity}");
                _outputMessage.AppendLine($"Road Status Description is {roadStatus.StatusSeverityDescription}");

                return 0;
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == 404)
                {
                    _outputMessage.AppendLine($"{roadId} is not a valid road");
                }
                else
                {
                    //TODO: LOG with details from Exception
                    _outputMessage.AppendLine($"There was an error running the application");
                }

                return 1;
            }
        }

        public void PrintOutPut()
        {
            _consoleWrapper.Write(_outputMessage.ToString());
        }

        public string GetOutputMessage()
        {
            return _outputMessage.ToString();
        }
    }
}