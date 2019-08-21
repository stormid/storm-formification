using Storm.Formification.Core;
using Storm.Formification.Core.Infrastructure;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using static Storm.Formification.Core.Forms;

namespace Storm.Formification.Web.Forms
{
    [Info("Kitchen sink", "85f6c9a9-f223-40ad-8b03-4c655ba761f7")]
    public class KitchenSink
    {
        [Text]
        public string StringField { get; set; }

        [MultilineText]
        [HintLabel("Optional")]
        public string MultilineStringField { get; set; }

        [Numeric]
        public int NumericField { get; set; }

        [Boolean]
        public bool BooleanField { get; set; }

        [Date]
        [HintLabel("Must be a valid date")]
        public DateTime DateField { get; set; }

        [Upload]
        [HintLabel("Maximum file size of 5MB")]
        public IFormFile UploadField { get; set; }

        [Choice]
        public DayOfWeek? OptionalSingleSelect { get; set; }

        [Choice]
        public DayOfWeek RequiredSingleSelect { get; set; }

        [Choice]
        public IEnumerable<DayOfWeek> MultiSelect { get; set; }

        [Choice(ConditionalTrigger = nameof(ColourSelect))]
        [HintLabel("Be colourful!")]
        public ConsoleColor ColourSelect { get; set; }

        [Choice]
        [ChoiceDataSource(typeof(ListOfPostcodesDataSource))]
        public string Postcode { get; set; }

        [Choice]
        [ChoiceDataSource(typeof(ListOfOptionsDataSource))]
        public int? FromListOfOptions { get; set; }

        [Boolean(ConditionalTrigger = nameof(TriggeredConditional))]
        public bool TriggeredConditional { get; set; }

        [Text]
        [ConditionalTarget(nameof(TriggeredConditional))]
        public string ConditionallyTriggered { get; set; }

        [Text]
        [ConditionalTarget(nameof(TriggeredConditional))]
        public string ConditionallyTriggeredAlso { get; set; }

        public class ListOfOptionsDataSource : IChoiceDataSource
        {
            private readonly IList<ChoiceItem> choices = new List<ChoiceItem> { new ChoiceItem("1", "Option 1"), new ChoiceItem("2", "Option 2"), new ChoiceItem("3", "Option 3"), };
            public async Task<IEnumerable<ChoiceItem>> GetAsync()
            {
                return await Task.FromResult(choices);
            }

            public async Task<ChoiceItem> GetAsync(string value)
            {
                return (await GetAsync()).FirstOrDefault(c => c.Value == value);
            }
        }

        public class ListOfPostcodesDataSource : IChoiceDataSource
        {
            private readonly string postcode;
            private readonly HttpClient client;

            public ListOfPostcodesDataSource(IHttpClientFactory httpClientFactory)
            {
                postcode = "eh6";
                client = httpClientFactory.CreateClient();
            }

            public async Task<IEnumerable<ChoiceItem>> GetAsync()
            {
                try
                {
                    var data = await client.GetStringAsync($"https://api.postcodes.io/postcodes?q={postcode}&limit=100");
                    var result = JsonConvert.DeserializeObject<Root>(data);
                    if (result.status == 200)
                    {
                        return result.result.GroupBy(g => g.nhs_ha).SelectMany(c => c.Select(s => new ChoiceItem(s.postcode, s.postcode, c.Key)));
                    }
                }
                catch
                {
                    // argh, testing!
                }

                return Enumerable.Empty<ChoiceItem>();
            }

            public async Task<ChoiceItem> GetAsync(string value)
            {
                try
                {
                    var data = await client.GetStringAsync($"https://api.postcodes.io/postcodes?q={value}&limit=1");
                    var result = JsonConvert.DeserializeObject<Root>(data);
                    if (result.status == 200)
                    {
                        var item = result.result.FirstOrDefault();
                        return new ChoiceItem(item.postcode, item.postcode);
                    }
                }
                catch
                {
                    // argh, testing!
                }

                return new ChoiceItem();
            }

            public class Root
            {
                public int status { get; set; }
                public Result[] result { get; set; }
            }

            public class Result
            {
                public string postcode { get; set; }
                public string nhs_ha { get; set; }
                public string primary_care_trust { get; set; }

            }
        }

        public class Validator : AbstractValidator<KitchenSink>
        {
            public Validator()
            {
                RuleFor(r => r.StringField).NotEmpty();
            }
        }
    }
}
