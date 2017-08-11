using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Menu;
using Aimtec.SDK.Orbwalking;
using iKalista.Utils;

namespace iKalista.Modules.impl.Misc
{
    class UnkillableMinionModuleModule : IEventModule<object, NonKillableMinionEventArgs>
    {
        public void OnLoad()
        {
            
        }

        public string GetName()
        {
            return "Non killable minion";
        }

        public bool ShouldExecute()
        {
            return false;
        }

        public ModuleType GetModuleType()
        {
            return ModuleType.OnUnkillableMinion;
        }

        public void Execute(object minion, NonKillableMinionEventArgs args)
        {
            var killableMinion = minion as Obj_AI_Base;
            if (killableMinion == null || !Variables.Spells[SpellSlot.E].Ready
                || ObjectManager.GetLocalPlayer().HasBuff("summonerexhaust") || !killableMinion.HasRendBuff())
                return;

            if (Variables.Menu["com.ikalista.misc"]["autoEUnkillable"].Enabled
                && killableMinion.IsRendKillable())
                Variables.Spells[SpellSlot.E].Cast();
        }
    }
}
