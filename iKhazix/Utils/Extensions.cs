using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Damage.JSON;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Menu;
using Aimtec.SDK.Prediction.Skillshots;
using Aimtec.SDK.Util.Cache;

namespace iKhazix.Utils
{
    public static class Extensions
    {
        public static bool IsIsolated(this Obj_AI_Base target)
        {
            return !ObjectManager.Get<Obj_AI_Base>()
                .Any(x => x.NetworkId != target.NetworkId && x.Team == target.Team && x.Distance(target) <= 500 &&
                          (x.Type == GameObjectType.obj_AI_Hero || x.Type == GameObjectType.obj_AI_Minion ||
                           x.Type == GameObjectType.obj_AI_Turret));
        }

        public static HitChance GetHitchance(string item)
        {
            var hitchance = Variables.Menu["combo"][item].Value;

            switch (hitchance)
            {
                case 0: // LOW
                    return HitChance.Low;
                case 1: // medium
                    return HitChance.Medium;
                case 2: //high
                    return HitChance.High;
                case 3: // very high
                    return HitChance.VeryHigh;
            }
            return HitChance.Medium;
        }

        public static float Distance(
            this Vector2 point,
            Vector2 segmentStart,
            Vector2 segmentEnd,
            bool onlyIfOnSegment = false,
            bool squared = false)
        {
            var objects = point.ProjectOn(segmentStart, segmentEnd);

            if (objects.IsOnSegment || onlyIfOnSegment == false)
            {
                return squared
                    ? Vector2.DistanceSquared(objects.SegmentPoint, point)
                    : Vector2.Distance(objects.SegmentPoint, point);
            }
            return float.MaxValue;
        }

        public static float GetCorrectQDamage(Obj_AI_Base target)
        {
            var source = ObjectManager.GetLocalPlayer();
            var level = ObjectManager.GetLocalPlayer().SpellBook.GetSpell(SpellSlot.Q).Level;

            if (target.IsIsolated())
            {
                if (Variables.HasEvolvedQ)
                {
                    return (float) source
                        .CalculateDamage(target, DamageType.Physical,
                            new[] {91, 123.5, 156, 188.5, 221}[level] +
                            2.6 * source.FlatPhysicalDamageMod +
                            10 * source.Level);
                }
                return (float) source.CalculateDamage(target, DamageType.Physical,
                    new[] {91, 123.5, 156, 188.5, 221}[level] + 1.56 * source.FlatPhysicalDamageMod);
            }

            if (Variables.HasEvolvedQ)
            {
                return (float) source.CalculateDamage(target, DamageType.Physical,
                    new double[] {70, 95, 120, 145, 170}[level]
                    + 2.24 * source.FlatPhysicalDamageMod
                    + 10 * source.Level);
            }

            return (float) source.CalculateDamage(target, DamageType.Physical,
                new double[] {70, 95, 120, 145, 170}[level]
                + 1.4 * source.FlatPhysicalDamageMod);
        }

        public static float GetBurstDamage(this Obj_AI_Hero source, Obj_AI_Hero target)
        {
            double damage = 0;

            if (Variables.Spells[SpellSlot.Q].Ready)
            {
                damage = GetCorrectQDamage(target);
            }

            if (Variables.Spells[SpellSlot.E].Ready)
            {
                damage += source.GetSpellDamage(target, SpellSlot.E);
            }

            if (Variables.Spells[SpellSlot.W].Ready)
            {
                damage += source.GetSpellDamage(target, SpellSlot.W);
            }

            // TODO items
            return (float) damage;
        }

        public static bool ShouldJump(Vector3 position, Obj_AI_Hero target = null)
        {
            if (!Variables.Menu["safety"]["enabled"].Enabled)
                return true;

            if (Variables.Menu["safety"]["turretCheck"].Enabled && position.PointUnderEnemyTurret())
                return false;
            else if (Variables.Menu["safety"]["enabled"].Enabled)
            {
                if (ObjectManager.GetLocalPlayer().HealthPercent() < Variables.Menu["safety"]["minHealth"].Value &&
                    ObjectManager.GetLocalPlayer().GetBurstDamage(target) < target.HealthPercent())
                    return false;

                if (Variables.Menu["safety"]["countCheck"].Enabled)
                {
                    var enemies = position.GetEnemiesInRange(400);
                    var allies = position.GetAlliesInRange(400);

                    var ec = enemies.Count;
                    var ac = allies.Count;

                    //if no enemies within 400 radius of jumping position then dont jump
                    if (ec == 0)
                    {
                        return false;
                    }

                    float ratio = ac / ec;
                    var setratio = Variables.Menu["safety"]["ratio"].Value / 5f;

                    //Ratio of allies:enemies
                    //if < allies then enemies
                    if (ratio < setratio)
                    {
                        return false;
                    }
                }

                return true;
            }

            return false;
        }

