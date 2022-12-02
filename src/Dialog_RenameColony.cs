using System;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace RenameColony
{
    class Dialog_RenameColony : Dialog_GiveName
    {
        private Settlement settlement;
        public string changeNameTo;
        public string changeSecondNameTo;
        public Settlement changingSettlement;

        public Dialog_RenameColony(Settlement settlement)
        {
            this.settlement = settlement;
            changingSettlement = settlement;
            if (settlement.HasMap && settlement.Map.mapPawns.FreeColonistsSpawnedCount != 0)
            {
                Pawn suggestingPawn = settlement.Map.mapPawns.FreeColonistsSpawned.RandomElement<Pawn>();
            }
            this.curName = Faction.OfPlayer.Name;
            this.changeNameTo = curName;
            this.changeSecondNameTo = settlement.Name;
            this.nameMessageKey = "RCNamePlayerFactionMessage";
            this.invalidNameMessageKey = "PlayerFactionNameIsInvalid";
            this.useSecondName = true;
            this.curSecondName = settlement.Name;
            this.secondNameMessageKey = "RCNamePlayerFactionBaseMessage_NameFactionContinuation";
            this.invalidSecondNameMessageKey = "RCPlayerFactionBaseNameIsInvalid";
            this.gainedNameMessageKey = "RCPlayerFactionAndBaseGainsName";
            this.nameGenerator = () => NameGenerator.GenerateName(Faction.OfPlayer.def.factionNameMaker, IsValidName);
            this.secondNameGenerator = () => NameGenerator.GenerateName(Faction.OfPlayer.def.settlementNameMaker, IsValidSecondName);
        }

        public override void PostOpen()
        {
            base.PostOpen();
            if (this.settlement.Map != null)
            {
                Current.Game.CurrentMap = this.settlement.Map;
            }
        }

        protected override bool IsValidName(string s)
        {
            return NamePlayerFactionDialogUtility.IsValidName(s);
        }

        protected override bool IsValidSecondName(string s)
        {
            return NamePlayerSettlementDialogUtility.IsValidName(s);
        }

        public void NamedPublic(string s)
        {
            
        }

        protected override void Named(string s)
        {
            changeNameTo = s;
            NamePlayerFactionDialogUtility.Named(s);
        }

        protected override void NamedSecond(string s)
        {
            changeSecondNameTo = s;
            changingSettlement = this.settlement;
            NamedPublic(s);
            NamePlayerSettlementDialogUtility.Named(this.settlement, s);
        }
    }
}
