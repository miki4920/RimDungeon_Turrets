using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using RimWorld;
using Verse;
using HarmonyLib;

namespace RimDungeon
{
    [HarmonyPatch(typeof(Verb), nameof(Verb.CanHitTargetFrom))]
    public static class CanHitTargetFrom
    {
        public static void Postfix(Verb  __instance, LocalTargetInfo targ, ref bool __result)
        {
            IntVec3 position_caster = __instance.caster.Position;
            IntVec3 position_target = targ.Cell;
            Rot4 rot = __instance.caster.Rotation;
            Turret_Def TurretDef = __instance.caster.def.GetModExtension<Turret_Def>();
            if(TurretDef != null && TurretDef.firingArc != 360)
            {
                __result = __result && PublicFunctions.WithinFiringArcOf(position_caster, position_target, rot, TurretDef.firingArc);
            }
        }

    }

    [HarmonyPatch(typeof(Verb), nameof(Verb.DrawHighlight))]
    public static class DrawHighlight
    {
        public static bool Prefix(Verb __instance, LocalTargetInfo target)
        {
            float arc = Turret_Def.Get(__instance.caster.def).firingArc;
            if (arc == 360)
            {
                return true;
            }
            PublicFunctions.TryDrawFiringCone(__instance.caster.Position, __instance.caster.Rotation, __instance.verbProps.range, arc);
            if (target.IsValid)
            {
                var baseClass = __instance.GetType();
                MethodInfo drawHighlightFieldRadiusAroundTargetMethod = baseClass.GetMethod("DrawHighlightFieldRadiusAroundTarget", BindingFlags.NonPublic | BindingFlags.Instance);
                GenDraw.DrawTargetHighlight(target);
                drawHighlightFieldRadiusAroundTargetMethod.Invoke(__instance, new object[] { target });
            }
            return false;
        }
    }
}
