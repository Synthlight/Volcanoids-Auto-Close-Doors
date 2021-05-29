using System.Linq;
using Base_Mod;
using JetBrains.Annotations;
using UnityEngine;

namespace Auto_Close_Doors {
    [UsedImplicitly]
    public class Plugin : BaseGameMod {
        protected override      string ModName => "Auto-Close-Doors";
        private static readonly GUID   COPPER_DOOR_GUID = GUID.Parse("2f37c2f7701fb8b44abefdcd03681b8b");

        // Add to door prefabs.
        public override void OnInitData() {
            var doorItemDef = GameResources.Instance.Items.First(item => item.AssetId == COPPER_DOOR_GUID);

            if (doorItemDef.Prefabs != null) {
                foreach (var prefab in doorItemDef.Prefabs) {
                    if (!prefab.HasComponent<AutoCloseComponent>() && prefab.HasComponent<DoorUseAnimation>()) {
                        prefab.AddComponent<AutoCloseComponent>();
                    }
                }
            }

            base.OnInitData();
        }

        // Add to existing door objects.
        public override void OnGameLoaded() {
            foreach (var doorAnim in Resources.FindObjectsOfTypeAll<DoorUseAnimation>()) {
                if (!doorAnim.gameObject.HasComponent<AutoCloseComponent>()) {
                    doorAnim.gameObject.AddComponent<AutoCloseComponent>();
                }
            }

            base.OnGameLoaded();
        }
    }
}