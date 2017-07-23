using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Orbwalking;
using iKalista.Utils;

namespace iKalista.Modules.impl.Combo
{
    class ForceTargetModule : IPreAttackModule
    {
        public void OnLoad()
        {

        }

        public string GetName()
        {
            return "Force Target Module";
        }

        public bool ShouldExecute()
        {
            return Variables.Menu["com.ikalista.misc"]["forceW"].Enabled && Variables.Orbwalker.Mode == OrbwalkingMode.Combo;
        }

        public ModuleType GetModuleType()
        {
            return ModuleType.PreAttack;
        }

        public void OnPreAttack(Obj_AI_Base sender, PreAttackEventArgs args)
        {
            if (sender.IsMe)
                return;

            var target =
                GameObjects.EnemyHeroes.FirstOrDefault(
                    x => x.IsValidAutoRange() && x.HasBuff("kalistacoopstrikemarkally"));
            if (target != null)
                Variables.Orbwalker.ForceTarget(target);
        }

        public void Execute()
        {
        }
    }
}
