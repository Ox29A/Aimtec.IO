using Aimtec;
using Aimtec.SDK.Events;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Orbwalking;
using Aimtec.SDK.Prediction.Skillshots;
using Aimtec.SDK.TargetSelector;
using iKalista.Utils;

namespace iKalista.Modules.impl.Combo
{
    internal class AutoQModule : IModule
    {
        public string GetName()
        {
            return "AutoQModule";
        }

        public bool ShouldExecute()
        {
            return Variables.Spells[SpellSlot.Q].Ready &&
                   Variables.Menu["com.ikalista.combo.q"]["useQ"].Enabled && Variables.Orbwalker.Mode == OrbwalkingMode.Combo;
        }

        public void Execute()
        {
            var target = TargetSelector.GetTarget(Variables.Spells[SpellSlot.Q].Range);

            if (target == null || !target.IsValidTarget(Variables.Spells[SpellSlot.Q].Range) || !target.IsValidTarget(Variables.Spells[SpellSlot.Q].Range))
                return;

            var prediction = Variables.Spells[SpellSlot.Q].GetPrediction(target);

            if (prediction.HitChance >= HitChance.Medium && !Variables.Orbwalker.IsWindingUp && !ObjectManager.GetLocalPlayer().IsDashing())
                Variables.Spells[SpellSlot.Q].Cast(prediction.CastPosition);
        }

        public ModuleType GetModuleType()
        {
            return ModuleType.OnUpdate;
        }
    }
}