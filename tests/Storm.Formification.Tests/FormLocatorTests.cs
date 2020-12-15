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
    public class FormLocatorTests
    {
        public const string TestEmptyFormId = "a0dcb904-271a-4475-878c-2c6059fcc307";
        private const int Version = 1;
        [Forms.Info("Test empty form", TestEmptyFormId,Version)]
        public class TestEmptyForm
        {

        }

        public class NotAForm
        {

        }

        [Fact]
        public void ShouldRetrieveFormInfoFromId()
        {
            var formLocator = new FormLocator(typeof(TestEmptyForm));

            var formType = formLocator.Get(TestEmptyFormId);

            formType.Should().Be<TestEmptyForm>();
        }

        [Fact]
        public void ShouldOnlyRetrieveFormInfoForFormsWithInfoAttribute()
        {
            var formLocator = new FormLocator(typeof(NotAForm), typeof(TestEmptyForm));

            formLocator.All().Should().NotContain(typeof(NotAForm));
            formLocator.All().Should().Contain(typeof(TestEmptyForm));
        }

        [Fact]
        public void ShouldRetrieveInfo()
        {
            var formLocator = new FormLocator(typeof(NotAForm), typeof(TestEmptyForm));

            var info = formLocator.Info(TestEmptyFormId);

            info?.Id.Should().Be(TestEmptyFormId);
            info?.Name.Should().Be("Test empty form");
            info?.Slug.Should().Be("test-empty-form");
        }

        [Fact]
        public void ShouldNotRetrieveInfoForInvalidForm()
        {
            var formLocator = new FormLocator(typeof(NotAForm), typeof(TestEmptyForm));

            var info = formLocator.Info("");

            info.Should().BeNull();
        }

        [Fact]
        public void ShouldRetrieveFormDescriptorById()
        {
            var formLocator = new FormLocator(typeof(NotAForm), typeof(TestEmptyForm));

            var descriptor = formLocator.GetFormLayoutDescriptor(TestEmptyFormId);

            descriptor.Should().NotBeNull();
            descriptor?.Id.Should().Be(TestEmptyFormId);
            descriptor?.Name.Should().Be("Test empty form");
            descriptor?.Properties.Should().BeEmpty();
            descriptor?.Sections.Should().BeEmpty();
        }

        [Fact]
        public void ShouldRetrieveFormDescriptorByType()
        {
            var formLocator = new FormLocator(typeof(NotAForm), typeof(TestEmptyForm));

            var descriptor = formLocator.GetFormLayoutDescriptor(typeof(TestEmptyForm));

            descriptor.Should().NotBeNull();
            descriptor?.Id.Should().Be(TestEmptyFormId);
            descriptor?.Name.Should().Be("Test empty form");
            descriptor?.Properties.Should().BeEmpty();
            descriptor?.Sections.Should().BeEmpty();
        }

        [Fact]
        public void ShouldRetrieveFormDescriptorByInfo()
        {
            var formLocator = new FormLocator(typeof(NotAForm), typeof(TestEmptyForm));

            var formInfo = formLocator.Info(typeof(TestEmptyForm));

            if(formInfo == null)
            {
                throw new NullReferenceException("formInfo should not be null");
            }

            var descriptor = formLocator.GetFormLayoutDescriptor(formInfo);

            descriptor.Should().NotBeNull();
            descriptor?.Id.Should().Be(TestEmptyFormId);
            descriptor?.Name.Should().Be("Test empty form");
            descriptor?.Properties.Should().BeEmpty();
            descriptor?.Sections.Should().BeEmpty();
        }

        [Fact]
        public void ShouldNotRetrieveFormDescriptorByTypeForInvalidFormType()
        {
            var formLocator = new FormLocator(typeof(NotAForm), typeof(TestEmptyForm));

            var descriptor = formLocator.GetFormLayoutDescriptor(typeof(NotAForm));
            descriptor.Should().BeNull();
        }
    }
}
