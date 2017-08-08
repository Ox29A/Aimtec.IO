namespace iLulu.Modules.UpdateModules
{
    #region Imports

    using System.Linq;

    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Util.Cache;

    using iLulu.Interfaces;
    using iLulu.Utils;

    #endregion

    public class SpeedyGonzales : IUpdateModule
    {
        public bool CanExecute()
        {
            return true;
        }

        public void Execute()
        {
            if (!Variables.Menu["speedy"]["key"].Enabled)
                return;

            var target = GameObjects.AllyHeroes.Where(x => !x.IsMe)
                .OrderByDescending(h => Variables.Menu["speedy"][h.ChampionName + "WEPriority"].Value).FirstOrDefault();

            if (target != null && ObjectManager.GetLocalPlayer().Distance(target)
                <= Variables.Spells[SpellSlot.W].Range)
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

        public string GetDescription()
        {
            return "SPEEDY LIKE SPEEDY GONZALES";
        }

        public string GetName()
        {
            return "Speedy Gonzales";
        }

        public void OnLoad()
        {
        }
    }
}