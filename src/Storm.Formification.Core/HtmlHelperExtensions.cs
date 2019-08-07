using Storm.Formification.Core.Infrastructure;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Storm.Formification.Core
{
    public static class HtmlHelperExtensions
    {
        public static IHtmlContent RenderFormForModel<TModel>(this IHtmlHelper<TModel> htmlHelper)
        {
            return htmlHelper.Editor(null, "Forms__Form", null, null);
        }

        public static IHtmlContent RenderForm<TModel, TResult>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TResult>> expression)
        {
            return htmlHelper.EditorFor(expression, "Forms__Form", null, null);
        }

        public static IHtmlContent RenderFormForModel(this IHtmlHelper htmlHelper)
        {
            return htmlHelper.Editor(null, "Forms__Form", null, null);
        }

        public static IHtmlContent RenderForm(this IHtmlHelper htmlHelper, string expression)
        {
            return htmlHelper.Editor(expression, "Forms__Form", null, null);
        }

        public static async Task<IHtmlContent> RenderFormPropertiesAsync<TModel>(this IHtmlHelper htmlHelper, TModel model)
        {
            return await htmlHelper.PartialAsync("Forms__Form_Body", model);
        }

        public static async Task<IHtmlContent> FormSectionAsync(this IHtmlHelper htmlHelper, FormSection formSection)
        {
            htmlHelper.ViewData.SetCurrentFormSection(formSection);
            return await htmlHelper.PartialAsync("Forms__Form_Body_Section");
        }

        public static async Task<IHtmlContent> FormPropertiesAsync(this IHtmlHelper htmlHelper, IEnumerable<FormProperty> formProperties)
        {
            htmlHelper.ViewData.SetCurrentFormProperties(formProperties);
            return await htmlHelper.PartialAsync("Forms__Form_Body_Properties");
        }

        public static IHtmlContent FormProperty(this IHtmlHelper htmlHelper, FormProperty formProperty)
        {
            return htmlHelper.Editor(formProperty.Property.Name, formProperty.DataType);
        }

        public static async Task<IEnumerable<SelectListItem>> GetChoices(this IHtmlHelper htmlHelper)
        {
            var dataSourceSelector = htmlHelper.ViewContext.HttpContext.RequestServices.GetRequiredService<IChoiceDataSourceSelector>();

            var type = htmlHelper.ViewData.ModelMetadata.ModelType;

            var allowEmptyOption = Nullable.GetUnderlyingType(type) != null || htmlHelper.ViewData.ModelMetadata.IsReferenceOrNullableType;

            var items = new List<SelectListItem>();

            if (allowEmptyOption)
            {
                items.Add(new SelectListItem());
            }

            if (type?.IsEnum ?? false)
            {
                items.AddRange(htmlHelper.GetEnumSelectList(type));
            }
            else if (type.IsGenericType && type.GenericTypeArguments.FirstOrDefault().IsEnum)
            {
                items.AddRange(htmlHelper.GetEnumSelectList(type.GenericTypeArguments.FirstOrDefault()));
            }
            else
            {
                items.AddRange(await dataSourceSelector.GetSelectListItemsForModelAsync(htmlHelper.ViewData));
            }

            return items;
        }
    }
}
