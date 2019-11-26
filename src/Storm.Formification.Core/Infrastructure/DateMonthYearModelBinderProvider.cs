using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using System;
using System.Linq;

namespace Storm.Formification.Core.Infrastructure
{
    public class DateMonthYearModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Metadata.UnderlyingOrModelType != typeof(string))
            {
                return null;
            }

            if (context.Metadata is DefaultModelMetadata defaultMetadata && (defaultMetadata?.Attributes?.PropertyAttributes?.OfType<Forms.DateMonthYearAttribute>().Any() ?? false))
            {
                return new DateMonthYearModelBinder();
            }

            return null;
        }
    }
}
