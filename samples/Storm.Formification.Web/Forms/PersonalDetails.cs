using static Storm.Formification.Core.Forms;

namespace Storm.Formification.Web.Forms
{
    [Info("Personal details")]
    public class PersonalDetailsForm
    {
        [Text]
        public string Firstname { get; set; }

        [Text]
        public string Lastname { get; set; }

        [Text]
        public string AddressLine1 { get; set; }

        [Text]
        public string AddressLine2 { get; set; }

        [Text]
        public string AddressLine3 { get; set; }

        [Text]
        public string Postcode { get; set; }
    }
}
