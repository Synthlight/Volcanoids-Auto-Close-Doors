﻿using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace Auto_Close_Doors.Patches {
    [HarmonyPatch]
    public class DoorUseAnimationPatch {
        [HarmonyTargetMethod]
        public static MethodBase TargetMethod() {
            return typeof(DoorUseAnimation).GetMethod("Use", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        [HarmonyPostfix]
        public static void Postfix(ref DoorUseAnimation __instance, ref GameObject user) {
            if (__instance.CurrentState) return;
            if (!__instance.gameObject.name.Contains("Door")) return;
            var instance = __instance;
            var o = user;
            new Routine(instance, delegate {
                if (!instance.CurrentState) return;
                TargetMethod().Invoke(instance, new object[] {o});
            }, new WaitForSeconds(2f)).Restart();
            __instance.CloseSoon();
        }
    }
}