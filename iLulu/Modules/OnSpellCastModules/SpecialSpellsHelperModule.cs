namespace iLulu.Modules.OnSpellCastModules
{
    using Aimtec;
    using Aimtec.SDK.Extensions;

    using iLulu.Interfaces;
    using iLulu.Utils;

    internal class SpecialSpellsHelperModule : ISpellCastModule
    {
        public bool CanExecute() => Variables.Spells[SpellSlot.W].Ready || Variables.Spells[SpellSlot.E].Ready;

        public void Execute(Obj_AI_Base sender1, Obj_AI_BaseMissileClientDataEventArgs args)
        {
            var sender = sender1 as Obj_AI_Hero;

            if (sender == null || sender.IsEnemy || sender.CountEnemyHeroesInRange(500) == 0) return;

            foreach (var spell in Variables.SpecialSpells)
            {
                if (sender.ChampionName != spell.Key || !args.SpellData.Name.Equals(spell.Value))
                    continue;

                if (!Variables.Menu["misc"]["spec"][sender.ChampionName][args.SpellData.Name].Enabled)
                    continue;

                if (Variables.Spells[SpellSlot.W].Ready)
                {
                    Variables.Spells[SpellSlot.W].CastOnUnit(sender);
                }

                if (Variables.Spells[SpellSlot.E].Ready)
                {
                    Variables.Spells[SpellSlot.E].CastOnUnit(sender);
                }
            }
        }

        public string GetDescription() => "Casts W / E on special spells";

        public string GetName() => "Active Spell Helper";

        public void OnLoad()
        {
        }
    }
}