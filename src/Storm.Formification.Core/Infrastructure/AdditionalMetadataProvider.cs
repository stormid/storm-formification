namespace Storm.Formification.Core.Infrastructure
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

    public class AdditionalMetadataProvider : IDisplayMetadataProvider
    {
        public void CreateDisplayMetadata(DisplayMetadataProviderContext context)
        {
            if (context.PropertyAttributes != null && context.PropertyAttributes.OfType<AdditionalMetadataAttribute>().Any())
            {
                foreach (var addMetaAttr in context.PropertyAttributes.OfType<AdditionalMetadataAttribute>())
                {
                    context.DisplayMetadata.AdditionalValues.Add(addMetaAttr.Name, addMetaAttr.Value);
                }
            }
        }
    }
}
