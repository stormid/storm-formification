using System;

namespace Storm.Formification.Core.Infrastructure
{

    public class ChoiceDataSourceAttribute : AdditionalMetadataAttribute
    {
        public const string Key = "choice-datasource";

        public ChoiceDataSourceAttribute(Type dataSourceType)
            : base(Key, typeof(IChoiceDataSource).IsAssignableFrom(dataSourceType) ? dataSourceType : throw new InvalidCastException($"'{dataSourceType.Name}' must be derived from {nameof(IChoiceDataSource)}"))
        {
        }
    }
}
