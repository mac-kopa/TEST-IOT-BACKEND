using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using WeatherMeasurement.Repositories;
using WeatherMeasurement.Repositories.Interfaces;
using WeatherMeasurement.Services;
using WeatherMeasurement.Services.Interfaces;

[assembly: FunctionsStartup(typeof(WeatherMeasurementApi.Startup))]

namespace WeatherMeasurementApi
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddScoped<IMeasurementService,MeasurementService>();
            builder.Services.AddScoped<IMeasurementRepository, CsvMeasurementRepository>();
            builder.Services.AddScoped<IBlobRepository, BlobRepository>();
            builder.Services.AddScoped<IValidationContext, ModelStateWrapper>();
            builder.Services.AddTransient<ContainerReferenceBuilder, ContainerReferenceBuilder>();
        }
    }
}
