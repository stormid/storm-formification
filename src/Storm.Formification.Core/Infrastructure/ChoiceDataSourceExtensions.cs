using System;
using System.Collections;
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
                                return c.Select(s => new SelectListItem(s.Text, s.Value)
                                {
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
            var modelValue = viewData?.Model?.ToString() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(modelValue) && viewData.ModelMetadata.AdditionalValues.ContainsKey(ChoiceDataSourceAttribute.Key))
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

        public static async Task<IEnumerable<ChoiceItem>> GetChoiceItemForModelAsync2(this IChoiceDataSourceSelector dataSourceSelector, ViewDataDictionary viewData)
        {
            // var modelValue = viewData?.Model?.ToString() ?? string.Empty;
            var selectedItemsList = new List<ChoiceItem>();

            if (viewData.ModelMetadata.AdditionalValues.ContainsKey(ChoiceDataSourceAttribute.Key))
            {
                if (viewData.ModelMetadata.AdditionalValues.TryGetValue(ChoiceDataSourceAttribute.Key, out var type) && type is Type datasourceType)
                {
                    var dataSource = dataSourceSelector.Get(datasourceType);
                    if (dataSource != null)
                    {
                        if (viewData.ModelMetadata.IsEnumerableType && viewData.Model is IEnumerable selectedItems)
                        {
                            foreach (var selectedItem in selectedItems)
                            {
                                var item = await dataSource.GetAsync(selectedItem.ToString());
                                selectedItemsList.Add(item);
                            }
                        }
                        else
                        {
                            var item = await dataSource.GetAsync(viewData.Model?.ToString() ?? string.Empty);
                            selectedItemsList.Add(item);
                        }
                    }
                }
            }
            else
            {
                if (viewData.ModelMetadata.IsEnumerableType && viewData.Model is IEnumerable selectedItems)
                {
                    foreach (var selectedItem in selectedItems)
                    {
                        selectedItemsList.Add(new ChoiceItem(selectedItem?.ToString() ?? string.Empty, selectedItem?.ToString() ?? string.Empty));
                    }
                }
                else
                {
                    selectedItemsList.Add(new ChoiceItem(viewData.Model?.ToString() ?? string.Empty, viewData.Model?.ToString() ?? string.Empty));
                }
            }

            return selectedItemsList;
        }

        public static async Task<IEnumerable<ChoiceItem>> GetChoiceItemForModelAsync(this IChoiceDataSourceSelector dataSourceSelector, ViewDataDictionary viewData)
        {
            var selectedItemsList = new List<ChoiceItem>();

            if (viewData.ModelMetadata.IsEnumerableType && viewData.Model is IEnumerable selectedItems)
            {
                if (viewData.ModelMetadata.AdditionalValues.TryGetValue(ChoiceDataSourceAttribute.Key, out var type) && type is Type datasourceType)
                {
                    var dataSource = dataSourceSelector.Get(datasourceType);
                    foreach (var selectedItem in selectedItems)
                    {
                        var selectedItemValue = selectedItem?.ToString() ?? string.Empty;
                        var selectedItemText = selectedItem?.ToString() ?? string.Empty;

                        if (selectedItem.GetType().IsEnum)
                        {
                            selectedItemText = Enum.Format(selectedItem.GetType(), selectedItem ?? default(int), "G");
                            selectedItemValue = Enum.Format(selectedItem.GetType(), selectedItem ?? default(int), "D");
                        }

                        var item = dataSource != null ? await dataSource.GetAsync(selectedItemValue) : new ChoiceItem(selectedItemValue, selectedItemText);
                        selectedItemsList.Add(item);
                    }
                }
                else
                {
                    foreach (var selectedItem in selectedItems)
                    {
                        var selectedItemValue = selectedItem?.ToString() ?? string.Empty;
                        var selectedItemText = selectedItem?.ToString() ?? string.Empty;

                        if (selectedItem.GetType().IsEnum)
                        {
                            selectedItemText = Enum.Format(selectedItem.GetType(), selectedItem, "G");
                            selectedItemValue = Enum.Format(selectedItem.GetType(), selectedItem, "D");
                        }
                        selectedItemsList.Add(new ChoiceItem(selectedItemValue, selectedItemText));
                    }
                }
            }
            else
            {
                var selectedItemValue = viewData.Model?.ToString() ?? string.Empty;
                var selectedItemText = viewData.Model?.ToString() ?? string.Empty;

                if (viewData.ModelMetadata.IsEnum)
                {
                    selectedItemText = Enum.Format(viewData.ModelMetadata.UnderlyingOrModelType, viewData.Model ?? default(int), "G");
                    selectedItemValue = Enum.Format(viewData.ModelMetadata.UnderlyingOrModelType, viewData.Model ?? default(int), "D");
                }

                if (viewData.ModelMetadata.AdditionalValues.TryGetValue(ChoiceDataSourceAttribute.Key, out var type) && type is Type datasourceType)
                {
                    var dataSource = dataSourceSelector.Get(datasourceType);
                    var item = await dataSource.GetAsync(selectedItemValue);
                    selectedItemsList.Add(item);
                }
                else
                {
                    selectedItemsList.Add(new ChoiceItem(selectedItemValue, selectedItemText));
                }
            }

            return selectedItemsList;
        }
    }
}
