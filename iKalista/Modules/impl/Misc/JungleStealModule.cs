using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aimtec;
using Aimtec.SDK.Util.Cache;
using iKalista.Utils;
using GameObjects = iKalista.Utils.GameObjects;

namespace iKalista.Modules.impl.Misc
{
    class JungleStealModule : IUpdateModule
    {
        public void OnLoad()
        {
            
        }

        public string GetName()
        {
            return "Jungle Steal Module";
        }

        public bool ShouldExecute()
        {
            return Variables.Spells[SpellSlot.E].Ready && Variables.Menu["com.ikalista.jungle"]["enabled"].Enabled;
        }

        public void Execute()
        {
            var small =
                GameObjects.JungleSmall.Any(x => x.IsRendKillable() && x.IsValid);
            var large =
                GameObjects.JungleLarge.Any(x=>x.IsRendKillable() && x.IsValid);
            var legendary =
                GameObjects.JungleLegendary.Any(x=> x.IsRendKillable() && x.IsValid);

            if (small && Variables.Menu["com.ikalista.jungle"]["small"].Enabled
                || large && Variables.Menu["com.ikalista.jungle"]["large"].Enabled
                || legendary && Variables.Menu["com.ikalista.jungle"]["legendary"].Enabled)
                Variables.Spells[SpellSlot.E].Cast();
        }

        public ModuleType GetModuleType()
        {
            return ModuleType.OnUpdate;
        }
    }
}
