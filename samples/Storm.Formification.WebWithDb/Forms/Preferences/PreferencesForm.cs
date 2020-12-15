using System;
using System.IO;
using Storm.Formification.Core;
using Storm.Formification.Core.Infrastructure;
using static Storm.Formification.Core.Forms;

namespace Storm.Formification.WebWithDb.Forms.Preferences
{
    [Info("Preferences", "2a0467a7-ae00-40ef-b1c3-8b009c1b32bb",Version)]
    public class PreferencesForm
    {
        private const int Version = 1;

        [Section("Personal")]
        [Text]
        [HintLabel("Who are you?")]
        public string? Name { get; set; }

        [Section("Personal")]
        [Date]
        public DateTime DateOfBirth { get; set; }

        [Section("Personal")]
        [HintLabel("Select a user")]
        [Choice]
        [ChoiceDataSource(typeof(AvailableUsersDatasource))]
        public string? Email { get; set; }

        [Section("Misc")]
        [MultilineText]
        public string? Notes { get; set; }
    }
}
