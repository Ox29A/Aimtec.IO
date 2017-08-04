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

    public class AutoSaver : IUpdateModule
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
            return Variables.Menu["autoShield"]["useE"].Enabled;
        }

        public void Execute()
        {
            Console.WriteLine("Executing module cast e shielding");
            foreach (var selectedAlly in GameObjects.AllyHeroes.Where(x => x.CountEnemyHeroesInRange(1200) > 0))
            {
                Console.WriteLine(selectedAlly.ChampionName);
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

                /*foreach (var skillshot in EvadeManager.DetectedSkillshots)
                {
                    var spellDamage =
                        (skillshot.Unit as Obj_AI_Hero).GetSpellDamage(selectedAlly, skillshot.SpellData.Slot);

                    if (skillshot.SpellData.IsDangerous && skillshot.SpellData.DangerValue >= 3 || (selectedAlly.Health <= spellDamage + 15))
                    {
                        if (skillshot.IsAboutToHit(100, selectedAlly))
                        {

                        }
                    }
                }*/
            }
        }
    }
}
