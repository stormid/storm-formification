using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Storm.Formification.Core.Forms;

namespace Storm.Formification.Web.Forms
{
    [Info("Form with sections", Id,Version)]
    public class FormWithSections
    {
        public const string Id = "form-with-sections-1";
        private const int Version = 1;

        [Text, Section("About me")]
        public string? Title { get; set; }
        
        [Text, Section("About me")] 
        public string? Firstname { get; set; }
        
        [Text, Section("About me")] 
        public string? Surname { get; set; }

        [Text, Section("Home address")]
        public string? AddressLine1 { get; set; }

        [Text, Section("Home address")]
        public string? AddressLine2 { get; set; }

        [Text, Section("Home address")]
        public string? AddressLine3 { get; set; }

        [Text, Section("Home address")]
        public string? AddressLine4 { get; set; }

        [Text, Section("Home address")]
        public string? Postcode { get; set; }

        [Text, Section("Employment")]
        public string? JobTitle { get; set; }

        [Text, Section("Employment")]
        public string? PlaceOfWork { get; set; }
    }
}
