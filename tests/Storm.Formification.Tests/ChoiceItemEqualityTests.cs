using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Storm.Formification.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Storm.Formification.Tests
{
    public class ChoiceItemTests
    {
        [Fact]
        public void ShouldNotBeEqual()
        {
            var choiceItem = new ChoiceItem("1", "One");
            var choiceItem2 = new ChoiceItem("2", "Two");

            choiceItem.Equals(choiceItem2).Should().BeFalse();
        }

        [Fact]
        public void ShouldBeEqual()
        {
            var choiceItem = new ChoiceItem("1", "One");
            var choiceItem2 = new ChoiceItem("1", "Two");

            choiceItem.Equals(choiceItem2).Should().BeTrue();
        }

        [Theory]
        [InlineData("1", "One", null, false)]
        [InlineData("2", "Two", null, false)]
        [InlineData("3", "Three", "Group1", false)]
        [InlineData("3", "Three", null, true)]
        public void ShouldImplicitConvertToSelectListItem(string value, string text, string group, bool disabled)
        {
            var choiceItem = new ChoiceItem(value, text, group, disabled);
            var selectListItem = new SelectListItem
            {
                Value = value,
                Text = text,
                Disabled = disabled,
            };

            if(!string.IsNullOrWhiteSpace(group))
            {
                selectListItem.Group = new SelectListGroup { Name = group };
            }

            var convertedChoiceItem = (SelectListItem)choiceItem;

            convertedChoiceItem.Should().BeEquivalentTo(selectListItem);
        }
    }
}
