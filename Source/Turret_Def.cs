using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace RimDungeon
{
    public class Turret_Def : DefModExtension
    {

        private static readonly Turret_Def DefaultValues = new Turret_Def();

        public static Turret_Def Get(Def def)
        {
            return def.GetModExtension<Turret_Def>() ?? DefaultValues;
        }

        public List<ThingDef> guns = null;
        public float firingArc = 360;

        
    }

    public class Projectile_Def : DefModExtension
    {
        public int shotCount = 1;
    }
}

