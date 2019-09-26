using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Common
{
    public static class ModelStateExtensions
    {
        public static void AddErrorsToModelState(this ModelStateDictionary modelState, IEnumerable<IdentityError> errors)
        {
            foreach (IdentityError error in errors)
            {
                modelState.TryAddModelError(error.Code, error.Description);
            }
        }
    }
}
