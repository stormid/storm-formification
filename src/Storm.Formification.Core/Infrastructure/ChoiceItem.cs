namespace Storm.Formification.Core.Infrastructure
{
    public struct ChoiceItem
    {
        public string Value { get; set; }

        public string Text { get; set; }

        public string Group { get; set; }

        public bool Disabled { get; set; }

        public ChoiceItem(string value, string text, string group = null, bool disabled = false)
        {
            Value = value;
            Text = text;
            Group = group;
            Disabled = disabled;
        }
    }
}
