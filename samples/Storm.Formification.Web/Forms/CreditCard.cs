using System.ComponentModel.DataAnnotations;
using static Storm.Formification.Core.Forms;

namespace Storm.Formification.Web.Forms
{
    [Info("Credit card", Id)]
    public class CreditCard
    {
        public const string Id = "69CE927B-0315-4DD0-91AB-08ED483EEE95";

        [Text]
        [Required]
        [MaxLength(25)]
        public string Issuer { get; set; }

        [Text]
        public string NameOnCard { get; set; }

        [DateMonthYear]
        [Required]
        public string ValidFrom { get; set; }

        [DateMonthYear]
        [Required]
        public string ValidTo { get; set; }
    }
}
