namespace iLulu.Modules.Combo
{
    using System;

    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Orbwalking;
    using Aimtec.SDK.Prediction.Skillshots;
    using Aimtec.SDK.TargetSelector;

    using iLulu.Interfaces;
    using iLulu.Managers;
    using iLulu.Utils;

    internal class CastQModule : IUpdateModule
    {
        public void OnLoad()
        {
            Console.WriteLine("Cast Q Module Loaded");
        }

        public string GetName()
        {
            return "Cast Q Module";
        }

        public string GetDescription()
        {
            return "Casts Q at a given target";
        }

        public bool CanExecute()
        {
            return Variables.Orbwalker.Mode == OrbwalkingMode.Combo
                && Variables.Menu["combo"]["q"]["useQ"].Enabled 
                && Variables.Spells[SpellSlot.Q].Ready;
        }

        public void Execute()
        {
            var target = TargetSelector.GetTarget(Variables.Spells[SpellSlot.Q].Range + Variables.Spells[SpellSlot.E].Range);
            if (target != null && target.IsValid)
            {
                var bestPosition = GetBestPosition(target);
                if (bestPosition != default(Vector3))
                {
                    var prediction = Variables.Spells[SpellSlot.Q].GetPrediction(target, bestPosition, bestPosition);
                    if (prediction.HitChance >= HitChance.High)
                    {
                        Variables.Spells[SpellSlot.Q].Cast(prediction.CastPosition);
                    }
                }
            }
        }

        private Vector3 GetBestPosition(Obj_AI_Hero target)
        {
            var playerPosition = ObjectManager.GetLocalPlayer().ServerPosition;
            var pixPosition = PixManager.Pix.ServerPosition;

            var pixPrediction = Variables.Spells[SpellSlot.Q]
                .GetPrediction(target, pixPosition, pixPosition);
            var standardPrediction = Variables.Spells[SpellSlot.Q]
                .GetPrediction(target, pixPosition, pixPosition);

            if (pixPrediction.HitChance >= HitChance.High && standardPrediction.HitChance <= HitChance.Medium)
                return pixPosition;

            if (standardPrediction.HitChance >= HitChance.High && pixPrediction.HitChance <= HitChance.Medium)
                return playerPosition;

            return standardPrediction.HitChance == pixPrediction.HitChance ? pixPosition : default(Vector3);

            // Coreys shit below again
            /*if (playerPosition.Distance(target) <= range && pixPosition.Distance(target) >= range)
                return playerPosition;
            if (pixPosition.Distance(target) <= range && playerPosition.Distance(target) >= range)
                return pixPosition;*/
        }
    }
}
