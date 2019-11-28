using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using WeatherMeasurement.Models;
using WeatherMeasurement.Models.Dtos;
using WeatherMeasurement.Services.Interfaces;

namespace WeatherMeasurementApi.v1
{
    public class MeasurementApi
    {
        private readonly IMeasurementService _measurementService;
        private readonly IValidationContext _validationContext;

        public MeasurementApi(IMeasurementService measurementService, IValidationContext validatonDictionary)
        {
            _measurementService = measurementService;
            _validationContext = validatonDictionary;
        }

        [FunctionName("GetDeviceMeasurementsForDate")]
        public async Task<HttpResponseMessage> GetDeviceMeasurementsForDate(
            [HttpTrigger(AuthorizationLevel.Anonymous,"get", Route = "v1/devices/{device}/data/{date}")]HttpRequest req,
            string device,
            string date)
        {
            var result = await _measurementService
                .GetMeasurementsAsync(new MeasurementsDto() { Date = date, DeviceName = device });

            if (!_validationContext.IsValid)
            {
                return BadRequest();
            }

            if (!DoesAnyMeasurementExist(result))
            {
                return NotFound();
            }

            return Ok(typeof(AllMeasurements), result);            
        }
                
        [FunctionName("GetDeviceSpecificMeasurementsForDate")]
        public async Task<HttpResponseMessage> GetDeviceSpecificMeasurementsForDate(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/devices/{device}/data/{date}/{sensortype}")] HttpRequest req,
            string device,
            string sensortype,
            string date)
        {
            var result = await _measurementService
                .GetSpecificMeasurementAsync(new MeasurementsTypeDto() { Date = date, DeviceName = device, SensorType = sensortype });

            if (!_validationContext.IsValid)
            {
                return BadRequest();
            }

            if (result == null || !result.Any())
            {
                return NotFound();
            }

            return Ok(typeof(IEnumerable<Measurement>), result);
        }

        private static bool DoesAnyMeasurementExist(AllMeasurements result)
        {
            return result != null && (result.Humidities.Any() || result.Rainfalls.Any() || result.Temperatures.Any());
        }

        private HttpResponseMessage NotFound()
        {
            return new HttpResponseMessage(HttpStatusCode.NotFound);
        }

        private HttpResponseMessage BadRequest()
        {
            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }
        private HttpResponseMessage Ok(Type type, object result)
        {
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ObjectContent(type, result, new JsonMediaTypeFormatter())
            };
        }
    }
}
