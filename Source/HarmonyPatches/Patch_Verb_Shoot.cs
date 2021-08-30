using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using RimWorld;
using Verse;
using HarmonyLib;
using System.Runtime.CompilerServices;

namespace RimDungeon
{
    [HarmonyPatch]
    public static class TryCastShot
    {
        [HarmonyReversePatch]
        [HarmonyPatch(typeof(Verb_LaunchProjectile), "TryCastShot")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        static bool BaseMethodDummy(Verb_Shoot instance)
        {
            return false;
        }

        [HarmonyPatch(typeof(Verb_Shoot), "TryCastShot")]
        static bool Prefix(Verb_Shoot __instance, ref bool __result)
        {
            var castedShot = BaseMethodDummy(__instance);
            if(__instance.verbProps.defaultProjectile.HasModExtension<Projectile_Def>()) 
            {
                int projectileCount = __instance.verbProps.defaultProjectile.GetModExtension<Projectile_Def>().shotCount;
                if (castedShot && projectileCount - 1 > 0)
                {
                    for (int i = 0; i < projectileCount - 1; i++)
                    {
                        BaseMethodDummy(__instance);
                    }
                }
            }
            __result = castedShot;
            return false;
        }

    }
}