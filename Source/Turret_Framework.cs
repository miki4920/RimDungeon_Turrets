using RimWorld;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimDungeon
{
    public class Turret_Framework : Building_TurretGun
    {
        public Turret_Def TurretDef => base.def.GetModExtension<Turret_Def>();
        bool changeGun = false;
        bool changedShell = false;
        int currentGun = 0;
        
        public override void PostMake()
        {
            base.PostMake();
            DetermineGun();

        }
        private void UpdateGunVerbs()
        {
            List<Verb> allVerbs = this.gun.TryGetComp<CompEquippable>().AllVerbs;
            for (int i = 0; i < allVerbs.Count; i++)
            {
                Verb verb = allVerbs[i];
                verb.caster = this;
                verb.castCompleteCallback = new Action(this.BurstComplete);
            }
        }
        public void DetermineGun()
        {
            CompChangeableProjectile projectile = null;
            if (this.gun != null)
            {
                projectile = this.gun.TryGetComp<CompChangeableProjectile>();
            }
            if (changeGun && currentGun + 1 < TurretDef.guns.Count)
            {
                currentGun += 1;
                this.gun = ThingMaker.MakeThing(TurretDef.guns[currentGun], null);
                changeGun = false;
            }
            
            else if (changeGun && currentGun + 1 == TurretDef.guns.Count)
            {
                currentGun = 0;
                this.gun = ThingMaker.MakeThing(TurretDef.guns[currentGun], null);
                changeGun = false;
            }
            UpdateGunVerbs();
            if (projectile != null && projectile.Loaded && projectile.LoadedShell != this.AttackVerb.verbProps.defaultProjectile)
            {
                ThingDef shell = projectile.LoadedShell;
                shell.projectileWhenLoaded = this.AttackVerb.verbProps.defaultProjectile;
                this.gun.TryGetComp<CompChangeableProjectile>().LoadShell(shell, this.AttackVerb.verbProps.burstShotCount);
            }
            this.burstCooldownTicksLeft = this.BurstCooldownTime().SecondsToTicks();
        }

        public override void Tick()
        {
            base.Tick();
            CompChangeableProjectile projectile = this.gun.TryGetComp<CompChangeableProjectile>();
            if (projectile != null)
            {
                if (!changedShell && projectile.Loaded && projectile.LoadedShell != this.AttackVerb.verbProps.defaultProjectile)
                {
                    changedShell = true;
                    ThingDef shell = projectile.LoadedShell;
                    shell.projectileWhenLoaded = AttackVerb.verbProps.defaultProjectile;
                    this.gun.TryGetComp<CompChangeableProjectile>().LoadShell(shell, this.AttackVerb.verbProps.burstShotCount);
                }
                else if (!projectile.Loaded)
                {
                    changedShell = false;
                }
            }
        }

        public override IEnumerable<Gizmo> GetGizmos()
        {
            foreach (Gizmo gizmo in base.GetGizmos())
            {
                if (!gizmo.ToString().Contains("Extract"))
                {
                    yield return gizmo;
                }
                else if (gizmo.ToString().Contains("Extract") && TurretDef.guns == null)
                {
                    yield return gizmo;
                }
            }
            IEnumerator<Gizmo> enumerator = null;
            if (TurretDef.guns != null && TurretDef.guns.Count > 1)
            {
                yield return new Command_Action
                {
                    defaultLabel = "ChangeGun".Translate(),
                    defaultDesc = "ChangeGunDesc".Translate(),
                    icon = ContentFinder<Texture2D>.Get("UI/Commands/HoldFire", true),
                    action = delegate ()
                    {
                        changeGun = true;
                        DetermineGun();
                    },
                };
            }
            yield break;

        }
        public override string GetInspectString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            string inspectString = base.GetInspectString();
            if (!inspectString.NullOrEmpty())
            {
                stringBuilder.AppendLine(inspectString);
            }
            if (TurretDef.guns != null)
            {
                stringBuilder.AppendLine("CurrentMode".Translate() + TurretDef.guns[currentGun].label);
            }
            return stringBuilder.ToString().TrimEndNewlines();
        }
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<int>(ref currentGun, "currentGun", 0, false);
        }
    }
}
