using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;

namespace Storm.Formification.Core.Infrastructure
{
    public interface IChoiceDataSourceSelector
    {
        IChoiceDataSource Get<TDatasourceType>() where TDatasourceType : IChoiceDataSource;
        IChoiceDataSource Get(Type datasourceType);
    }

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

    public class ChoiceDataSourceAttribute : AdditionalMetadataAttribute
    {
        public const string Key = "choice-datasource";

        public ChoiceDataSourceAttribute(Type dataSourceType)
            : base(Key, typeof(IChoiceDataSource).IsAssignableFrom(dataSourceType) ? dataSourceType : throw new InvalidCastException($"'{dataSourceType.Name}' must be derived from {nameof(IChoiceDataSource)}"))
        {
        }
    }

    public struct ChoiceItem
    {
        public string Value { get; set; }

        public string Text { get; set; }

        public string Group { get; set; }

        public ChoiceItem(string value, string text, string group = null)
        {
            Value = value;
            Text = text;
            Group = group;
        }
    }

    public static class ChoiceDataSourceExtensions
    {
        public static async Task<IEnumerable<SelectListItem>> GetSelectListItemsForModelAsync(this IChoiceDataSourceSelector dataSourceSelector, ViewDataDictionary viewData)
        {
            if (viewData.ModelMetadata.AdditionalValues.ContainsKey(ChoiceDataSourceAttribute.Key))
            {
                if (viewData.ModelMetadata.AdditionalValues.TryGetValue(ChoiceDataSourceAttribute.Key, out var type) && type is Type datasourceType)
                {
                    var dataSource = dataSourceSelector.Get(datasourceType);
                    if (dataSource != null)
                    {
                        var items = await dataSource.GetAsync();
                        return items.Select(c => new SelectListItem(c.Text, c.Value)) ?? Enumerable.Empty<SelectListItem>();
                    }
                }
            }

            return Enumerable.Empty<SelectListItem>();
        }

        public static async Task<string> GetSelectedItemTextForModelAsync(this IChoiceDataSourceSelector dataSourceSelector, ViewDataDictionary viewData)
        {
            var modelValue = viewData.Model.ToString();
            if (viewData.ModelMetadata.AdditionalValues.ContainsKey(ChoiceDataSourceAttribute.Key))
            {
                if (viewData.ModelMetadata.AdditionalValues.TryGetValue(ChoiceDataSourceAttribute.Key, out var type) && type is Type datasourceType)
                {
                    var dataSource = dataSourceSelector.Get(datasourceType);
                    if (dataSource != null)
                    {
                        var item = await dataSource.GetAsync(modelValue);
                        return item.Text;
                    }
                }
            }

            return modelValue;
        }
    }

    public interface IChoiceDataSource
    {
        Task<IEnumerable<ChoiceItem>> GetAsync();
        Task<ChoiceItem> GetAsync(string value);
    }
}
