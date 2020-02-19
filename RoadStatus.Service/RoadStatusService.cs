using Newtonsoft.Json;
using RoadStatus.Service.Exceptions;
using RoadStatus.Service.Interfaces;
using RoadStatus.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoadStatus.Service
{
    public class RoadStatusService : IRoadStatusService
    {
        private readonly IHttpHandler _httpHandler;
        private readonly IConfig _config;

        public RoadStatusService(IHttpHandler httpHandler, IConfig config)
        {
            _httpHandler = httpHandler ?? throw new ArgumentNullException(nameof(httpHandler));
            _config = config ?? throw new ArgumentException(nameof(config));
        }

        public async Task<RoadStatusDto> GetRoadStatusAsync(string roadId)
        {
            string url = _config.Url + $"{roadId}?app_id={ _config.AppID}&app_key={_config.AppKey}";

            var response = await _httpHandler.SendAsync(url);

            if (response.IsSuccessStatusCode == false)
            {
                var content = JsonConvert.DeserializeObject<ApiErrorResponse>(await response.Content.ReadAsStringAsync());

                throw new ApiException
                {
                    StatusCode = (int)response.StatusCode,
                    Error = content
                };
            }

            var roadResponse = JsonConvert.DeserializeObject<List<RoadStatusDto>>(await response.Content.ReadAsStringAsync());

            return roadResponse.First();
        }
    }
}