using FluentAssertions;
using Storm.Formification.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Storm.Formification.Tests
{
    public class String_FormLayoutDescriptorTests
    {
        private const string TestFormId = "9e015411-a682-4942-b8d7-e30c0ea5c9af";
        private const string TestFormName = "test form";
        private const int Version = 1;

        [Forms.Info(TestFormName, TestFormId,Version)]
        public class TestForm
        {
            [Forms.Text]
            public string? Field { get; set; }
        }

        [Fact]
        public void ShouldContainExpectedFormIdValue()
        {
            var descriptor = FormLayoutDescriptor.Build(typeof(TestForm));

            descriptor?.Id.Should().Be(TestFormId);
        }

        [Fact]
        public void ShouldContainExpectedFormNameValue()
        {
            var descriptor = FormLayoutDescriptor.Build(typeof(TestForm));

            descriptor?.Name.Should().Be(TestFormName);
        }

        [Fact]
        public void ShouldContainExpectedFormSections()
        {
            var descriptor = FormLayoutDescriptor.Build(typeof(TestForm));

            descriptor?.HasSections().Should().BeFalse();
            descriptor?.Sections.Should().HaveCount(0);
        }

        [Fact]
        public void ShouldContainExpectedFormProperties()
        {
            var descriptor = FormLayoutDescriptor.Build(typeof(TestForm));

            descriptor?.Properties.Should().HaveCount(1);
        }
    }

    public class Boolean_FormLayoutDescriptorTests
    {
        private const string TestFormId = "9e015411-a682-4942-b8d7-e30c0ea5c9af";
        private const string TestFormName = "test form";
        private const int Version = 1;
        [Forms.Info(TestFormName, TestFormId,1)]
        public class TestForm
        {
            [Forms.Boolean]
            public bool Field { get; set; }
        }

        [Fact]
        public void ShouldContainExpectedFormIdValue()
        {
            var descriptor = FormLayoutDescriptor.Build(typeof(TestForm));

            descriptor?.Id.Should().Be(TestFormId);
        }

        [Fact]
        public void ShouldContainExpectedFormNameValue()
        {
            var descriptor = FormLayoutDescriptor.Build(typeof(TestForm));

            descriptor?.Name.Should().Be(TestFormName);
        }

        [Fact]
        public void ShouldContainExpectedFormSections()
        {
            var descriptor = FormLayoutDescriptor.Build(typeof(TestForm));

            descriptor?.HasSections().Should().BeFalse();
            descriptor?.Sections.Should().HaveCount(0);
        }

        [Fact]
        public void ShouldContainExpectedFormProperties()
        {
            var descriptor = FormLayoutDescriptor.Build(typeof(TestForm));

            descriptor?.Properties.Should().HaveCount(1);
        }
    }

    public class Date_FormLayoutDescriptorTests
    {
        private const string TestFormId = "9e015411-a682-4942-b8d7-e30c0ea5c9af";
        private const string TestFormName = "test form";
        private const int Version = 1;
        [Forms.Info(TestFormName, TestFormId,Version)]
        public class TestForm
        {
            [Forms.Date]
            public bool Field { get; set; }
        }

        [Fact]
        public void ShouldContainExpectedFormIdValue()
        {
            var descriptor = FormLayoutDescriptor.Build(typeof(TestForm));

            descriptor?.Id.Should().Be(TestFormId);
        }

        [Fact]
        public void ShouldContainExpectedFormNameValue()
        {
            var descriptor = FormLayoutDescriptor.Build(typeof(TestForm));

            descriptor?.Name.Should().Be(TestFormName);
        }

        [Fact]
        public void ShouldContainExpectedFormSections()
        {
            var descriptor = FormLayoutDescriptor.Build(typeof(TestForm));

            descriptor?.HasSections().Should().BeFalse();
            descriptor?.Sections.Should().HaveCount(0);
        }

        [Fact]
        public void ShouldContainExpectedFormProperties()
        {
            var descriptor = FormLayoutDescriptor.Build(typeof(TestForm));

            descriptor?.Properties.Should().HaveCount(1);
        }
    }

    public class Numeric_FormLayoutDescriptorTests
    {
        private const string TestFormId = "9e015411-a682-4942-b8d7-e30c0ea5c9af";
        private const string TestFormName = "test form";
        private const int Version = 1;
        [Forms.Info(TestFormName, TestFormId,Version)]
        public class TestForm
        {
            [Forms.Numeric]
            public int IntField { get; set; }

            [Forms.Numeric]
            public int? NullableIntField { get; set; }

            [Forms.Numeric]
            public float FloatField { get; set; }

            [Forms.Numeric]
            public float? NullableFloatField { get; set; }

            [Forms.Numeric]
            public decimal DecimalField { get; set; }

            [Forms.Numeric]
            public decimal? NullableDecimalField { get; set; }
        }

        [Fact]
        public void ShouldContainExpectedFormIdValue()
        {
            var descriptor = FormLayoutDescriptor.Build(typeof(TestForm));

            descriptor?.Id.Should().Be(TestFormId);
        }

        [Fact]
        public void ShouldContainExpectedFormNameValue()
        {
            var descriptor = FormLayoutDescriptor.Build(typeof(TestForm));

            descriptor?.Name.Should().Be(TestFormName);
        }

        [Fact]
        public void ShouldContainExpectedFormSections()
        {
            var descriptor = FormLayoutDescriptor.Build(typeof(TestForm));

            descriptor?.HasSections().Should().BeFalse();
            descriptor?.Sections.Should().HaveCount(0);
        }

        [Fact]
        public void ShouldContainExpectedFormProperties()
        {
            var descriptor = FormLayoutDescriptor.Build(typeof(TestForm));

            descriptor?.Properties.Should().HaveCount(6);
        }
    }

    public class NoSections_FormLayoutDescriptorTests
    {
        private const string TestFormId = "9e015411-a682-4942-b8d7-e30c0ea5c9af";
        private const string TestFormName = "test form";
        private const int Version = 1;

        [Forms.Info(TestFormName, TestFormId,Version)]
        public class TestForm
        {
            [Forms.Text]
            public string? TextField { get; set; }
        }

        [Fact]
        public void ShouldContainExpectedFormIdValue()
        {
            var descriptor = FormLayoutDescriptor.Build(typeof(TestForm));

            descriptor?.Id.Should().Be(TestFormId);
        }

        [Fact]
        public void ShouldContainExpectedFormNameValue()
        {
            var descriptor = FormLayoutDescriptor.Build(typeof(TestForm));

            descriptor?.Name.Should().Be(TestFormName);
        }

        [Fact]
        public void ShouldContainExpectedFormSections()
        {
            var descriptor = FormLayoutDescriptor.Build(typeof(TestForm));

            descriptor?.HasSections().Should().BeFalse();
            descriptor?.Sections.Should().HaveCount(0);
        }

        [Fact]
        public void ShouldContainExpectedFormProperties()
        {
            var descriptor = FormLayoutDescriptor.Build(typeof(TestForm));

            descriptor?.Properties.Should().HaveCount(1);
        }
    }

    public class SingleSection_FormLayoutDescriptorTests
    {
        private const string TestFormId = "9e015411-a682-4942-b8d7-e30c0ea5c9af";
        private const string TestFormName = "test form";
        private const int Version = 1;
        [Forms.Info(TestFormName, TestFormId,Version)]
        public class TestForm
        {
            [Forms.Text]
            [Forms.Section("SectionName")]
            public string? TextField { get; set; }
        }

        [Fact]
        public void ShouldContainExpectedFormIdValue()
        {
            var descriptor = FormLayoutDescriptor.Build(typeof(TestForm));

            descriptor?.Id.Should().Be(TestFormId);
        }

        [Fact]
        public void ShouldContainExpectedFormNameValue()
        {
            var descriptor = FormLayoutDescriptor.Build(typeof(TestForm));

            descriptor?.Name.Should().Be(TestFormName);
        }

        [Fact]
        public void ShouldContainExpectedFormSections()
        {
            var descriptor = FormLayoutDescriptor.Build(typeof(TestForm));

            descriptor?.HasSections().Should().BeTrue();
            descriptor?.Sections.Should().HaveCount(1);
        }

        [Fact]
        public void ShouldContainExpectedFormProperties()
        {
            var descriptor = FormLayoutDescriptor.Build(typeof(TestForm));

            descriptor?.Properties.Should().HaveCount(1);
        }
    }

    public class MultipleSections_FormLayoutDescriptorTests
    {
        private const string TestFormId = "9e015411-a682-4942-b8d7-e30c0ea5c9af";
        private const string TestFormName = "test form";
        private const int Version = 1;
        [Forms.Info(TestFormName, TestFormId,Version)]
        public class TestForm
        {
            [Forms.Text]
            [Forms.Section("SectionName")]
            public string? TextField { get; set; }

            [Forms.Text]
            [Forms.Section("Section2Name")]
            public string? Text2Field { get; set; }
        }

        [Fact]
        public void ShouldContainExpectedFormIdValue()
        {
            var descriptor = FormLayoutDescriptor.Build(typeof(TestForm));

            descriptor?.Id.Should().Be(TestFormId);
        }

        [Fact]
        public void ShouldContainExpectedFormNameValue()
        {
            var descriptor = FormLayoutDescriptor.Build(typeof(TestForm));

            descriptor?.Name.Should().Be(TestFormName);
        }

        [Fact]
        public void ShouldContainExpectedFormSections()
        {
            var descriptor = FormLayoutDescriptor.Build(typeof(TestForm));

            descriptor?.HasSections().Should().BeTrue();
            descriptor?.Sections.Should().HaveCount(2);
        }

        [Fact]
        public void ShouldContainExpectedFormProperties()
        {
            var descriptor = FormLayoutDescriptor.Build(typeof(TestForm));

            descriptor?.Properties.Should().HaveCount(2);
        }
    }

    public class ConditionalTrigger_FormLayoutDescriptorTests
    {
        private const string TestFormId = "9e015411-a682-4942-b8d7-e30c0ea5c9af";
        private const string TestFormName = "test form";
        private const int Version = 1;
        [Forms.Info(TestFormName, TestFormId,Version)]
        public class TestForm
        {
            [Forms.Boolean(ConditionalTrigger = nameof(TextField))]
            public string? TextField { get; set; }

            [Forms.Text]
            public string? Text2Field { get; set; }
        }
        
        [Fact]
        public void ShouldContainExpectedFormProperties()
        {
            var descriptor = FormLayoutDescriptor.Build(typeof(TestForm));

            descriptor?.Properties.Should().HaveCount(2);
        }

        [Fact]
        public void ShouldContainExpectedFormPropertiesWithConditionalTriggerMetadata()
        {
            var descriptor = FormLayoutDescriptor.Build(typeof(TestForm));

            var conditionalTriggers = descriptor?.Properties.Select(p => p.GetConditionalTrigger()).Where(s => s != null).ToList();
            var conditionalTrigger = conditionalTriggers.FirstOrDefault();

            conditionalTriggers.Should().HaveCount(1);

            conditionalTrigger.Should().Be("TextField");
        }
    }
}
