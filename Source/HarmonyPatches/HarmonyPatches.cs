using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using RimWorld;
using System.Reflection;
using Verse;

namespace RimDungeon
{
	[StaticConstructorOnStartup]
	class Main
	{
		static Main()
		{
			Harmony harmonyInstance = new Harmony("miki4920.RimDungeonTurrets");
			harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
		}
	}
}
