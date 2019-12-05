using Storm.Formification.Core.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Linq;
using System.Reflection;
using static Storm.Formification.Core.Forms;

namespace Storm.Formification.Core
{

    public static class FormsServiceCollectionExtensions
    {
        private static Assembly[] GetAssemblyList(Assembly[] assemblies)
        {
            return (assemblies?.Union(new[] { typeof(Forms).Assembly }) ?? new[] { typeof(Forms).Assembly }).ToArray();
        }

        private static Type[]? FindChoiceDatasourceTypes(Assembly[] assemblies)
        {
            return assemblies?.SelectMany(a => a
                    .GetExportedTypes()
                    .Where(t => typeof(IChoiceDataSource).IsAssignableFrom(t))
                    .Where(t => !t.IsAbstract && t.IsClass)
                )?.ToArray();
        }

        public static IServiceCollection AddFormsHandlers(this IServiceCollection services, params Assembly[] assemblies)
        {
            var includeSelfAssemblies = GetAssemblyList(assemblies);

            var locator = new FormLocator(includeSelfAssemblies);
            services.AddSingleton<IFormLocator>(sp => locator);

            foreach (var formType in locator.All())
            {
                services.AddFormHandler(formType);
            }

            services.AddChoiceDataSources(includeSelfAssemblies);

            return services;
        }

        private static IServiceCollection AddChoiceDataSources(this IServiceCollection services, params Assembly[] assemblies)
        {
            services.TryAddScoped<IChoiceDataSourceSelector, DefaultChoiceDataSourceSelector>();
            foreach (var datasourceType in FindChoiceDatasourceTypes(assemblies) ?? Enumerable.Empty<Type>())
            {
                services.TryAddScoped(datasourceType);
            }

            return services;
        }

        private static IServiceCollection AddFormHandler(this IServiceCollection services, Type formType)
        {
            // must be a non-abstract class with a public, parameterless constructor
            if (!formType.IsAbstract && formType.IsClass && formType.GetConstructor(Type.EmptyTypes) != null)
            {
                var formActionServiceType = typeof(IFormActions<>).MakeGenericType(formType);
                var handlerImplType = formType.GetCustomAttribute<HandlerAttribute>()?.HandlerType;
                var formActionImplType = handlerImplType != null && formActionServiceType.IsAssignableFrom(handlerImplType) ? handlerImplType : typeof(DefaultFormActions<>).MakeGenericType(formType);

                services.TryAddScoped(formActionServiceType, formActionImplType);
            }

            return services;
        }
    }
}
