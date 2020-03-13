using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using System.Reflection;
using UnityEngine;
using Verse;

namespace RenameColony
{
    public class Mod : Verse.Mod
    {
        public Mod(ModContentPack content) : base(content)
        {
            Harmony renameColony = new Harmony("weilbyte.rimworld.renamecolony");
            MethodInfo targetMethod = AccessTools.Method(typeof(PlaySettings), "DoPlaySettingsGlobalControls");
            HarmonyMethod postFix = new HarmonyMethod(typeof(RenameColony).GetMethod("AddWidget"));
            renameColony.Patch(targetMethod, null, postFix);
            Log.Message("RenameColony :: Postfixed");
        }

        public class RenameColony
        {
            public static void AddWidget(WidgetRow row, bool worldView)
            {
                if (worldView)
                {
                    return;
                }
                if (row.ButtonIcon(ContentFinder<Texture2D>.Get("Rename"), "Rename the Colony!"))
                {
                    Settlement settlement = (Settlement)Find.CurrentMap.info.parent;
                    Find.WindowStack.Add(new Dialog_RenameColony(settlement));
                }
            }
        }
    }
}