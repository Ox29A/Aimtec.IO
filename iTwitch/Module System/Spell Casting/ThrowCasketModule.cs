using System;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Menu.Components;
using Aimtec.SDK.Orbwalking;
using Aimtec.SDK.Prediction.Skillshots;
using iTwitch.Module_System.Enumerations;
using Variables = iTwitch.Utils.Variables;

namespace iTwitch.Module_System.Spell_Casting
{
    internal class ThrowCasketModule : IModule
    {
        public string GetName()
        {
            return "Throw Casket Module";
        }

        public string GetDescription()
        {
            return "Handles All the logic for Twitch's W spell";
        }

        public void OnLoad()
        {
            Console.WriteLine("Throw Casket Module Succesfully Loaded!");

            Variables.Orbwalker.PostAttack += OnPostAttack;
        }

        private void OnPostAttack(object sender, PostAttackEventArgs e)
        {
            var unit = sender as Obj_AI_Hero;
            var target = e.Target as Obj_AI_Hero;

            if (target == null || !unit.IsMe || !target.IsValidTarget(Variables.Spells[SpellSlot.W].Range) ||
                target.IsInvulnerable || ObjectManager.GetLocalPlayer().IsUnderEnemyTurret())
                return;

            var autoAttackDamage = ObjectManager.GetLocalPlayer().GetAutoAttackDamage(target);

            if (target.Health < autoAttackDamage * Variables.Menu["com.itwitch.misc"]["noWAA"].Value)
              return;

            if (target.IsValidTarget(Variables.Spells[SpellSlot.W].Range) && !ObjectManager.GetLocalPlayer().HasBuff("TwitchHideInShadows"))
            {
                Variables.Spells[SpellSlot.W].Cast(target);
            }
        }

        public bool CanExecute()
        {
            return
                Variables.Spells[SpellSlot.W]
                    .Ready && Variables.Menu["com.itwitch.combo"]["useQ"].Enabled && Variables.Orbwalker.GetActiveMode() == Orbwalker.Implementation.Combo;
        }

        public void Execute()
        {
            //Ignored
        }

        public ModulePriority GetPriority()
        {
            return ModulePriority.Medium;
        }

        public ModuleType GetModuleType()
        {
            return ModuleType.OnPostAttack;
        }
    }
}
