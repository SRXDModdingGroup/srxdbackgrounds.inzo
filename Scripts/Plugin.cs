using BepInEx;

namespace SRXDBackgrounds.Inzo {
    [BepInDependency("srxd.customvisuals", "1.2.5")]
    [BepInDependency("srxd.backgrounds.common","1.0.2")]
    [BepInPlugin("srxd.backgrounds.inzo", "srxdbackgrounds.inzo", "1.0.2")]
    public class Plugin : BaseUnityPlugin {
        private void Awake() => Destroy(gameObject);
    }
}
