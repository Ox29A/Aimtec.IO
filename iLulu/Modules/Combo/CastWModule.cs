namespace iLulu.Modules.Combo
{
    using System;
    using System.Linq;

    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Orbwalking;
    using Aimtec.SDK.TargetSelector;
    using Aimtec.SDK.Util.Cache;

    using iLulu.Interfaces;
    using iLulu.Utils;

    internal class CastWModule : IUpdateModule
    {
        public void OnLoad()
        {
            Console.WriteLine("Cast W Module Loaded");
        }

        public string GetName()
        {
            return "Cast W Module";
        }

        public string GetDescription()
        {
            return "Casts W at a given target";
        }

        public bool CanExecute()
        {
            return Variables.Orbwalker.Mode == OrbwalkingMode.Combo && Variables.Menu["combo"]["w"]["useW"].Enabled
                && Variables.Spells[SpellSlot.W].Ready;
        }

        public void Execute()
        {
            // Variables.Menu["combo"]["w"]["poly"][hero.ChampionName + "WPriority"].Value
            var target = ObjectManager.Get<Obj_AI_Hero>().Where(h => h.IsValidTarget(Variables.Spells[SpellSlot.W].Range) && h.IsEnemy && h.IsHero)
                .MaxOrDefault(o => Variables.Menu["combo"]["w"]["poly"][o.ChampionName + "WPriority"].Value);
            if (target != null)
            {
                if (Variables.Menu["combo"]["w"]["poly"][target.ChampionName + "WPriority"].Value == 0)
                    return;

                Variables.Spells[SpellSlot.W].CastOnUnit(target);
            }
        }
    }
}
