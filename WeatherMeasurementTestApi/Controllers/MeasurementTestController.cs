using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace WeatherMeasurementTestApi.Controllers
{
    [ApiController]
    public class MeasurementTestController : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;

        public MeasurementTestController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        /// <summary>
        /// Collect all of the measurements for one day, one sensor type, and one unit..
        /// </summary>
        /// <param name="device">Id of the device.</param>
        /// <param name="sensortype">Type of sensor.</param>
        /// <param name="date">Date of measurement.</param>
        /// <response code="200">OK</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">File not found or empty</response>
        /// <returns></returns>
        [HttpGet("v1/devices/{device}/data/{date}/{sensortype}")]
        public async Task<IActionResult> TestGetDeviceSpecificMeasurementsForDate(string device, string sensortype, string date)
        {
            return await GetResponseAsync($"http://localhost:7071/api/v1/devices/{device}/data/{date}/{sensortype}");
        }

        /// <summary>
        /// Collect all data points for one unit and one day.
        /// </summary>
        /// <param name="device">Id of the device.</param>
        /// <param name="date">Date of measurement.</param>
        /// <response code="200">OK</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">File not found or empty</response>
        /// <returns></returns>
        [HttpGet("v1/devices/{device}/data/{date}")]
        public async Task<IActionResult> TestGetDeviceMeasurementsForDate(string device, string date)
        {
            return await GetResponseAsync($"http://localhost:7071/api/v1/devices/{device}/data/{date}");
        }

        private async Task<IActionResult> GetResponseAsync(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var client = _clientFactory.CreateClient();
            var result = await client.SendAsync(request);
            var response = new ContentResult()
            {
                Content = await result.Content.ReadAsStringAsync(),
                StatusCode = (int)result.StatusCode
            };

            return response;
        }
    }
}
