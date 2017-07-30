namespace iLulu.Modules.UpdateModules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Util.Cache;

    using iLulu.Interfaces;
    using iLulu.Utils;

    public class AutoCaster : IUpdateModule
    {
        public void OnLoad()
        {
            Console.WriteLine("Loaded module");
        }

        public string GetName()
        {
            return "Auto Caster Module";
        }

        public string GetDescription()
        {
            return "Auto casts spells";
        }

        public bool CanExecute()
        {
            return Variables.Spells[SpellSlot.Q].Ready && Variables.Menu["combo"]["q"]["useQImpaired"].Enabled;
        }

        public void Execute()
        {
            var target = GameObjects.EnemyHeroes.FirstOrDefault(x => x.IsValidTarget(Variables.Spells[SpellSlot.Q].Range) && x.IsMovementImpaired());

            if (target != null)
            {
                Variables.Spells[SpellSlot.Q].Cast(target);
            }
        }
    }
}
