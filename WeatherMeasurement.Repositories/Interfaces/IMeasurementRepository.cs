using System.Collections.Generic;
using System.Threading.Tasks;
using WeatherMeasurement.Models;

namespace WeatherMeasurement.Services.Interfaces
{
    public interface IMeasurementRepository
    {
        Task<IEnumerable<Measurement>> GetMeasurementAsync(string date, string type, string device);

        Task<IList<MetadataEntry>> GetSensorTypesAsync();
    }
}
