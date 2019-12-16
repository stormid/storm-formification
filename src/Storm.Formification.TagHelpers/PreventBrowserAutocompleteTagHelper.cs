namespace Storm.Formification.TagHelpers
{
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Storm.Formification.Core;

    [HtmlTargetElement("input", Attributes = "asp-for")]
    public class PreventBrowserAutocompleteTagHelper : TagHelper
    {
        private readonly IModelMetadataProvider modelMetadataProvider;

        public PreventBrowserAutocompleteTagHelper(IModelMetadataProvider modelMetadataProvider)
        {
            this.modelMetadataProvider = modelMetadataProvider;
        }

        [HtmlAttributeName("asp-for")]
        public ModelExpression? For { get; set; }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext? ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if(For != null && ViewContext != null)
            {
                var key = Forms.PreventBrowserAutocomplete.Key;
                var modelExplorer = ExpressionMetadataProvider.FromStringExpression(For.Name, ViewContext.ViewData, modelMetadataProvider);
                if (modelExplorer.Metadata.AdditionalValues.ContainsKey(key) && modelExplorer.Metadata.AdditionalValues[key] is bool value)
                {
                    if(!output.Attributes.ContainsName("autocomplete"))
                    {
                        output.Attributes.Add("autocomplete", value ? "off" : "on");
                    }
                }
            }
        }
    }
}
