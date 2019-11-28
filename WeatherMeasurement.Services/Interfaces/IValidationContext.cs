namespace WeatherMeasurement.Services.Interfaces
{
    public interface IValidationContext
    {
        void AddError(string key, string errorMessage);

        bool IsValid { get; }
    }
}
