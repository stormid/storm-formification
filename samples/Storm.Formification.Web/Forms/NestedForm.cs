using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Storm.Formification.Core.Forms;

namespace Storm.Formification.Web.Forms
{
    [Info("Nested form")]
    public class NestedForm
    {
        [Text]
        public string StringField { get; set; }

        [Nested]
        public NestedSubForm Nested { get; set; }
    }

    public class NestedSubForm
    {
        [Text]
        public string StringField { get; set; }

        [Boolean(ConditionalTrigger = nameof(BooleanField))]
        public bool BooleanField { get; set; }

        [Text]
        [ConditionalTarget(nameof(BooleanField))]
        public string ConditionalField { get; set; }

        [Nested]
        [ConditionalTarget(nameof(BooleanField))]
        public NestedSubSubForm ConditionalForm { get; set; }
    }

    public class NestedSubSubForm
    {
        public string NestedStringField { get; set; }
    }
}
