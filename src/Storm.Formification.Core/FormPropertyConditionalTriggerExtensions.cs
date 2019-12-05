namespace Storm.Formification.Core
{
    public static class FormPropertyConditionalTriggerExtensions
    {
        public static void SetConditionalTriggerTarget(this FormProperty formProperty, string triggerKey)
        {
            formProperty.Metadata.Add("_trigger-target", triggerKey);
        }

        public static string? GetConditionalTriggerTarget(this FormProperty formProperty)
        {
            if (formProperty.Metadata.TryGetValue("_trigger-target", out var triggerTargetValue) && triggerTargetValue is string triggerTarget)
            {
                return triggerTarget;
            }

            return null;
        }

        public static void SetConditionalTrigger(this FormProperty formProperty, string triggerKey)
        {
            if (!string.IsNullOrWhiteSpace(triggerKey))
            {
                formProperty.Metadata.Add("_trigger", triggerKey);
            }
        }

        public static string? GetConditionalTrigger(this FormProperty formProperty)
        {
            if(formProperty.Metadata.TryGetValue("_trigger", out var triggerKeyValue) && triggerKeyValue is string triggerKey)
            {
                return triggerKey;
            }

            return null;
        }
    }
}
