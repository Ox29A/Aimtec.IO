using System.Linq;
using Aimtec;
using Aimtec.SDK.Extensions;
using iKalista.Utils;

namespace iKalista.Modules.impl.Misc
{
    internal class KillstealModule : IModule
    {
        public string GetName()
        {
            return "KillstealModule";
        }

        public bool ShouldExecute()
        {
            return Variables.Spells[SpellSlot.E].Ready && Variables.Menu["com.ikalista.combo.e"]["useE"].Enabled; // menu item
        }

        public void Execute()
        {
            if (ObjectManager.Get<Obj_AI_Hero>().Any(x => x.IsValidTarget(1100) && x.IsRendKillable()))
                Variables.Spells[SpellSlot.E].Cast();
        }

        public ModuleType GetModuleType()
        {
            return ModuleType.OnUpdate;
        }
    }
}