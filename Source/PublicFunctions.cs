using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;

namespace RimDungeon
{
    class PublicFunctions
    {
        public static bool WithinFiringArcOf(IntVec3 position_caster, IntVec3 pos_target, Rot4 rot, float firingArc)
        {
            return GenGeo.AngleDifferenceBetween(rot.AsAngle, (pos_target - position_caster).AngleFlat) <= (firingArc / 2);
        }

        public static void TryDrawFiringCone(IntVec3 centre, Rot4 rot, float distance, float arc)
        {
            if (arc < 360)
            {
                List<IntVec3> ringDrawCells = new List<IntVec3>();
                ringDrawCells.Clear();
                int num = GenRadial.NumCellsInRadius(distance);
                for (int i = 0; i < num; i++)
                {
                    var curCell = centre + GenRadial.RadialPattern[i];
                    if (PublicFunctions.WithinFiringArcOf(centre, curCell, rot, arc))
                        ringDrawCells.Add(curCell);
                }
                GenDraw.DrawFieldEdges(ringDrawCells);
            }
        }
    }
}
