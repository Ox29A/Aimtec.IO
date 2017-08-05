using System;

namespace iLulu.Modules.UpdateModules
{
    using System.Linq;

    using Aimtec;
    using Aimtec.SDK.Damage;
    using Aimtec.SDK.Damage.JSON;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Util.Cache;

    using iLulu.Interfaces;
    using iLulu.Utils;

    using ZLib.Base;
    using ZLib.Handlers;

    public class AutoSaver : IEventModule<Unit, PredictDamageEventArgs>
    {

        public void OnLoad()
        {
            Console.WriteLine("Auto Saver Module Loaded");
        }

        public string GetName()
        {
            return "Auto Save Module";
        }

        public string GetDescription()
        {
            return "Saves the nigger from death?";
        }

        public bool CanExecute()
        {
            return Variables.Menu["autoShield"]["useE"].Enabled && Variables.Spells[SpellSlot.E].Ready;
        }

        public void Execute()
        {
            foreach (var selectedAlly in GameObjects.AllyHeroes.Where(x => x.CountEnemyHeroesInRange(1200) > 0))
            {
                if (Variables.Menu["autoShield"]["prior"][selectedAlly.ChampionName + "EPriority"].Value == 0)
                    return;

                if (ObjectManager.GetLocalPlayer().Distance(selectedAlly) <= Variables.Spells[SpellSlot.E].Range
                    && selectedAlly.HealthPercent()
                    <= Variables.Menu["autoShield"]["prior"][selectedAlly.ChampionName + "EPriority"].Value)
                {
                    Variables.Spells[SpellSlot.E].CastOnUnit(selectedAlly);
                }

                if (!Variables.Spells[SpellSlot.E].Ready && ObjectManager.GetLocalPlayer().Distance(selectedAlly) <= Variables.Spells[SpellSlot.R].Range
                    && selectedAlly.HealthPercent()
                    <= Variables.Menu["autoShield"]["prior"][selectedAlly.ChampionName + "EPriority"].Value && Variables.Menu["autoShield"]["useR"].Enabled)
                {
                    Variables.Spells[SpellSlot.R].CastOnUnit(selectedAlly);
                }
            }
        }

        public void Execute(Unit sender, PredictDamageEventArgs args)
        {
            if (sender == null)
                return;

            // Cast to OBJ_AI_HERO
            var objAiHero = sender.Instance as Obj_AI_Hero;

            // Checks if we can cast our shield on ally.
            var shouldCast = objAiHero != null && objAiHero.IsAlly && Variables.Menu["autoShield"]["prior"][objAiHero.ChampionName + "EPriority"].Value != 0;

            // Checks if we should cast, and if the given ally is within range of our spell.
            if (shouldCast && ObjectManager.GetLocalPlayer().Distance(objAiHero) <= Variables.Spells[SpellSlot.E].Range)
            {
                // Checks if any of the incoming spells are either crowd control spells, or Ultimates
                if (sender.Events.Contains(EventType.CrowdControl) || sender.Events.Contains(EventType.Ultimate))
                {

                    // Checks if any enemies are within 1000 units of our given ally.
                    if (objAiHero.CountEnemyHeroesInRange(1000) > 0)
                    {
                        Variables.Spells[SpellSlot.E].CastOnUnit(objAiHero);
                        return;
                    }

                    // Calculates the percent damage incoming to our ally.
                    var incomingDamagePercent = sender.IncomeDamage * 100 / objAiHero.Health;

                    // Checks if our ally is going to die from incoming spell, or if the damage from the incoming spell will deal more then 50% health
                    // Also checks if allys health percent is lower then given percent in menu, if any of the cases met, should cast E on ally.
                    if (sender.IncomeDamage >= objAiHero.Health 
                        || incomingDamagePercent >= 50 
                        || objAiHero.HealthPercent()
                        <= Variables.Menu["autoShield"]["prior"][objAiHero.ChampionName + "EPriority"].Value)
                    {
                        Variables.Spells[SpellSlot.E].CastOnUnit(objAiHero);
                    }
                }
            }
        }
    }
}
