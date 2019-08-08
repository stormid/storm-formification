using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.Extensions.Primitives;
using Storm.Formification.Core;
using Storm.Formification.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Xunit;

namespace Storm.Formification.Tests
{
    public class DateTimeOffsetModelBinderTests
    {
        private static DefaultModelBindingContext GetBindingContext(IValueProvider valueProvider, Type modelType)
        {
            var metadataProvider = new EmptyModelMetadataProvider();

            var metadata = metadataProvider.GetMetadataForType(modelType);

            var bindingContext = new DefaultModelBindingContext
            {
                ModelMetadata = metadata,
                ModelName = modelType.Name,
                ModelState = new ModelStateDictionary(),
                ValueProvider = valueProvider,
            };
            return bindingContext;
        }

        [Fact]
        public async Task ShouldNotBindIncompleteValueCollectionForDateTime_MissingDay()
        {
            var formCollection = new FormCollection(new Dictionary<string, StringValues>()
            {
                { "Month", "1" },
                { "Year", "1990" },
            });

            var binder = new DateTimeModelBinder();

            var vp = new FormValueProvider(BindingSource.Form, formCollection, CultureInfo.CurrentCulture);

            var context = GetBindingContext(vp, typeof(DateTime));

            await binder.BindModelAsync(context);

            context.Result.Model.Should().BeNull();
        }

        [Fact]
        public async Task ShouldNotBindIncompleteValueCollectionForDateTime_MissingMonth()
        {
            var formCollection = new FormCollection(new Dictionary<string, StringValues>()
            {
                { "Day", "1" },
                { "Year", "1990" },
            });

            var binder = new DateTimeModelBinder();

            var vp = new FormValueProvider(BindingSource.Form, formCollection, CultureInfo.CurrentCulture);

            var context = GetBindingContext(vp, typeof(DateTime));

            await binder.BindModelAsync(context);

            context.Result.Model.Should().BeNull();
        }

        [Fact]
        public async Task ShouldNotBindIncompleteValueCollectionForDateTime_MissingYear()
        {
            var formCollection = new FormCollection(new Dictionary<string, StringValues>()
            {
                { "Day", "1" },
                { "Month", "12" },
            });

            var binder = new DateTimeModelBinder();

            var vp = new FormValueProvider(BindingSource.Form, formCollection, CultureInfo.CurrentCulture);

            var context = GetBindingContext(vp, typeof(DateTime));

            await binder.BindModelAsync(context);

            context.Result.Model.Should().BeNull();
        }

        [Fact]
        public async Task ShouldNotBindIncompleteValueCollectionForNullableDateTime_MissingDay()
        {
            var formCollection = new FormCollection(new Dictionary<string, StringValues>()
            {
                { "Month", "1" },
                { "Year", "1990" },
            });

            var binder = new NullableDateTimeModelBinder();

            var vp = new FormValueProvider(BindingSource.Form, formCollection, CultureInfo.CurrentCulture);

            var context = GetBindingContext(vp, typeof(DateTime));

            await binder.BindModelAsync(context);

            context.Result.Model.Should().BeNull();
        }

        [Fact]
        public async Task ShouldNotBindIncompleteValueCollectionForNullableDateTime_MissingMonth()
        {
            var formCollection = new FormCollection(new Dictionary<string, StringValues>()
            {
                { "Day", "1" },
                { "Year", "1990" },
            });

            var binder = new NullableDateTimeModelBinder();

            var vp = new FormValueProvider(BindingSource.Form, formCollection, CultureInfo.CurrentCulture);

            var context = GetBindingContext(vp, typeof(DateTime));

            await binder.BindModelAsync(context);

            context.Result.Model.Should().BeNull();
        }

        [Fact]
        public async Task ShouldNotBindIncompleteValueCollectionForNullableDateTime_MissingYear()
        {
            var formCollection = new FormCollection(new Dictionary<string, StringValues>()
            {
                { "Day", "1" },
                { "Month", "12" },
            });

            var binder = new NullableDateTimeModelBinder();

            var vp = new FormValueProvider(BindingSource.Form, formCollection, CultureInfo.CurrentCulture);

            var context = GetBindingContext(vp, typeof(DateTime));

            await binder.BindModelAsync(context);

            context.Result.Model.Should().BeNull();
        }

        [Theory]
        [InlineData(1990, 12, 31, true)]
        [InlineData(1990, 13, 31, false)]
        [InlineData(null, null, null, false)]
        public async Task ShouldBindDateTimeAsModel(int? year, int? month, int? day, bool isValid)
        {
            var formCollection = new FormCollection(new Dictionary<string, StringValues>()
            {
                { "Day", day.ToString() },
                { "Month", month.ToString() },
                { "Year", year.ToString() },
            });

            var binder = new DateTimeModelBinder();

            var vp = new FormValueProvider(BindingSource.Form, formCollection, CultureInfo.CurrentCulture);

            var context = GetBindingContext(vp, typeof(DateTime));

            await binder.BindModelAsync(context);

            context.ModelState.IsValid.Should().Be(isValid);

            if (isValid)
            {
                var dateValue = (DateTime)context.Result.Model;

                dateValue.Date.Day.Should().Be(day);
                dateValue.Date.Month.Should().Be(month);
                dateValue.Date.Year.Should().Be(year);
                dateValue.TimeOfDay.Hours.Should().Be(0);
                dateValue.TimeOfDay.Minutes.Should().Be(0);
                dateValue.TimeOfDay.Seconds.Should().Be(0);
            }
            else
            {
                context.ModelState.ErrorCount.Should().Be(1);
            }
        }

        [Theory]
        [InlineData(1990, 12, 31, true)]
        [InlineData(1990, 13, 31, false)]
        [InlineData(null, 12, 31, false)]
        [InlineData(1990, null, 31, false)]
        [InlineData(1990, 12, null, false)]
        [InlineData(null, null, null, true)]
        public async Task ShouldBindNullableDateTimeAsModel(int? year, int? month, int? day, bool isValid)
        {
            var formCollection = new FormCollection(new Dictionary<string, StringValues>()
            {
                { "Day", day.ToString() },
                { "Month", month.ToString() },
                { "Year", year.ToString() },
            });

            var binder = new NullableDateTimeModelBinder();

            var vp = new FormValueProvider(BindingSource.Form, formCollection, CultureInfo.CurrentCulture);

            var context = GetBindingContext(vp, typeof(DateTime?));

            await binder.BindModelAsync(context);

            context.ModelState.IsValid.Should().Be(isValid);

            var dateValue = (DateTime?)context.Result.Model;

            if (isValid)
            {
                if (dateValue.HasValue)
                {
                    dateValue.Value.Date.Day.Should().Be(day);
                    dateValue.Value.Date.Month.Should().Be(month);
                    dateValue.Value.Date.Year.Should().Be(year);
                    dateValue.Value.TimeOfDay.Hours.Should().Be(0);
                    dateValue.Value.TimeOfDay.Minutes.Should().Be(0);
                    dateValue.Value.TimeOfDay.Seconds.Should().Be(0);
                }
            }
            else
            {
                context.ModelState.ErrorCount.Should().Be(1);
            }
        }
    }
}
