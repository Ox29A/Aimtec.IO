// -------------------------------------------------------------------------------------------------------------------
// <copyright file="BuffExtensions.cs" company="">
//   
// </copyright>
// <summary>
//   The Buff Extensions
// </summary>
// -------------------------------------------------------------------------------------------------------------------

using System.Linq;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Damage.JSON;
using Aimtec.SDK.Extensions;

namespace iKalista.Utils
{
    /// <summary>
    ///     The Buff Extensions
    /// </summary>
    internal static class BuffExtensions
    {
        /// <summary>
        ///     Gets the current amount of rend stacks on the given <see cref="Obj_AI_Hero" /> target
        /// </summary>
        /// <param name="target">The target</param>
        /// <returns>amount of stacks</returns>
        public static int GetRendBuffCount(this Obj_AI_Hero target) => target.GetBuffCount("kalistaexpungemarker");

        /// <summary>
        ///     Checks if the given target has the rend buff
        /// </summary>
        /// <param name="target">the target</param>
        /// <returns>true / false if target has buff??</returns>
        public static bool HasRendBuff(this Obj_AI_Hero target) => target != null &&
                                                                   target.HasBuff("kalistaexpungemarker");

        public static bool IsRendKillable(this Obj_AI_Hero target)
        {
            if (target == null || !target.IsValid || !target.HasRendBuff() || HasInvulnerability(target))
                return false;

            var damage = ObjectManager.GetLocalPlayer().GetSpellDamage(target, SpellSlot.E, DamageStage.Buff);
            return damage >= target.Health;
        }

        /// <summary>
        ///     Checks if the given target is un kill able :(
        /// </summary>
        /// <param name="target">THE TARGET AGAIN</param>
        /// <returns>true / false given the buffs the target has kappa</returns>
        public static bool HasInvulnerability(this Obj_AI_Hero target)
        {
            if (target == null)
                return false;

            if (target.HasBuffOfType(BuffType.Invulnerability) || target.HasBuffOfType(BuffType.SpellShield)
                || target.HasBuffOfType(BuffType.SpellImmunity))
                return true;

            if (target.Buffs.Any(x => x.IsValid && x.DisplayName == "JudicatorIntervention"))
                return true;

            if (target.ChampionName == "Tryndamere" && target.Buffs.Any(
                    x => x.Caster.NetworkId == target.NetworkId && x.IsValid && x.DisplayName == "Undying Rage"))
                return true;

            if (target.Buffs.Any(b => b.IsValid && b.DisplayName == "Chrono Shift"))
                return true;

            if (target.HasBuff("kindredrnodeathbuff"))
                return true;

            return false;
        }
    }
}