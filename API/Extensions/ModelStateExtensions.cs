using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace API.Extensions
{
    public static class ModelStateExtensions
    {
        public static void AddErrorsToModelState(this ModelStateDictionary modelState, IEnumerable<IdentityError> errors)
        {
            foreach (var error in errors)
            {
                modelState.TryAddModelError(error.Code, error.Description);
            }
        }
    }
}
