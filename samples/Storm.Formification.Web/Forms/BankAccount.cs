using static Storm.Formification.Core.Forms;

namespace Storm.Formification.Web.Forms
{
    [Info("Bank Account", "1557f06a-9908-4c5a-aa84-04036c6c16bb")]
    public class BankAccountForm
    {
        [Text]
        [Section("About your bank")]
        public string? NameOfBank { get; set; }

        [Text]
        [Section("About your bank")]
        public string? Branch { get; set; }

        [Text]
        [Section("About your bank")]
        public string? BranchAddressLine1 { get; set; }

        [Text]
        [Section("About your bank")]
        public string? BranchAddressLine2 { get; set; }

        [Text]
        [Section("About your bank")]
        public string? BranchAddressLine3 { get; set; }

        [Text]
        [Section("About your bank")]
        public string? BranchPostcode { get; set; }

        [Section("About your account")]
        [Text]
        [PreventBrowserAutocomplete]
        public string? SortCode { get; set; }

        [Text]
        [Section("About your account")]
        [PreventBrowserAutocomplete]
        public string? AccountNumber { get; set; }
    }
}
