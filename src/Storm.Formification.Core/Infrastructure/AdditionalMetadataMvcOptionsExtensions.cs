namespace Storm.Formification.Core.Infrastructure
{
    using System;
    using Microsoft.AspNetCore.Mvc;

    public static class AdditionalMetadataMvcOptionsExtensions
    {
        public static MvcOptions AddAdditionalMetadataProvider(this MvcOptions options)
        {
            options.ModelMetadataDetailsProviders.Add(new AdditionalMetadataProvider());
            return options;
        }
    }
}
