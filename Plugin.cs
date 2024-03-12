using Base_Mod;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Auto_Close_Doors;

[UsedImplicitly]
public class Plugin : BaseGameMod {
    private static readonly GUID COPPER_DOOR_GUID = GUID.Parse("2f37c2f7701fb8b44abefdcd03681b8b");

    // Add to door prefabs.
    public override void OnInitData() {
        foreach (var prefab in Lib.GetItemDefinitionWithComponent<DoorUseAnimation>(COPPER_DOOR_GUID)) {
            if (!prefab.HasComponent<AutoCloseComponent>() && prefab.HasComponent<DoorUseAnimation>()) {
                prefab.AddComponent<AutoCloseComponent>();
            }
        }

        base.OnInitData();
    }

    // Add to existing door objects.
    public override void OnGameLoaded(Scene scene) {
        foreach (var doorAnim in Resources.FindObjectsOfTypeAll<DoorUseAnimation>()) {
            if (!doorAnim.gameObject.HasComponent<AutoCloseComponent>()) {
                doorAnim.gameObject.AddComponent<AutoCloseComponent>();
            }
        }

        base.OnGameLoaded(scene);
    }
}