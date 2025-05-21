using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TaskManagment.Mvc.Extensions
{
    public static class ModelStateExtensions
    {
        public static string GetErrorsAsString(this ModelStateDictionary modelState, string separator = " | ")
        {
            if (modelState == null || !modelState.Any())
                return string.Empty;

            return string.Join(separator, modelState.Values
                                                    .SelectMany(v => v.Errors ?? Enumerable.Empty<ModelError>())
                                                   ?.Select(e => e.ErrorMessage) ?? new List<string>());
        }
    }
}
