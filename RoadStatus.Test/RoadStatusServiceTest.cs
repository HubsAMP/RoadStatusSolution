using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using RoadStatus.Service;
using RoadStatus.Service.Exceptions;
using RoadStatus.Service.Interfaces;

namespace RoadStatus.Test
{
    [TestFixture]
    public class RoadStatusServiceTest
    {
        private string _validJsonResponse;
        private string _invalidJsonResponse;

        [SetUp]
        public void Setup()
        {
            _validJsonResponse =
                @"[
                        {
                            '$type': 'Tfl.Api.Presentation.Entities.RoadCorridor, Tfl.Api.Presentation.Entities',
                            'id': 'a2',
                            'displayName': 'A2',
                            'statusSeverity': 'Good',
                            'statusSeverityDescription': 'No Exceptional Delays',
                            'bounds': '[[-0.0857,51.44091],[0.17118,51.49438]]',
                            'envelope': '[[-0.0857,51.44091],[-0.0857,51.49438],[0.17118,51.49438],[0.17118,51.44091],[-0.0857,51.44091]]',
                            'url': '/Road/a2'
                        }
                    ]";

            _invalidJsonResponse =
                @"{
                    '$type': 'Tfl.Api.Presentation.Entities.ApiError, Tfl.Api.Presentation.Entities',
                    'timestampUtc': '2017-11-21T14:37:39.7206118Z',
                    'exceptionType': 'EntityNotFoundException',
                    'httpStatusCode': 404,
                    'httpStatus': 'NotFound',
                    'relativeUri': '/Road/A233',
                    'message': 'The following road id is not recognised: A233'
                }";
        }

        [Test]
        public async Task GivenValidRoadId_WhenServiceIsCalled_TheDetailsOfStatus_ShouldBeReturned()
        {
            // Given a valid road
            var road = "A2";

            // Call the service
            var mockHandler = new Mock<IHttpHandler>();
            var mockConfig = new Mock<IConfig>();

            var response = new HttpResponseMessage
            {
                Content = new StringContent(_validJsonResponse),
                StatusCode = HttpStatusCode.OK
            };
            mockHandler.Setup(r => r.SendAsync(It.IsAny<string>())).Returns(Task.FromResult(response));

            var roadStatusService = new RoadStatusService(mockHandler.Object, mockConfig.Object);

            var roadStatus = await roadStatusService.GetRoadStatusAsync(road);

            //Then detail of the road status should be returned
            Assert.AreEqual(road, roadStatus.DisplayName);
            Assert.AreEqual("Good", roadStatus.StatusSeverity);
            Assert.AreEqual("No Exceptional Delays", roadStatus.StatusSeverityDescription);
        }

        [Test]
        public void GivenInvalidRoadId_WhenServiceIsCalled_An_ApiException_ShouldBeThrownWith_NotFoundExceptionStatus()
        {
            // Given an invalid road
            var road = "A233";

            // Call the service
            var mockClient = new Mock<IHttpHandler>();
            var mockConfig = new Mock<IConfig>();
            var response = new HttpResponseMessage
            {
                Content = new StringContent(_invalidJsonResponse),
                StatusCode = HttpStatusCode.NotFound
            };
            mockClient.Setup(r => r.SendAsync(It.IsAny<string>())).Returns(Task.FromResult(response));

            var roadStatusService = new RoadStatusService(mockClient.Object, mockConfig.Object);

            //Then an ApiException should be thrown with 404 (Not Found) status
            ApiException ex = Assert.ThrowsAsync<ApiException>(async () => await roadStatusService.GetRoadStatusAsync(road));
            Assert.AreEqual(404, ex.StatusCode);
            Assert.AreEqual("EntityNotFoundException", ex.Error.ExceptionType);
            Assert.AreEqual("NotFound", ex.Error.HttpStatus);
            Assert.AreEqual("The following road id is not recognised: A233", ex.Error.Message);
        }
    }
}