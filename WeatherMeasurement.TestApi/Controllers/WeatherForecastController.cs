using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WeatherMeasurement.TestApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;
        public TestController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [HttpGet]
        public async Task<object> TestGetDeviceSpecificMeasurementsForDate(string device, string sensortype, string date)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"http://localhost:7071/api/v1/devices/{device}/data/{date}/{sensortype}");
            var client = _clientFactory.CreateClient();
            return await client.SendAsync(request);

        }

        [HttpGet]
        public async Task<object> TestGetDeviceMeasurementsForDate(string device, string date)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"http://localhost:7071/api/v1/devices/{device}/data/{date}");
            var client = _clientFactory.CreateClient();
            return await client.SendAsync(request);
        }
    }
}
