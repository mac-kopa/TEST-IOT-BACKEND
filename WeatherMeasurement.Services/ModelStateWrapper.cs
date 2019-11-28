using System.Web.Http.ModelBinding;
using WeatherMeasurement.Services.Interfaces;

namespace WeatherMeasurement.Services
{
    public class ModelStateWrapper : IValidationContext
    {
        private readonly ModelStateDictionary _modelState;

        public ModelStateWrapper()
        {
            _modelState = new ModelStateDictionary();
        }

        public void AddError(string key, string errorMessage)
        {
            _modelState.AddModelError(key, errorMessage);
        }

        public bool IsValid
        {
            get { return _modelState.IsValid; }
        }
    }
}
