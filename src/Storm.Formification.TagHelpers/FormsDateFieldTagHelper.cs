﻿namespace Storm.Formification.TagHelpers
{
    using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Storm.Formification.Core;

    [HtmlTargetElement("input", Attributes = "form-date-field")]
    public class FormsDateFieldTagHelper : TagHelper
    {
        public enum DateFields
        {
            Day,
            Month,
            Year,
        }

        public string? Id { get; set; }

        public string? Name { get; set; }

        [HtmlAttributeName("form-date-field")]
        public DateFields FormDateField { get; set; }
        
        public override int Order => 9999;

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext? ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (ViewContext != null)
            {
                output.Attributes.SetAttribute("id", string.IsNullOrWhiteSpace(Id) ? $"{ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(string.Empty).Replace(".", "_")}_{FormDateField.ToString()}" : Id);
                output.Attributes.SetAttribute("name", string.IsNullOrWhiteSpace(Name) ? ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(FormDateField.ToString()) : Name);
                output.Attributes.SetAttribute("value", GetAttemptedFieldValue(ViewContext.ViewData));
            }
        }

        private string? GetAttemptedFieldValue(ViewDataDictionary viewData)
        {
            return viewData.GetAttemptedValue(FormDateField.ToString());
        }
    }
}
