using RimWorld;
using Verse;
using HarmonyLib;
using UnityEngine;

namespace RimDungeon
{
    [HarmonyPatch(typeof(Building_TurretGun), "IsValidTarget")]
    public static class IsValidTarget
    {

        public static bool Prefix(Building_TurretGun __instance, Thing t)
        {
        if (!PublicFunctions.WithinFiringArcOf(__instance.Position, t.Position, __instance.Rotation, Turret_Def.Get(__instance.def).firingArc))
            {
                return false;
            }
            return true;
        }
        

    }

    [HarmonyPatch(typeof(Building_TurretGun), nameof(Building_TurretGun.OrderAttack))]
    public static class OrderAttack
    {

        public static bool Prefix(Building_TurretGun __instance, LocalTargetInfo targ)
        {
            if (targ.IsValid && !PublicFunctions.WithinFiringArcOf(__instance.Position,targ.Cell, __instance.Rotation, Turret_Def.Get(__instance.def).firingArc))
            {
                Messages.Message("MessageTargetBeyondMaximumRange".Translate(), MessageTypeDefOf.RejectInput, false);
                return false;
            }
            return true;
        }

    }
}
