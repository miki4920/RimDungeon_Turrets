using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;
using HarmonyLib;

namespace RimDungeon
{
    [HarmonyPatch(typeof(Command_VerbTarget), nameof(Command_VerbTarget.GizmoUpdateOnMouseover))]
    public static class GizmoUpdateOnMouseover
    {
        public static bool Prefix(Command_VerbTarget __instance)
        {
            Turret_Def TurretDef = Turret_Def.Get(__instance.verb.caster.def);
            if(TurretDef.firingArc == 360)
            {
                return true;
            }
            float range = __instance.verb.verbProps.range;
            float minRange = __instance.verb.verbProps.minRange;
            if (range < 56.4)
            {
                PublicFunctions.TryDrawFiringCone(__instance.verb.caster.Position, __instance.verb.caster.Rotation, __instance.verb.verbProps.range, TurretDef.firingArc);
            }
            if(minRange < 56.4)
            {
                PublicFunctions.TryDrawFiringCone(__instance.verb.caster.Position, __instance.verb.caster.Rotation, __instance.verb.verbProps.minRange, TurretDef.firingArc);
            }
            return false;
        }

    }
}