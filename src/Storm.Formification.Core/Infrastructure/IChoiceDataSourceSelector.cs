using System;

namespace Storm.Formification.Core.Infrastructure
{
    public interface IChoiceDataSourceSelector
    {
        IChoiceDataSource Get<TDatasourceType>() where TDatasourceType : IChoiceDataSource;
        IChoiceDataSource Get(Type datasourceType);
    }
}
