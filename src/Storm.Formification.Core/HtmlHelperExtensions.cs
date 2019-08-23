using Storm.Formification.Core.Infrastructure;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Storm.Formification.Core
{
    public static class HtmlHelperExtensions
    {
        public static IHtmlContent DisplayFormForModel<TModel>(this IHtmlHelper<TModel> htmlHelper)
        {
            htmlHelper.ViewData.SetDisplayMode(true);
            return htmlHelper.Display(null, "Forms__Form", null, null);
        }

        public static IHtmlContent DisplayForm<TModel, TResult>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TResult>> expression)
        {
            htmlHelper.ViewData.SetDisplayMode(true);
            return htmlHelper.DisplayFor(expression, "Forms__Form", null, null);
        }

        public static IHtmlContent DisplayFormForModel(this IHtmlHelper htmlHelper)
        {
            htmlHelper.ViewData.SetDisplayMode(true);
            return htmlHelper.Display(null, "Forms__Form", null, null);
        }
        
        public static IHtmlContent DisplayForm(this IHtmlHelper htmlHelper, string expression)
        {
            htmlHelper.ViewData.SetDisplayMode(true);
            return htmlHelper.Display(expression, "Forms__Form", null, null);
        }

        public static IHtmlContent RenderFormForModel<TModel>(this IHtmlHelper<TModel> htmlHelper)
        {
            htmlHelper.ViewData.SetDisplayMode(false);
            return htmlHelper.Editor(null, "Forms__Form", null, null);
        }

        public static IHtmlContent RenderForm<TModel, TResult>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TResult>> expression)
        {
            htmlHelper.ViewData.SetDisplayMode(false);
            return htmlHelper.EditorFor(expression, "Forms__Form", null, null);
        }

        public static IHtmlContent RenderFormForModel(this IHtmlHelper htmlHelper)
        {
            htmlHelper.ViewData.SetDisplayMode(false);
            return htmlHelper.Editor(null, "Forms__Form", null, null);
        }

        public static IHtmlContent RenderForm(this IHtmlHelper htmlHelper, string expression)
        {
            htmlHelper.ViewData.SetDisplayMode(false);
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
            if(htmlHelper.ViewData.IsDisplayMode())
            { 
                return htmlHelper.Display(formProperty.Property.Name, formProperty.DataType);
            }

            return htmlHelper.Editor(formProperty.Property.Name, formProperty.DataType);
        }

        public static async Task<IEnumerable<SelectListItem>> GetChoices(this IHtmlHelper htmlHelper)
        {
            var dataSourceSelector = htmlHelper.ViewContext.HttpContext.RequestServices.GetRequiredService<IChoiceDataSourceSelector>();

            var type = htmlHelper.ViewData.ModelMetadata.UnderlyingOrModelType;

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

        public static async Task<IEnumerable<ChoiceItem>> GetChoicesForModel(this IHtmlHelper htmlHelper)
        {
            var dataSourceSelector = htmlHelper.ViewContext.HttpContext.RequestServices.GetRequiredService<IChoiceDataSourceSelector>();

            return await dataSourceSelector.GetChoiceItemForModelAsync(htmlHelper.ViewData);
        }
    }
}
