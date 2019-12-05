using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Storm.Formification.Core
{
    public static class Forms
    {
        public interface IAmConditionalTriggerAware
        {
            string? ConditionalTrigger { get; set; }
        }

        [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
        public class ConditionalTriggerAttribute : Attribute
        {
            public ConditionalTriggerAttribute(string key)
            {
                Key = key;
            }

            public string Key { get; }
        }

        [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
        public class ConditionalTargetAttribute : Attribute
        {
            public ConditionalTargetAttribute(string triggerKey)
            {
                TriggerKey = triggerKey;
            }

            public string TriggerKey { get; }
        }

        [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
        public class ChoiceAttribute : DataTypeAttribute, IAmConditionalTriggerAware
        {
            public ChoiceAttribute() : base("Forms__Choice")
            {
            }

            public string? ConditionalTrigger { get; set; }
        }

        [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
        public class TextAttribute : DataTypeAttribute
        {
            public TextAttribute() : base("Forms__Text")
            {

            }
        }

        [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
        public class UploadAttribute : DataTypeAttribute
        {
            public string? AllowedExtensions { get; set; }

            public int? MaximumSizeInBytes { get; set; }

            public UploadAttribute() : base("Forms__Upload")
            {

            }
        }

        [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
        public class DateAttribute : DataTypeAttribute
        {
            public DateAttribute() : base("Forms__Date")
            {
            }
        }

        [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
        public class NestedAttribute : DataTypeAttribute
        {
            public NestedAttribute() : base("Forms__NestedForm")
            {

            }
        }

        [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
        public class DateOffsetAttribute : DataTypeAttribute
        {
            public DateOffsetAttribute() : base("Forms__DateTimeOffset")
            {

            }
        }

        [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
        public class MultilineTextAttribute : DataTypeAttribute
        {
            public MultilineTextAttribute() : base("Forms__MultilineText")
            {

            }
        }

        [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
        public class NumericAttribute : DataTypeAttribute
        {
            public NumericAttribute() : base("Forms__Numeric")
            {

            }
        }

        [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
        public class BooleanAttribute : DataTypeAttribute, IAmConditionalTriggerAware
        {
            public BooleanAttribute() : base("Forms__Boolean")
            {

            }

            public string? ConditionalTrigger { get; set; }
        }
        
        [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
        public class DateMonthYearAttribute : DataTypeAttribute
        {
            public enum DayValueOption
            {
                FirstDay,
                LastDay
            }

            public DateMonthYearAttribute() : base("Forms__DateMonthYear")
            {
            }

            public DayValueOption DayValue { get; set; } = DayValueOption.FirstDay;

            public string DateFormatString { get; set; } = "MM/yy";

            public string MonthFormatString { get; set; } = "MM";

            public string YearFormatString { get; set; } = "yy";
        }


        [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
        public class EmailAttribute : DataTypeAttribute
        {
            private const string EmailPattern = @"^[a-zA-Z0-9.!#$%&'*+\/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$";

            private static Regex PatternMatcher = new Regex(EmailPattern, RegexOptions.Singleline | RegexOptions.CultureInvariant);

            public EmailAttribute() : base("Forms__Email")
            {
            }

            public override string FormatErrorMessage(string name)
            {
                return $"'{name}' is not a valid email address";
            }

            private bool IsPatternMatch(string value)
            {
                return PatternMatcher.IsMatch(value);
            }

            public override bool IsValid(object value)
            {
                if (value == null)
                {
                    return true;
                }

                if (!(value is string valueAsString))
                {
                    return false;
                }

                // only return true if there is only 1 '@' character
                // and it is neither the first nor the last character
                int index = valueAsString.IndexOf('@');

                return
                    index > 0 &&
                    index != valueAsString.Length - 1 &&
                    index == valueAsString.LastIndexOf('@') &&
                    IsPatternMatch(valueAsString);
            }
        }

        public interface IInfo
        {
            string Slug { get; }
            string Name { get; }
            string Id { get; }
        }

        [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
        public sealed class InfoAttribute : Attribute, IInfo
        {
            public InfoAttribute(string name, string id)
            {
                Name = name;
                Id = id;
                Slug = name.Kebaberize();
            }

            public string Slug { get; set; }
            public string Name { get; }
            public string Id { get; }
        }

        [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
        public sealed class SectionAttribute : Attribute
        {
            public SectionAttribute(string name)
            {
                Name = name;
            }

            public string Name { get; }
        }

        [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
        internal class FormsControllerNameAttribute : Attribute, IControllerModelConvention
        {
            public void Apply(ControllerModel controller)
            {
                if (controller.ControllerType.GetGenericTypeDefinition() == typeof(FormsController<>))
                {
                    var entityType = controller.ControllerType.GenericTypeArguments[0];

                    var formAttribute = entityType.GetCustomAttribute<InfoAttribute>();

                    if (formAttribute != null)
                    {
                        controller.ControllerName = formAttribute.Slug;
                    }
                }
            }
        }

        [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
        public class HandlerAttribute : Attribute
        {
            public HandlerAttribute(Type handlerType)
            {
                if(!handlerType.IsConstructedGenericType || !handlerType.IsClass || handlerType.IsAbstract)
                {
                    throw new InvalidCastException($"{nameof(handlerType)} must be a non-abstract, constructable class type");
                }

                HandlerType = handlerType;
            }

            public Type HandlerType { get; }
        }

        public interface IFormActions<TForm> where TForm : class, new()
        {
            Task<TForm?> Retrieve(Guid id, CancellationToken cancellationToken = default(CancellationToken));
            Task<Guid> Save(Guid id, TForm form, CancellationToken cancellationToken = default(CancellationToken));
        }

        public class DefaultFormActions<TForm> : IFormActions<TForm> where TForm : class, new()
        {
            public Task<TForm?> Retrieve(Guid id, CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.FromResult<TForm?>(new TForm());
            }

            public Task<Guid> Save(Guid id, TForm form, CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.FromResult(id);
            }
        }

        [FormsControllerName]
        [Area("Forms")]
        [Route("[area]/[controller]")]
        public class FormsController<TForm> : Controller
            where TForm : class, new()
        {
            private readonly IFormActions<TForm> formActions;

            public FormsController(IFormActions<TForm> formActions)
            {
                this.formActions = formActions;
            }

            [HttpGet]
            [Route("")]
            public async Task<IActionResult> Index()
            {
                var form = await formActions.Retrieve(Guid.Empty);
                return View(form);
            }


            [HttpGet]
            [Route("{id:guid}")]
            public async Task<IActionResult> Index(Guid id, CancellationToken cancellationToken)
            {
                var form = await formActions.Retrieve(id, cancellationToken);
                ViewData.Add("id", id);
                return View(form);
            }

            [HttpPost]
            [Route("{id:guid?}")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Index(Guid? id, TForm form, CancellationToken cancellationToken)
            {
                if (ModelState.IsValid)
                {
                    var savedId = await formActions.Save(id.GetValueOrDefault(Guid.NewGuid()), form, cancellationToken);
                }

                return View(form);
            }

            [HttpGet]
            [Route("{id:guid}/view")]
            public async Task<IActionResult> Display(Guid id, CancellationToken cancellationToken)
            {
                var form = await formActions.Retrieve(id, cancellationToken);
                ViewData.Add("id", id);
                return View(form);
            }
        }
    }
}
