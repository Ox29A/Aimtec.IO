namespace iLulu.Modules.Combo
{
    using System;
    using System.Linq;

    using Aimtec;
    using Aimtec.SDK.Damage;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Orbwalking;
    using Aimtec.SDK.TargetSelector;
    using Aimtec.SDK.Util.Cache;

    using iLulu.Interfaces;
    using iLulu.Managers;
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
            var target = TargetSelector.GetTarget(Variables.Spells[SpellSlot.E].Range + Variables.Spells[SpellSlot.Q].Range);

            if (target != null && ObjectManager.GetLocalPlayer().CountAllyHeroesInRange(1200) < 1)
            {
                if (target.IsValidTarget(Variables.Spells[SpellSlot.E].Range))
                {
                    Variables.Spells[SpellSlot.E].CastOnUnit(target);
                    return;
                }

                if (Variables.Menu["combo"]["e"]["eq"].Enabled && ObjectManager.GetLocalPlayer().Distance(target) >= Variables.Spells[SpellSlot.Q].Range)
                {
                    var minion = ObjectManager.Get<Obj_AI_Base>()
                        .Where(
                            o =>
                                o.IsValidTarget(Variables.Spells[SpellSlot.E].Range, true, true, PixManager.Pix.ServerPosition) &&
                                o.Distance(target) < 600 && (o.IsAlly || !(ObjectManager.GetLocalPlayer().GetSpellDamage(o, SpellSlot.Q) >= o.Health)))
                        .OrderBy(o => o.Distance(target))
                        .ThenBy(o => o.Team != ObjectManager.GetLocalPlayer().Team)
                        .FirstOrDefault();

                    if (minion != null)
                    {
                        Variables.Spells[SpellSlot.E].CastOnUnit(minion);
                    }
                }
            }
        }
    }
}
