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
        public static bool BaseMethodDummy(Verb_Shoot instance)
        {
            return false;
        }

        [HarmonyPatch(typeof(Verb_Shoot), "TryCastShot")]
        public static bool Prefix(Verb_Shoot __instance, ref bool __result)
        {
            if(__instance.Projectile.HasModExtension<Projectile_Def>()) 
            {
                var castedShot = BaseMethodDummy(__instance);
                int projectileCount = __instance.Projectile.GetModExtension<Projectile_Def>().shotCount;
                if (castedShot && projectileCount - 1 > 0)
                {
                    for (int i = 0; i < projectileCount - 1; i++)
                    {
                        BaseMethodDummy(__instance);
                    }
                }
                __result = castedShot;
                return false;
            }
            return true;
        }

    }
}