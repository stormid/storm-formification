using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace Storm.Formification.Core.Infrastructure
{
    public struct ChoiceItem : IEquatable<ChoiceItem>
    {
        public string Value { get; set; }

        public string Text { get; set; }

        public string? Group { get; set; }

        public bool Disabled { get; set; }

        public ChoiceItem(string value, string text, string? group = null, bool disabled = false)
        {
            Value = value;
            Text = text;
            Group = group;
            Disabled = disabled;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public bool Equals(ChoiceItem other)
        {
            return other.Value == Value;
        }

        public static implicit operator SelectListItem(ChoiceItem choiceItem)
        {
            var item = new SelectListItem
            {
                Text = choiceItem.Text,
                Value = choiceItem.Value,
                Disabled = choiceItem.Disabled,
            };

            if(!string.IsNullOrWhiteSpace(choiceItem.Group))
            {
                item.Group = new SelectListGroup { Name = choiceItem.Group };
            }

            return item;
        }

        public static bool operator ==(ChoiceItem left, ChoiceItem right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ChoiceItem left, ChoiceItem right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            if(obj is ChoiceItem choiceItem)
            {
                return Equals(choiceItem);
            }

            return false;
        }
    }
}
