using System;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Orbwalking;
using Aimtec.SDK.Prediction.Skillshots;
using iTwitch.Module_System.Enumerations;
using iTwitch.Utils;

namespace iTwitch.Module_System.Spell_Casting
{
    internal class ThrowCasketModule : IModule
    {
        public string GetName() => "Throw Casket Module";

        public string GetDescription() => "Handles All the logic for Twitch's W spell";

        public void OnLoad()
        {
            Console.WriteLine("Throw Casket Module Succesfully Loaded!");

            Variables.Orbwalker.PostAttack += OnPostAttack;
        }

        public bool CanExecute() => Variables.Spells[SpellSlot.W]
                                        .Ready && Variables.Menu["com.itwitch.combo"]["useW"].Enabled &&
                                    Variables.Orbwalker.GetActiveMode() == Orbwalker.Implementation.Combo;

        public void Execute()
        {
            //Ignored
        }

        public ModulePriority GetPriority() => ModulePriority.Medium;

        public ModuleType GetModuleType() => ModuleType.OnPostAttack;

        private void OnPostAttack(object sender, PostAttackEventArgs e)
        {
            if (!CanExecute())
                return;

            var unit = sender as Obj_AI_Hero;
            var target = e.Target as Obj_AI_Hero;

            if (unit == null || target == null || !unit.IsMe ||
                !target.IsValidTarget(Variables.Spells[SpellSlot.W].Range) ||
                target.IsInvulnerable || ObjectManager.GetLocalPlayer().IsUnderEnemyTurret())
                return;

            var autoAttackDamage = ObjectManager.GetLocalPlayer().GetAutoAttackDamage(target);

            if (target.Health < autoAttackDamage * Variables.Menu["com.itwitch.misc"]["noWAA"].Value)
                return;

            var prediction = Variables.Spells[SpellSlot.W].GetPrediction(target);

            if (target.IsValidTarget(Variables.Spells[SpellSlot.W].Range) &&
                !ObjectManager.GetLocalPlayer().HasBuff("TwitchHideInShadows") &&
                prediction.HitChance >= HitChance.Medium)
                Variables.Spells[SpellSlot.W].Cast(target);
        }
    }
}