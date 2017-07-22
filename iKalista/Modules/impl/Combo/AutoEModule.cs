using System.Linq;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Orbwalking;
using Aimtec.SDK.Util.Cache;
using iKalista.Utils;

namespace iKalista.Modules.impl.Combo
{
    internal class AutoEModule : IModule
    {
        public ModuleType GetModuleType()
        {
            return ModuleType.OnUpdate;
        }

        public string GetName()
        {
            return "AutoEModule";
        }

        public bool ShouldExecute()
        {
            return Variables.Spells[SpellSlot.E].Ready &&
                   Variables.Menu["com.ikalista.combo.e"]["useE"].Enabled && Variables.Orbwalker.GetActiveMode() == Orbwalker.Implementation.Combo;
        }

        public void Execute()
        {
            foreach (
                var target in
                GameObjects.EnemyHeroes.Where(
                    x => x.IsValid && x.HasRendBuff() && x.IsValidTarget(Variables.Spells[SpellSlot.E].Range)))
                if (target.GetRendBuffCount() >= Variables.Menu["com.ikalista.combo.e"]["eStacks"].Value)
                {
                    Variables.Spells[SpellSlot.E].Cast();
                }
        }
    }
}