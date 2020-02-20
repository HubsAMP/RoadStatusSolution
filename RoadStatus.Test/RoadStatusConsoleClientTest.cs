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
        private Mock<IConsoleWrapper> _mockConsoleWrapper;

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
            _mockConsoleWrapper = new Mock<IConsoleWrapper>();
            _mockPrintService = new Mock<PrintService>(_mockRoadStatusService.Object, _mockConsoleWrapper.Object);
        }

        [Test]
        public async Task GivenValidRoadId_IsSpecified_WhenTheClientIsRunThen_TheRoadDisplayName_ShouldBeDisplayedAsync()
        {
            _mockRoadStatusService.Setup(s => s.GetRoadStatusAsync("A2")).Returns(Task.FromResult(_validRoadStatus));

            var response = await _mockPrintService.Object.PrintRoadStatusResponseAsync("A2");

            string expected = $"The status of the {_validRoadStatus.DisplayName} is as follows:";

            Assert.IsTrue(_mockPrintService.Object.GetOutputMessage().Contains(expected));
        }

        [Test]
        public async Task GivenValidRoadId_IsSpecified_WhenTheClientIsRunThen_TheStatusSeverity_ShouldBeDisplayedAsync()
        {
            _mockRoadStatusService.Setup(s => s.GetRoadStatusAsync(It.IsAny<string>())).Returns(Task.FromResult(_validRoadStatus));

            var response = await _mockPrintService.Object.PrintRoadStatusResponseAsync("A2");

            string expected = $"Road Status is {_validRoadStatus.StatusSeverity}";

            Assert.IsTrue(_mockPrintService.Object.GetOutputMessage().Contains(expected));
        }

        [Test]
        public async Task GivenValidRoadId_IsSpecified_WhenTheClientIsRunThen_TheStatusSeverityDescription_ShouldBeDisplayedAsync()
        {
            _mockRoadStatusService.Setup(s => s.GetRoadStatusAsync(It.IsAny<string>())).Returns(Task.FromResult(_validRoadStatus));

            var response = await _mockPrintService.Object.PrintRoadStatusResponseAsync("A2");

            string expected = $"Road Status Description is {_validRoadStatus.StatusSeverityDescription}";

            Assert.IsTrue(_mockPrintService.Object.GetOutputMessage().Contains(expected));
        }

        [Test]
        public async Task GivenInValidRoadId_IsSpecified_WhenTheClientIsRunThen_TheClientIsRun_ThenApplicationShouldDisplay_InformativeErrorAsync()
        {
            _mockRoadStatusService.Setup(s => s.GetRoadStatusAsync(It.IsAny<string>())).Throws(new ApiException
            {
                StatusCode = 404,
                Error = new ApiErrorResponse
                {
                    HttpStatus = "NotFound",
                    HttpStatusCode = 404,
                    Message = "The following road id is not recognised: A233"
                }
            });

            var response = await _mockPrintService.Object.PrintRoadStatusResponseAsync("A233");

            string expected = "A233 is not a valid road";

            Assert.IsTrue(_mockPrintService.Object.GetOutputMessage().Contains(expected));
        }

        [Test]
        public async Task GivenValidRoadId_IsSpecifiedWhenTheClientIsRun_ThenShouldReturnZeroSystemSuccessCodeAsync()
        {
            _mockRoadStatusService.Setup(s => s.GetRoadStatusAsync(It.IsAny<string>())).Returns(Task.FromResult(_validRoadStatus));

            var result = await _mockPrintService.Object.PrintRoadStatusResponseAsync("A2");

            Assert.AreEqual(result, 0);
        }

        [Test]
        public async Task GivenInvalidRoadIdIsSpecifiedWhenTheClientIsRunThenShouldReturnNonZeroSystemErrorCodeAsync()
        {
            _mockRoadStatusService.Setup(s => s.GetRoadStatusAsync(It.IsAny<string>())).Throws(new ApiException
            {
                StatusCode = 404,
                Error = new ApiErrorResponse
                {
                    HttpStatus = "NotFound",
                    HttpStatusCode = 404,
                    Message = "The following road id is not recognised: A233"
                }
            });

            var result = await _mockPrintService.Object.PrintRoadStatusResponseAsync("A2");

            Assert.AreEqual(result, 1);
        }
    }
}