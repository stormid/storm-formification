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
                            .Select(c => new SelectListItem(c.Text, c.Value) { Group = new SelectListGroup { Name = c.Group }, Disabled = c.Disabled }) ?? Enumerable.Empty<SelectListItem>();
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