        public static List<Obj_AI_Hero> GetEnemiesInRange(this Vector3 point, float range)
        {
            return GameObjects.EnemyHeroes.Where(x => point.DistanceSquared(x.ServerPosition) <= range * range)
                .ToList();
        }

        public static List<Obj_AI_Hero> GetAlliesInRange(this Vector3 point, float range)
        {
            return GameObjects.AllyHeroes.Where(x => point.DistanceSquared(x.ServerPosition) <= range * range).ToList();
        }

        public static JumpResult GetJumpPosition(Obj_AI_Hero target, bool killsteal = false)
        {
            var mode = Variables.Menu["combo"]["jumpMode"].Value; // 0 = current pos, 1 = pred

            if (mode == 0) // currentPosition
            {
                return new JumpResult
                {
                    Position = target.ServerPosition,
                    HitChance = HitChance.Medium,
                    ShouldJump = ShouldJump(target.ServerPosition, target)
                };
            }

            if (mode == 1)
            {
                var prediction = Variables.Spells[SpellSlot.E].GetPrediction(target);
                return new JumpResult
                {
                    Position = prediction.CastPosition,
                    HitChance = prediction.HitChance,
                    ShouldJump = prediction.HitChance >= HitChance.Medium && ShouldJump(prediction.CastPosition)
                };
            }

            return new JumpResult
            {
                Position = target.ServerPosition,
                HitChance = HitChance.Immobile,
                ShouldJump = false
            };
        }

        public static bool IsHealthy(this Obj_AI_Hero source)
        {
            return source.HealthPercent() >= Variables.Menu["safety"]["minHealth"].Value;
        }

        public static Vector3 GetDoubleJumpPoint(Obj_AI_Hero target, bool first = true)
        {
            if (ObjectManager.GetLocalPlayer().ServerPosition.PointUnderEnemyTurret())
                return ObjectManager.GetLocalPlayer()
                    .ServerPosition.Extend(Variables.NexusPosition, Variables.Spells[SpellSlot.E].Range);

            if (Variables.Menu["doubleJump"]["jumpMode"].Value == 0)
                return ObjectManager.GetLocalPlayer()
                    .ServerPosition.Extend(Variables.NexusPosition, Variables.Spells[SpellSlot.E].Range);

            if (first && Variables.Menu["doubleJump"]["jCursor"].Enabled)
            {
                return Game.CursorPos;
            }

            if (!first && Variables.Menu["doubleJump"]["jCursor2"].Enabled)
            {
                return Game.CursorPos;
            }
            var position = new Vector3();
            var jumptarget = ObjectManager.GetLocalPlayer().IsHealthy()
                ? GameObjects.EnemyHeroes
                    .FirstOrDefault(x => x.IsValidTarget() && x != target &&
                                         Vector3.Distance(ObjectManager.GetLocalPlayer().ServerPosition,
                                             x.ServerPosition) < Variables.Spells[SpellSlot.E].Range)
                : GameObjects.AllyHeroes
                    .FirstOrDefault(x => x.IsAlly && !x.IsDead && !x.IsMe &&
                                         Vector3.Distance(ObjectManager.GetLocalPlayer().ServerPosition,
                                             x.ServerPosition) < Variables.Spells[SpellSlot.E].Range);

            if (jumptarget != null)
            {
                position = jumptarget.ServerPosition;
            }

            if (jumptarget == null)
            {
                return ObjectManager.GetLocalPlayer()
                    .ServerPosition.Extend(Variables.NexusPosition, Variables.Spells[SpellSlot.E].Range);
            }
            return position;
        }
    }
}