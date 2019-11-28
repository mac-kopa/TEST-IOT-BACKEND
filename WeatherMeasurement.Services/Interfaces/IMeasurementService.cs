using System.Collections.Generic;
using System.Threading.Tasks;
using WeatherMeasurement.Models;
using WeatherMeasurement.Models.Dtos;

namespace WeatherMeasurement.Services.Interfaces
{
    public interface IMeasurementService
    {
        Task<IEnumerable<Measurement>> GetSpecificMeasurementAsync(MeasurementsTypeDto dto);

        Task<AllMeasurements> GetMeasurementsAsync(MeasurementsDto dto);
    }
}
