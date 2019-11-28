using System.Collections.Generic;

namespace WeatherMeasurement.Models
{
    public class AllMeasurements
    {
        public IEnumerable<Measurement> Temperatures { get; set; }

        public IEnumerable<Measurement> Humidities { get; set; }

        public IEnumerable<Measurement> Rainfalls { get; set; }
    }
}
