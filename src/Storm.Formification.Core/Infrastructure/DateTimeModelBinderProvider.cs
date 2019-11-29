using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using System;
using System.Linq;

namespace Storm.Formification.Core.Infrastructure
{
    public class DateTimeModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Metadata.UnderlyingOrModelType != typeof(DateTime))
            {
                return null;
            }

            if (context.Metadata is DefaultModelMetadata defaultMetadata && (defaultMetadata?.Attributes?.PropertyAttributes?.OfType<Forms.DateAttribute>().Any() ?? false))
            {
                if (context.Metadata.IsReferenceOrNullableType)
                {
                    return new NullableDateTimeModelBinder();
                }
                else
                {
                    return new DateTimeModelBinder();
                }
            }

            return null;
        }
    }
}
