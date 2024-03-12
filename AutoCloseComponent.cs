using JetBrains.Annotations;
using UnityEngine;

namespace Auto_Close_Doors;

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