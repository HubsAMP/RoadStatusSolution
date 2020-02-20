using System;
using System.IO;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using RoadStatus.Client;
using RoadStatus.Service;
using RoadStatus.Service.Exceptions;
using RoadStatus.Service.Interfaces;
using RoadStatus.Service.Models;

namespace RoadStatus.Test
{
    [TestFixture]
    public class RoadStatusConsoleClientTest
    {
        private RoadStatusDto _validRoadStatus;
        private Mock<IRoadStatusService> _mockRoadStatusService;
        private Mock<PrintService> _mockPrintService;

        [SetUp]
        public void Setup()
        {
            _validRoadStatus = new RoadStatusDto
            {
                Id = "a2",
                DisplayName = "A2",
                StatusSeverity = "Good",
                StatusSeverityDescription = "No Exceptional Delays"
            };

            _mockRoadStatusService = new Mock<IRoadStatusService>();
            _mockPrintService = new Mock<PrintService>(_mockRoadStatusService.Object);
        }

        [Test]
        public async Task GivenValidRoadId_IsSpecified_WhenTheClientIsRunThen_TheRoadDisplayName_ShouldBeDisplayedAsync()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                _mockRoadStatusService.Setup(s => s.GetRoadStatusAsync("A2")).Returns(Task.FromResult(_validRoadStatus));

                var response = await _mockPrintService.Object.PrintRoadStatusResponseAsync("A2");

                string expected = $"The status of the {_validRoadStatus.DisplayName} is as follows:";

                Assert.IsTrue(sw.ToString().Contains(expected));
            }
        }

        [Test]
        public async Task GivenValidRoadId_IsSpecified_WhenTheClientIsRunThen_TheStatusSeverity_ShouldBeDisplayedAsync()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                var mockRoadStatusService = new Mock<IRoadStatusService>();
                mockRoadStatusService.Setup(s => s.GetRoadStatusAsync(It.IsAny<string>())).Returns(Task.FromResult(_validRoadStatus));

                RoadStatusPrinter printer = new RoadStatusPrinter(mockRoadStatusService.Object);

                var response = await printer.PrintRoadStatusResponseAsync("A2");

                string expected = $"Road Status is {_validRoadStatus.StatusSeverity}";

                Assert.IsTrue(sw.ToString().Contains(expected));
            }
        }

        [Test]
        public async Task GivenValidRoadId_IsSpecified_WhenTheClientIsRunThen_TheStatusSeverityDescription_ShouldBeDisplayedAsync()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                var mockRoadStatusService = new Mock<IRoadStatusService>();
                mockRoadStatusService.Setup(s => s.GetRoadStatusAsync(It.IsAny<string>())).Returns(Task.FromResult(_validRoadStatus));

                RoadStatusPrinter printer = new RoadStatusPrinter(mockRoadStatusService.Object);

                var response = await printer.PrintRoadStatusResponseAsync("A2");

                string expected = $"Road Status Description is {_validRoadStatus.StatusSeverityDescription}";

                Assert.IsTrue(sw.ToString().Contains(expected));
            }
        }

        [Test]
        public async Task GivenInValidRoadId_IsSpecified_WhenTheClientIsRunThen_TheClientIsRun_ThenApplicationShouldDisplay_InformativeErrorAsync()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                var mockRoadStatusService = new Mock<IRoadStatusService>();
                mockRoadStatusService.Setup(s => s.GetRoadStatusAsync(It.IsAny<string>())).Throws(new ApiException
                {
                    StatusCode = 404,
                    Error = new ApiErrorResponse
                    {
                        HttpStatus = "NotFound",
                        HttpStatusCode = 404,
                        Message = "The following road id is not recognised: A233"
                    }
                });

                RoadStatusPrinter printer = new RoadStatusPrinter(mockRoadStatusService.Object);

                var response = await printer.PrintRoadStatusResponseAsync("A233");

                string expected = "A233 is not a valid road"; ;

                Assert.IsTrue(sw.ToString().Contains(expected));
            }
        }

        [Test]
        public async Task GivenValidRoadId_IsSpecifiedWhenTheClientIsRun_ThenShouldReturnZeroSystemSuccessCodeAsync()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                var mockRoadStatusService = new Mock<IRoadStatusService>();
                mockRoadStatusService.Setup(r => r.GetRoadStatusAsync(It.IsAny<string>())).Returns(Task.FromResult(_validRoadStatus));

                RoadStatusPrinter printer = new RoadStatusPrinter(mockRoadStatusService.Object);

                var result = await printer.PrintRoadStatusResponseAsync("A2");

                Assert.AreEqual(result, 0);
            }
        }

        [Test]
        public async Task GivenInvalidRoadIdIsSpecifiedWhenTheClientIsRunThenShouldReturnNonZeroSystemErrorCodeAsync()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                var mockRoadStatusService = new Mock<IRoadStatusService>();
                mockRoadStatusService.Setup(r => r.GetRoadStatusAsync(It.IsAny<string>())).Throws(new ApiException
                {
                    StatusCode = 404,
                    Error = new ApiErrorResponse
                    {
                        HttpStatus = "NotFound",
                        HttpStatusCode = 404,
                        Message = "The following road id is not recognised: A233"
                    }
                });

                RoadStatusPrinter printer = new RoadStatusPrinter(mockRoadStatusService.Object);

                var result = await printer.PrintRoadStatusResponseAsync("A233");

                Assert.AreEqual(result, 1);
            }
        }
    }
}