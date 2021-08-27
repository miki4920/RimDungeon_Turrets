using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;

namespace RimDungeon
{
    public class Verb_RimDungeon : Verb_LaunchProjectile
    {
        protected override int ShotsPerBurst => verbProps.burstShotCount;

        public override bool CanHitTargetFrom(IntVec3 root, LocalTargetInfo targ)
        {
            IntVec3 position_caster = this.caster.Position;
            IntVec3 position_target = targ.Cell;
            Rot4 rot = this.caster.Rotation;
            float arc = this.caster.def.GetModExtension<Turret_Def>().firingArc;
            return base.CanHitTargetFrom(root, targ) && PublicFunctions.WithinFiringArcOf(position_caster, position_target, rot, arc);
        }
        protected override bool TryCastShot()
        {
            bool castedShot = base.TryCastShot();
            Projectile_Def projectile = verbProps.defaultProjectile.GetModExtension<Projectile_Def>();

            if (castedShot && projectile.shotCount - 1 > 0)
            {
                for (int i = 0; i < projectile.shotCount - 1; i++)
                {
                    base.TryCastShot();
                }
            }

            return castedShot;
        }

    }
}
