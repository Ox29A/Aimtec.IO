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

    internal class CastEModule : IUpdateModule
    {
        public void OnLoad()
        {
            Console.WriteLine("Cast E Module Loaded");
        }

        public string GetName()
        {
            return "Cast E Module";
        }

        public string GetDescription()
        {
            return "Casts E at a given target";
        }

        public bool CanExecute()
        {
            return Variables.Orbwalker.Mode == OrbwalkingMode.Combo
                && Variables.Spells[SpellSlot.E].Ready;
        }

        public void Execute()
        {
            var target = TargetSelector.GetTarget(Variables.Spells[SpellSlot.E].Range);

            Console.WriteLine("Allys in range: " + ObjectManager.GetLocalPlayer().CountAllyHeroesInRange(1200));

            if (target != null && target.IsValidTarget(Variables.Spells[SpellSlot.E].Range) && ObjectManager.GetLocalPlayer().CountAllyHeroesInRange(1200) < 1)
            {
                Variables.Spells[SpellSlot.E].CastOnUnit(target);
            }
        }
    }
}
