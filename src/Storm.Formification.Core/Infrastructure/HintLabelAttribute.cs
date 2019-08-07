namespace Storm.Formification.Core.Infrastructure
{
    public class HintLabelAttribute : AdditionalMetadataAttribute
    {
        public const string HintLabelKey = "hint-label";

        public HintLabelAttribute(string labelText)
            : base(HintLabelKey, labelText)
        {
        }
    }
}
