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
    [HarmonyPatch(typeof(Verb_LaunchProjectile), "get_Projectile")]
    public static class Projectile
    {
        public static bool Prefix(Verb_LaunchProjectile __instance, ref ThingDef __result)
        {
            CompChangeableProjectile compChangeableProjectile = __instance.EquipmentSource.GetComp<CompChangeableProjectile>();
            if (compChangeableProjectile != null && compChangeableProjectile.Loaded && __instance.caster.def.HasModExtension<Turret_Def>())
            {
                __result = __instance.verbProps.defaultProjectile;
                return false;
            }
            return true;
        }
    }
}
