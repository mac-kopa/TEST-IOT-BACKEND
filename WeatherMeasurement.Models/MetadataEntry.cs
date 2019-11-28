namespace WeatherMeasurement.Models
{
    public class MetadataEntry
    {
        public MetadataEntry(string mainFolder, string measurmentType)
        {
            SensorType = measurmentType;
            MainFolder = mainFolder;
        }

        public string MainFolder { get; set; }

        public string SensorType { get; set; }
    }
}
