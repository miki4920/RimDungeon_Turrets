using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;
using HarmonyLib;
using UnityEngine;

namespace RimDungeon
{
    [HarmonyPatch(typeof(PlaceWorker_ShowTurretRadius), nameof(PlaceWorker_ShowTurretRadius.AllowsPlacing))]
    public static class AllowsPlacing
    {
        public static bool Prefix(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, ref AcceptanceReport __result)
        {
            VerbProperties verbProperties = ((ThingDef)checkingDef).building.turretGunDef.Verbs.Find((VerbProperties v) => v.verbClass == typeof(Verb_Shoot));
            Turret_Def TurretDef = Turret_Def.Get(checkingDef);
            if (TurretDef.firingArc == 360)
            {
                return true;
            }
            if (verbProperties.range > 0f && verbProperties.range < 56.4)
            {
                PublicFunctions.TryDrawFiringCone(loc, rot, verbProperties.range, TurretDef.firingArc);
            }
            if (verbProperties.minRange > 0f && verbProperties.minRange < 56.4)
            {
                PublicFunctions.TryDrawFiringCone(loc, rot, verbProperties.minRange, TurretDef.firingArc);
            }
            __result = true;
            return false;
        }
    }
}
