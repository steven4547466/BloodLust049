using Exiled.API.Interfaces;
using System.ComponentModel;

namespace BloodLust049
{
    public sealed class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;

        [Description("Show debug logs.")]
        public bool Debug { get; set; } = false;

        [Description("Duration of blood lust (in seconds).")]
        public float BloodLustDuration { get; set; } = 10f;

        [Description("Minimum first time 049 will get blood lust mode (in seconds).")]
        public float MinimumFirstTime { get; set; } = 120f;

        [Description("Maximum first time 049 will get blood lust mode (in seconds).")]
        public float MaximumFirstTime { get; set; } = 240f;

        [Description("Minimum time 049 will return to blood lust mode (in seconds).")]
        public float MinimumWaitTime { get; set; } = 60f;

        [Description("Maximum time 049 will return to blood lust mode (in seconds).")]
        public float MaximumWaitTime { get; set; } = 240f;

        [Description("Intensity of 207 when 049 enters blood lust.")]
        public int Scp207Intensity { get; set; } = 4;

        [Description("Disables instant revive.")]
        public bool DisableInstantRevive { get; set; } = false;

        [Description("Whether or not 049 should always instant revive instead of just in a blood lust mode (because of the 049 bug). This is disregarded if DisabeInstantRevive is true.")]
        public bool AlwaysInstantRevive { get; set; } = false;

        [Description("Enables adrenaline camera effect.")]
        public bool EnableAdrenalineEffect { get; set; } = true;

        [Description("Enables bleeding camera effect.")]
        public bool EnableBleedEffect { get; set; } = true;

        [Description("The message 049 will see when he enters blood lust.")]
        public string BloodLustMessage { get; set; } = "<color=\"red\">You have entered blood lust (Hold shift!).</color>";
    }
}
