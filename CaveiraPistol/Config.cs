using Exiled.API.Enums;
using Exiled.API.Interfaces;
using System.ComponentModel;

namespace CaveiraPistol
{
    public class Config : IConfig
    {
        [Description("Whether the plugin is enabled.")]
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;
        [Description("Notifies when rampage is activated.")]
        public bool Hint { get; set; } = true;
        public float Damage { get; set; } = 50f;
        public float RampageDamageMultiplier { get; set; } = 3f;
        public float RampageDuration { get; set; } = 10;
        [Description("Check EXILED/Exiled.API/Enums/SpawnLocationType.cs")]
        public SpawnLocationType SpawnLocation { get; set; } = SpawnLocationType.Inside079Secondary;
        [Description("Should rampage be active when you affected by:")]
        public bool Scp207 { get; set; } = false;
        public bool Scp1853 { get; set; } = false;
        public bool Antiscp207 { get; set; } = false;
    }
}