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
    [HarmonyPatch(typeof(Verb_Shoot), "TryCastShot")]
    public static class TryCastShot
    {
        public static bool Postfix(Verb_Shoot __instance, ref bool __result)
        {
            var baseClass = __instance.GetType();
            MethodInfo tryCastShotMethod = baseClass.GetMethod("tryCastShot", BindingFlags.NonPublic | BindingFlags.Instance);
            Log.Message(tryCastShotMethod.ToString());
            Log.Message("Hello There!");
            
            return true;
        }

    }
}