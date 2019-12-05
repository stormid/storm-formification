using static Storm.Formification.Core.Forms;

namespace Storm.Formification.Web.Forms
{
    [Info("Personal details", "c61b2d00-14ae-4973-8892-b55c7c81f4d9")]
    public class PersonalDetailsForm
    {
        [Text]
        public string? Firstname { get; set; }

        [Text]
        public string? Lastname { get; set; }

        [Text]
        public string? AddressLine1 { get; set; }

        [Text]
        public string? AddressLine2 { get; set; }

        [Text]
        public string? AddressLine3 { get; set; }

        [Text]
        public string? Postcode { get; set; }
    }
}
