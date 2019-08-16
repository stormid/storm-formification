using Microsoft.Extensions.DependencyInjection;
using System;

namespace Storm.Formification.Core.Infrastructure
{
    public class DefaultChoiceDataSourceSelector : IChoiceDataSourceSelector
    {
        private readonly IServiceProvider serviceProvider;

        public DefaultChoiceDataSourceSelector(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IChoiceDataSource Get<TDatasourceType>() where TDatasourceType : IChoiceDataSource
        {
            return serviceProvider.GetRequiredService<TDatasourceType>();
        }

        public IChoiceDataSource Get(Type datasourceType)
        {
            if (!datasourceType.IsAbstract && datasourceType.IsClass && typeof(IChoiceDataSource).IsAssignableFrom(datasourceType))
            {
                if (serviceProvider.GetService(datasourceType) is IChoiceDataSource choiceDataSource)
                {
                    return choiceDataSource;
                }
            }
            throw new InvalidCastException($"The type {datasourceType.Name} must be derived from {nameof(IChoiceDataSource)} and be container registered");
        }
    }
}
