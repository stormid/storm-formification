using System;
using System.IO;
using Storm.Formification.Core;
using Storm.Formification.Core.Infrastructure;
using static Storm.Formification.Core.Forms;

namespace Storm.Formification.WebWithDb.Forms.Preferences
{
    [Info("Preferences")]
    public class PreferencesForm
    {
        [Section("Personal")]
        [Text]
        [HintLabel("Who are you?")]
        public string Name { get; set; }

        [Section("Personal")]
        [Date]
        public DateTime DateOfBirth { get; set; }

        [Section("Personal")]
        [HintLabel("Select a user")]
        [Choice]
        [ChoiceDataSource(typeof(AvailableUsersDatasource))]
        public string Email { get; set; }

        [Section("Misc")]
        [MultilineText]
        public string Notes { get; set; }
    }
}
