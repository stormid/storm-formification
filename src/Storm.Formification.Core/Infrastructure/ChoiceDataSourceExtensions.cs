using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Storm.Formification.Core.Infrastructure
{
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
                        return items
                            .GroupBy(c => c.Group)
                            .SelectMany(c =>
                            {
                                SelectListGroup group = string.IsNullOrWhiteSpace(c.Key) ? null : new SelectListGroup { Name = c.Key };
                                return c.Select(s => new SelectListItem(s.Text, s.Value) {
                                    Group = group,
                                    Disabled = s.Disabled
                                });
                            }) ?? Enumerable.Empty<SelectListItem>();
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
}
