using NUnit.Framework;
using RoadStatus.Service.Exceptions;
using RoadStatus.Service.Interfaces;
using RoadStatus.Test.Autofac.Modules;
using System.Threading.Tasks;

namespace RoadStatus.Test
{
    [TestFixture]
    public class RoadStatusIntegrationTest : IoCSupportedTest<RoadStatusTestModules>
    {
        private IRoadStatusService _roadStatusService;

        [SetUp]
        public void Setup()
        {
            _roadStatusService = Resolve<IRoadStatusService>();
        }

        [Test]
        public async Task Given_A_ValidRoadId_WhenServiceIsCalled_TheDetailsOfStatus_ShouldBeReturned()
        {
            // Given a valid road
            var road = "A2";

            // Call the service
            var roadStatus = await _roadStatusService.GetRoadStatusAsync(road);

            //Then detail of the road status should be returned
            Assert.AreEqual(road, roadStatus.DisplayName);
            Assert.AreEqual(road.ToLower(), roadStatus.Id);
            Assert.AreEqual("/Road/a2", roadStatus.Url);
            Assert.IsNotEmpty(roadStatus.StatusSeverity);
            Assert.IsNotEmpty(roadStatus.StatusSeverityDescription);
        }

        [Test]
        public void Given_aN_InvalidRoadId_WhenServiceCalled_AnApiException_ShouldBeThrown_With_NotFoundExceptionStatus()
        {
            // Given an ivalid road
            var road = "INVALID_ROAD_CODE";

            //Then an ApiException should be thrown with 404 (Not Found) status
            ApiException ex = Assert.ThrowsAsync<ApiException>(async () => await _roadStatusService.GetRoadStatusAsync(road));
            Assert.AreEqual(404, ex.StatusCode);
            Assert.AreEqual("EntityNotFoundException", ex.Error.ExceptionType);
            Assert.AreEqual("NotFound", ex.Error.HttpStatus);
            Assert.AreEqual("The following road id is not recognised: INVALID_ROAD_CODE", ex.Error.Message);
        }
    }
}