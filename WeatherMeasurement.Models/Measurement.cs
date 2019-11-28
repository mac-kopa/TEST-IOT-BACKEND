namespace WeatherMeasurement.Models
{
    public class Measurement
    {
        public Measurement(string timestamp, string value)
        {
            Timestamp = timestamp;
            Value = value;
        }

        public string Timestamp { get; set; }
        
        public string Value { get; set; }
    }
}
