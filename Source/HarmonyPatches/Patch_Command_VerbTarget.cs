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
            PublicFunctions.TryDrawFiringCone(__instance.verb.caster.Position, __instance.verb.caster.Rotation, __instance.verb.verbProps.range, TurretDef.firingArc);
            return false;
        }

    }
}