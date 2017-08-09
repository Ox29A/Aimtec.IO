using System;
using System.Linq;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Damage.JSON;
using Aimtec.SDK.Extensions;
using iKalista.Utils;

namespace iKalista.Modules.impl.Misc
{
    internal class KillstealModule : IOnUpdateModule
    {
        public void OnLoad()
        {
            
        }

        public string GetName()
        {
            return "KillstealModule";
        }

        public bool ShouldExecute()
        {
            return Variables.Spells[SpellSlot.E].Ready && Variables.Menu["com.ikalista.combo"]["useE"].Enabled; // menu item
        }

        public void Execute()
        {
            foreach (var hero in GameObjects.EnemyHeroes.Where(x => x != null && x.IsValidTarget()))
            {
                var damage = ObjectManager.GetLocalPlayer().GetSpellDamage(hero, SpellSlot.E)
                             + ObjectManager.GetLocalPlayer().GetSpellDamage(hero, SpellSlot.E, DamageStage.Buff);

                if (damage >= hero.Health)
                {
                    Variables.Spells[SpellSlot.E].Cast();
                    Console.WriteLine("REKT FAM");
                }
                else
                {
                    Console.WriteLine("Not killable wtf?");
                }
            }
        }

        public ModuleType GetModuleType()
        {
            return ModuleType.OnUpdate;
        }
    }
}