namespace Storm.Formification.TagHelpers
{
    using Storm.Formification.Core.Infrastructure;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    
    public class HintLabelTagHelper : TagHelper
    {
        [HtmlAttributeName("asp-for")]
        public ModelExpression? For { get; set; }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext? ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if(For != null && ViewContext != null)
            {
                if (For.Metadata.AdditionalValues.ContainsKey(HintLabelAttribute.HintLabelKey))
                {
                    var hintLabelValue = For.Metadata.AdditionalValues[HintLabelAttribute.HintLabelKey].ToString();
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
            else
            {
                output.SuppressOutput();
            }
        }
    }
}
