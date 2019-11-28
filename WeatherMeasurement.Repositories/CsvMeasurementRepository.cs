using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WeatherMeasurement.Models;
using WeatherMeasurement.Repositories.Interfaces;
using WeatherMeasurement.Services.Interfaces;

namespace WeatherMeasurement.Services
{
    public class CsvMeasurementRepository : IMeasurementRepository
    {
        private IList<MetadataEntry> sensorTypes = new List<MetadataEntry>();
        private readonly IBlobRepository _repository;
        private const string metadataFileName = "metadata.csv";

        public CsvMeasurementRepository(IBlobRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Measurement>> GetMeasurementAsync(string date, string type, string device)
        {
            if (!sensorTypes.Any())
            {
                sensorTypes = await GetSensorTypesAsync();
            }
            
            var sensorType = sensorTypes.First(x => x.SensorType == type);
            var path = $"{sensorType.MainFolder}/{sensorType.SensorType}/";
            var fileName = $"{ date }.csv";
            var measurements = new List<Measurement>();

            await ReadFileStream(path, fileName, (x1, x2) => measurements.Add(new Measurement(x1, x2)));

            return measurements;
        }

        public async Task<IList<MetadataEntry>> GetSensorTypesAsync()
        {
            await ReadFileStream("", metadataFileName, (x1, x2) => sensorTypes.Add(new MetadataEntry(x1, x2)));

            return sensorTypes;
        }

        private async Task ReadFileStream(string path, string fileName, Action<string, string> addToCollection)
        {
            using (var fileStream = await _repository.GetFileStreamAsync(path, fileName))
            {
                if (fileStream == null)
                {
                    return;
                }

                using (var reader = new StreamReader(fileStream))
                {
                    string line;
                    while (!reader.EndOfStream)
                    {
                        line = await reader.ReadLineAsync();
                        var lineValues = line.Split(';');

                        if(lineValues.Count() == 2)
                        {
                            addToCollection(lineValues[0], lineValues[1]);
                        }
                    }
                }
            }
        }
    }
}
