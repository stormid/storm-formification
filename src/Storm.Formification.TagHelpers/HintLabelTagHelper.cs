namespace Storm.Formification.TagHelpers
{
    using Storm.Formification.Core.Infrastructure;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
    using Microsoft.AspNetCore.Razor.TagHelpers;

    public class HintLabelTagHelper : TagHelper
    {
        private readonly IModelMetadataProvider modelMetadataProvider;

        public HintLabelTagHelper(IModelMetadataProvider modelMetadataProvider)
        {
            this.modelMetadataProvider = modelMetadataProvider;
        }

        [HtmlAttributeName("asp-for")]
        public ModelExpression For { get; set; }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var modelExplorer = ExpressionMetadataProvider.FromStringExpression(For.Name, ViewContext.ViewData, modelMetadataProvider);
            if (modelExplorer.Metadata.AdditionalValues.ContainsKey(HintLabelAttribute.HintLabelKey))
            {
                var hintLabelValue = modelExplorer.Metadata.AdditionalValues[HintLabelAttribute.HintLabelKey].ToString();
                if (!string.IsNullOrWhiteSpace(hintLabelValue))
                {
                    output.TagName = "span";
                    output.Content.SetContent(hintLabelValue);
                }
            }
            else
            {
                output.SuppressOutput();
            }
        }
    }
}
