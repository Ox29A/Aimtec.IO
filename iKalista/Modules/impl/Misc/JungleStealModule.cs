using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aimtec;
using Aimtec.SDK.Util.Cache;
using iKalista.Utils;

namespace iKalista.Modules.impl.Misc
{
    class JungleStealModule : IModule
    {
        public string GetName()
        {
            return "Jungle Steal Module";
        }

        public bool ShouldExecute()
        {
            return Variables.Spells[SpellSlot.E].Ready && Variables.Menu["com.ikalista.misc"]["junglesteal"].Enabled;
        }

        public void Execute()
        {
            
        }

        public ModuleType GetModuleType()
        {
            return ModuleType.OnUpdate;
        }
    }
}
