using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace Auto_Close_Doors {
    [BepInPlugin(UUID, "Auto-Close-Doors Mod", "1.0.0.0")]
    public class AutoCloseDoorsMod : BaseUnityPlugin {
        private const string UUID = "com.autoCloseDoors";
        private static ManualLogSource logSource;

        public void Awake() {
            logSource = Logger;

            Log(LogLevel.Info, "Auto-Close-Doors loaded.");

            var harmony = new Harmony(UUID);
            harmony.PatchAll();

            foreach (var patchedMethod in harmony.GetPatchedMethods()) {
                Log(LogLevel.Info, $"Patched: {patchedMethod.DeclaringType?.FullName}:{patchedMethod}");
            }
        }

        public static void Log(LogLevel level, string msg) {
            logSource.Log(level, msg);
        }
    }
}