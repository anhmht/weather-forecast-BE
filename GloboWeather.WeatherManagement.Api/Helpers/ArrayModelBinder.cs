using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GloboWeather.WeatherManagement.Api.Helpers
{
    public class ArrayModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            // Our binder works only on enumerable types
            if (!bindingContext.ModelMetadata.IsEnumerableType)
            {
                bindingContext.Result = ModelBindingResult.Failed();
                return  Task.CompletedTask;
            }

            // Get the inputted value through the value provider
            var vaule = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).ToString();
            
            // If that value is null or whitespace, we return null
            if (string.IsNullOrWhiteSpace(vaule))
            {
                bindingContext.Result = ModelBindingResult.Success(null);
                return Task.CompletedTask;
            }
            
            // The value isn't null or whitespace, 
            // and the type of the model is enumerable. 
            // Get the enumerable's type, and a converter 
            var elementType = bindingContext.ModelType.GetTypeInfo().GenericTypeArguments[0];
            var convert = TypeDescriptor.GetConverter(elementType);

            var values = vaule.Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => convert.ConvertToString(x.Trim()))
                .ToArray();

            var typeValues = Array.CreateInstance(elementType, values.Length);
            values.CopyTo(typeValues, 0);
            bindingContext.Model = typeValues;
            
            bindingContext.Result = ModelBindingResult.Success(bindingContext.Model);
            return Task.CompletedTask;
            ;
        }
    }
}