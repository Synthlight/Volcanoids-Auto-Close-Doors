using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Auto_Close_Doors {
    [UsedImplicitly]
    public class Plugin : GameMod {
        private static readonly GUID COPPER_DOOR_GUID = GUID.Parse("2f37c2f7701fb8b44abefdcd03681b8b");

        public override void Load() {
            Debug.Log("Auto-Close-Doors loaded.");

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        public override void Unload() {
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode) {
            if (scene.name != "Island") return;

            var doorItemDef = GameResources.Instance.Items.First(item => item.AssetId == COPPER_DOOR_GUID);

            // Add to door prefabs.
            if (doorItemDef?.Prefabs != null) {
                foreach (var prefab in doorItemDef.Prefabs) {
                    if (!prefab.HasComponent<AutoCloseComponent>() && prefab.HasComponent<DoorUseAnimation>()) {
                        prefab.AddComponent<AutoCloseComponent>();
                    }
                }
            }

            // Add to existing door objects.
            foreach (var doorAnim in Resources.FindObjectsOfTypeAll<DoorUseAnimation>()) {
                if (!doorAnim.gameObject.HasComponent<AutoCloseComponent>()) {
                    doorAnim.gameObject.AddComponent<AutoCloseComponent>();
                }
            }
        }

        [UsedImplicitly]
        public class AutoCloseComponent : MonoBehaviour {
            private bool canClose;
            private bool waitingForClose;

            public void CloseDoor() {
                if (!gameObject.TryGetComponentSafe(out DoorUseAnimation doorUseAnimation)) return;

                doorUseAnimation.Open(false, false);
                waitingForClose = false;
            }

            [UsedImplicitly]
            public void Update() {
                if (!gameObject.TryGetComponentSafe(out DoorUseAnimation doorUseAnimation)) return;

                // CurrentState true means door is open or it started closing (but not closed yet).
                // TargetState true means door is opening or open.

                // Wait for open, not animating, and not waiting for a delayed close.
                if (doorUseAnimation.TargetState && doorUseAnimation.IsAnimating && !waitingForClose) {
                    canClose = true;
                }

                // Then send close.
                if (doorUseAnimation.TargetState && !doorUseAnimation.IsAnimating && canClose) {
                    canClose        = false;
                    waitingForClose = true; // So we don't keep sending this till it's re-opened.
                    Invoke(nameof(CloseDoor), 2f);
                }
            }
        }
    }
}