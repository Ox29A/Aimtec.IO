namespace iLulu.Modules.UpdateModules
{
    using System.Linq;

    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Menu.Components;
    using Aimtec.SDK.Util.Cache;

    using iLulu.Interfaces;
    using iLulu.Utils;

    class SpeedyGonzales : IUpdateModule
    {
        public void OnLoad()
        {
            
        }

        public string GetName()
        {
            return "Speedy Gonzales";
        }

        public string GetDescription()
        {
            return "SPEEDY LIKE SPEEDY GONZALES";
        }

        public bool CanExecute()
        {
            return Variables.Menu["speedy"]["key"].Enabled;
        }

        public void Execute()
        {
            var target = GameObjects.AllyHeroes.Where(x => !x.IsMe)
                .OrderByDescending(h => Variables.Menu["speedy"][h.ChampionName + "WEPriority"].Value)
                .FirstOrDefault();

            if (target != null && ObjectManager.GetLocalPlayer().Distance(target) <= Variables.Spells[SpellSlot.W].Range)
            {
                if (Variables.Spells[SpellSlot.W].Ready)
                {
                    Variables.Spells[SpellSlot.W].CastOnUnit(target);
                }
                if (Variables.Spells[SpellSlot.E].Ready)
                {
                    Variables.Spells[SpellSlot.E].CastOnUnit(target);
                }
            }
        }
    }
}
