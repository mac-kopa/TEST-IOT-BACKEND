using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WeatherMeasurement.Models;
using WeatherMeasurement.Models.Dtos;
using WeatherMeasurement.Services.Interfaces;

namespace WeatherMeasurement.Services
{
    public class MeasurementService : IMeasurementService
    {
        private readonly IValidationContext _validatonDictionary;
        private readonly IMeasurementRepository _repository;

        public MeasurementService(IMeasurementRepository repository, IValidationContext validatonDictionary)
        {
            _validatonDictionary = validatonDictionary;
            _repository = repository;
        }

        public async Task<IEnumerable<Measurement>> GetSpecificMeasurementAsync(MeasurementsTypeDto dto)
        {
            if (await ValidateMeasurementsTypeDto(dto))
            {
                return await _repository.GetMeasurementAsync(dto.Date, dto.SensorType, dto.DeviceName);
            }                

            return null;
        }

        public async Task<AllMeasurements> GetMeasurementsAsync(MeasurementsDto dto)
        {
            if (ValidateMeasurementsDto(dto))
            {
                return new AllMeasurements
                {
                    Humidities = await _repository.GetMeasurementAsync(dto.Date, "humidity", dto.DeviceName),
                    Rainfalls = await _repository.GetMeasurementAsync(dto.Date, "rainfall", dto.DeviceName),
                    Temperatures = await _repository.GetMeasurementAsync(dto.Date, "temperature", dto.DeviceName)
                };
            }                

            return null;
        }

        private bool ValidateMeasurementsDto(MeasurementsDto dto)
        {
            if (dto.DeviceName.Length == 0)
            {
                _validatonDictionary.AddError("device", "Device is required.");
            }
                
            if (dto.Date.Length == 0)
            {
                _validatonDictionary.AddError("date", "Date is required.");
            }
                
            if (!Regex.IsMatch(dto.Date, @"[0-9][0-9][0-9][0-9]-[0-3][0-9]-[0-3][0-9]"))
            {
                _validatonDictionary.AddError("date", "Date should be in format YYYY-MM-DD.");
            }
                
            return _validatonDictionary.IsValid;
        }

        private async Task<bool> ValidateMeasurementsTypeDto(MeasurementsTypeDto dto)
        {
            ValidateMeasurementsDto(dto);
            var sensorTypes = await _repository.GetSensorTypesAsync();

            if (!sensorTypes.Any(x => x.SensorType == dto.SensorType))
            {
                _validatonDictionary.AddError("date", "Date is required.");
            }                

            return _validatonDictionary.IsValid;
        }
    }
}
