using Storm.Formification.Core.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Storm.Formification.Core
{
    public static class FormsMvcBuilderExtensions
    {
        public static IMvcBuilder ConfigureForms(this IMvcBuilder mvcBuilder, params Assembly[] assemblies)
        {
            mvcBuilder.Services.AddFormsHandlers(assemblies);
            mvcBuilder.AddMvcOptions(o =>
            {
                o.AddAdditionalMetadataProvider();
                o.ModelBinderProviders.Insert(0, new DateTimeModelBinderProvider());
                o.ModelMetadataDetailsProviders.Add(new HumanizerMetadataProvider());
            });
            return mvcBuilder;
        }

        public static IMvcBuilder EnableFormsController(this IMvcBuilder mvcBuilder, params Assembly[] assemblies)
        {
            mvcBuilder.ConfigureApplicationPartManager(s => s.FeatureProviders.Add(new FormsControllerFeatureProvider(assemblies)));
            return mvcBuilder;
        }
    }
}
