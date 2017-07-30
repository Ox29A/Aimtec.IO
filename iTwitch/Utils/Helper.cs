namespace iTwitch.Utils
{
    using System;
    using System.Linq;

    using Aimtec;
    using Aimtec.SDK.Extensions;

    internal static class Helper
    {
        #region Public Methods and Operators

        public static bool UnderTurret(Obj_AI_Base unit, bool enemyTurretsOnly)
        {
            return UnderTurret(unit.Position, enemyTurretsOnly);
        }

        public static bool UnderTurret(Vector3 position, bool enemyTurretsOnly)
        {
            return
                ObjectManager.Get<Obj_AI_Turret>().Any(turret => turret.IsValidTarget(950, enemyTurretsOnly, checkRangeFrom: position));
        }

        public static float GetPassiveDamage(this Obj_AI_Base target, int stacks = -1)
        {
            if (!target.HasBuff("twitchdeadlyvenom"))
                return 0;

            var damagePerStack = 0;
            var level = ObjectManager.GetLocalPlayer().Level;

            if (level < 5)
                damagePerStack = 2;
            else if (level < 9)
                damagePerStack = 3;
            else if (level < 13)
                damagePerStack = 4;
            else if (level < 17)
                damagePerStack = 5;
            else if (level >= 17)
                damagePerStack = 6;

            var remainingBuffTime = Math.Max(0, target.GetRemainingBuffTime("twitchdeadlyvenom"));
            var damageOverTime = damagePerStack * target.GetPoisonStacks() * remainingBuffTime -
                                 target.HPRegenRate * remainingBuffTime;

            return damageOverTime;
        }

        public static double GetPoisonDamage(this Obj_AI_Base target, bool includePassive = false)
        {
            if (target == null || !target.HasBuff("twitchdeadlyvenom") || target.IsInvulnerable ||
                target.HasBuff("KindredRNoDeathBuff")
                || target.HasBuffOfType(BuffType.SpellShield))
                return 0;

            /*
            var currentStacks = target.GetPoisonStacks();
            var currentLevel = 0; //Variables.Spells[SpellSlot.E].Level - 1;
            var baseDamage = new float[] {20, 35, 50, 65, 80};
            var bonusDamage = new float[] {15, 20, 25, 30, 35};

            if (currentStacks == 0)
                return 0;

            var calculatedDamage = baseDamage[currentLevel]
                                   + bonusDamage[currentLevel] * currentStacks
                                   + (0.25 * ObjectManager.GetLocalPlayer().BonusAttackDamage
                                      + 0.2 * ObjectManager.Player.BonusAbilityPower) * currentStacks;

            return ObjectManager.Player.CalcDamage(target, Damage.DamageType.Physical, calculatedDamage) + (includePassive ? GetPassiveDamage(target) : 0);
            */
            var countBuffs = target.BuffManager.GetBuffCount("TwitchDeadlyVenom");
            return countBuffs *
                   new double[] {15, 20, 25, 30, 35}[
                       ObjectManager.GetLocalPlayer().SpellBook.GetSpell(SpellSlot.E).Level - 1]
                   + 0.2 * ObjectManager.GetLocalPlayer().TotalAbilityDamage
                   + 0.25 * ObjectManager.GetLocalPlayer().FlatPhysicalDamageMod +
                   new double[] {20, 35, 50, 65, 80}[
                       ObjectManager.GetLocalPlayer().SpellBook.GetSpell(SpellSlot.E).Level - 1];

            // return ObjectManager.GetLocalPlayer().GetSpellDamage(target, SpellSlot.E, DamageStage.Buff);
        }

        public static int GetPoisonStacks(this Obj_AI_Base target) => target.GetBuffCount("TwitchDeadlyVenom");

        public static float GetRealHealth(this Obj_AI_Base target) => target.Health +
                                                                      (target.PhysicalShield > 0
                                                                          ? target.PhysicalShield
                                                                          : 0);

        public static float GetRemainingBuffTime(this Obj_AI_Base target, string buffName)
        {
            return
                target.Buffs.OrderByDescending(buff => buff.EndTime - Game.TickCount)
                    .Where(buff => string.Equals(buff.Name, buffName, StringComparison.CurrentCultureIgnoreCase))
                    .Select(buff => buff.EndTime)
                    .FirstOrDefault() - Game.TickCount;
        }

        public static bool IsPoisonKillable(this Obj_AI_Base target) =>
            GetPoisonDamage(target) >= GetRealHealth(target);

        #endregion
    }
}