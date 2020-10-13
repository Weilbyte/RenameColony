using Multiplayer.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace RenameColony
{
    [StaticConstructorOnStartup]
    static class MultiplayerCompatibility
    {
        static MultiplayerCompatibility()
        {
            if (!MP.enabled) return;
            Type colony = typeof(Dialog_RenameColony);
            MP.RegisterSyncMethod(colony, nameof(Dialog_RenameColony.NamedPublic));
            MP.RegisterSyncWorker<Dialog_RenameColony>(SyncRename, typeof(Dialog_RenameColony));
            MP.RegisterAll();
        }

        static void SyncRename(SyncWorker sync, ref Dialog_RenameColony rename)
        {
            if (sync.isWriting)
            {
                sync.Write(rename.changeNameTo);
                sync.Write(rename.changeSecondNameTo);
                sync.Write(rename.changingSettlement);
            }
            else
            {
                string new_faction_name = sync.Read<string>();
                string new_base_name = sync.Read<string>();
                Settlement new_base_obj = sync.Read<Settlement>();
                NamePlayerFactionDialogUtility.Named(new_faction_name);
                NamePlayerSettlementDialogUtility.Named(new_base_obj, new_base_name);
            }
        }
    }
}