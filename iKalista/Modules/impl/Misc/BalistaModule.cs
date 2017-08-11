using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Util.Cache;
using iKalista.Utils;
using GameObjects = iKalista.Utils.GameObjects;

namespace iKalista.Modules.impl.Misc
{
    class BalistaModule : IUpdateModule
    {
        public void OnLoad()
        {
            
        }

        public string GetName()
        {
            return "Balista Module";
        }

        public bool ShouldExecute()
        {
            return Variables.Spells[SpellSlot.R].Ready && Variables.Menu["com.ikalista.combo"]["balista"].Enabled;
        }

        public void Execute()
        {
            var soulboundhero = GameObjects.AllyHeroes.FirstOrDefault(x => x.HasBuff("kalistacoopstrikeally"));
            if (soulboundhero?.ChampionName == "Blitzcrank")
                foreach (var unit in
                    GameObjects.EnemyHeroes.Where(
                        h =>
                            h.IsVisible && h.Distance(ObjectManager.GetLocalPlayer().ServerPosition) > 700
                            && h.Distance(ObjectManager.GetLocalPlayer().ServerPosition) < 1400))
                    if (unit.HasBuff("rocketgrab2") )
                        Variables.Spells[SpellSlot.R].Cast();
        }

        public ModuleType GetModuleType()
        {
            return ModuleType.OnUpdate;
        }
    }
}
