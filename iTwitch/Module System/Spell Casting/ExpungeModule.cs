using System;
using System.Linq;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Orbwalking;
using Aimtec.SDK.Prediction.Health;
using Aimtec.SDK.Util.Cache;
using iTwitch.Module_System.Enumerations;
using iTwitch.Utils;

namespace iTwitch.Module_System.Spell_Casting
{
    internal class ExpungeModule : IModule
    {
        private HealthPrediction _healthPrediction;

        public string GetName() => "Expunge Module";

        public string GetDescription() => "Handles All the logic for Twitch's E spell";

        public void OnLoad()
        {
            Console.WriteLine("Expunge Module Succesfully Loaded!");
            _healthPrediction = new HealthPrediction();
        }

        public bool CanExecute() => Variables.Spells[SpellSlot.E]
                                        .Ready && Variables.Menu["com.itwitch.combo"]["useE"].Enabled;

        public void Execute()
        {
            #region OnUpdate

            if (ObjectManager.Get<Obj_AI_Hero>().Any(x => x.IsValidTarget(1100) && x.GetPoisonDamage() > x.Health))
                Variables.Spells[SpellSlot.E].Cast();

            if (Variables.Menu["com.itwitch.misc"]["ebeforedeath"].Enabled &&
                ObjectManager.GetLocalPlayer().HealthPercent() < 5 &&
                GameObjects.EnemyHeroes.Any(x => x.HasBuffOfType(BuffType.Poison)) &&
                _healthPrediction.GetPrediction(ObjectManager.GetLocalPlayer(), (int) (Game.ClockTime + 1000.0)) <=
                50.0f)
                Variables.Spells[SpellSlot.E].Cast();

            #endregion

            #region Orbwalking Combo Mode

            if (Variables.Orbwalker.GetActiveMode() != Orbwalker.Implementation.Combo)
                return;

            if (Variables.Menu["com.itwitch.combo"]["eSlider"].Value > 0)
            {
                var target = GameObjects.EnemyHeroes.FirstOrDefault(x => x.HasBuffOfType(BuffType.Poison));

                if (target != null && target.IsValidTarget(Variables.Spells[SpellSlot.E].Range)
                    && Variables.Menu["com.itwitch.combo"]["eSlider"].Value >=
                    target.GetBuffCount("TwitchExpungeMarker"))
                    Variables.Spells[SpellSlot.E].Cast();
            }
            //DONE auto ks logic - DONE
            //DONE auto expunge at x stacks - DONE

            #endregion
        }

        public ModulePriority GetPriority() => ModulePriority.VeryHigh;

        public ModuleType GetModuleType() => ModuleType.OnUpdate;
    }
}