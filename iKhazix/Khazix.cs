using System.Collections.Generic;
using System.Linq;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Damage.JSON;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Menu.Components;
using Aimtec.SDK.Orbwalking;
using Aimtec.SDK.Prediction.Skillshots;
using Aimtec.SDK.TargetSelector;
using Aimtec.SDK.Util;
using Aimtec.SDK.Util.Cache;
using iKhazix.Utils;

namespace iKhazix
{
    using iKhazix.Managers;

    public class Khazix
    {

        private static readonly JumpManager JumpManager = new JumpManager();

        public static void OnLoad()
        {
            MenuManager.Initialize();
            SetSkillshots();


            Variables.NexusPosition = new Vector3(200, 0, 0);

            Game.OnUpdate += OnUpdate;
            Game.OnUpdate += DoubleJump;
            Render.OnRender += OnRender;
            SpellBook.OnCastSpell += OnSpellCast;
        }

        private static void OnSpellCast(Obj_AI_Base sender, SpellBookCastSpellEventArgs args)
        {
            if (!Variables.HasEvolvedE || !Variables.Menu["safety"]["save"].Enabled)
                return;

            if (args.Slot == SpellSlot.Q && args.Target is Obj_AI_Hero &&
                Variables.Menu["doubleJump"]["enabled"].Enabled)
            {
                var target = (Obj_AI_Hero) args.Target;
                var qDamage = Extensions.GetCorrectQDamage(target);
                var damage = ObjectManager.GetLocalPlayer().GetAutoAttackDamage(target) * 2 + qDamage;

                if (target.Health < damage && target.Health > qDamage)
                {
                    args.Process = false;
                }
            }
        }

        private static void DoubleJump()
        {
            if (!Variables.Spells[SpellSlot.E].Ready || !Variables.HasEvolvedE || !Variables.Menu["doubleJump"]["enabled"].Enabled || ObjectManager.GetLocalPlayer().IsDead ||ObjectManager.GetLocalPlayer().IsRecalling() || JumpManager.MidAssasination)
                return;

            var targets = GameObjects.EnemyHeroes.Where(
                x => x.IsValidTarget() && !x.IsInvulnerable && !x.IsClone && !x.HasSpellShield());

            if (Variables.Spells[SpellSlot.Q].Ready && Variables.Spells[SpellSlot.E].Ready)
            {
                var checkKillable = targets.FirstOrDefault(
                    x => Vector3.Distance(ObjectManager.GetLocalPlayer().ServerPosition, x.ServerPosition) <
                         Variables.Spells[SpellSlot.Q].Range - 25 &&
                         Extensions.GetCorrectQDamage(x) > x.Health);

                if (checkKillable != null)
                {
                    Variables.IsAirborne = true;
                    Variables.FirstJumpPoint = Extensions.GetDoubleJumpPoint(checkKillable);
                    Variables.Spells[SpellSlot.E].Cast(Variables.FirstJumpPoint.To2D());
                    Variables.Spells[SpellSlot.Q].Cast(checkKillable);
                    DelayAction.Queue(Variables.Menu["doubleJump"]["EDelay"].Value + Game.Ping, () =>
                    {
                        if (Variables.Spells[SpellSlot.E].Ready)
                        {
                            Variables.SecondJumpPoint = Extensions.GetDoubleJumpPoint(checkKillable, false);
                            Variables.Spells[SpellSlot.E].Cast(Variables.SecondJumpPoint.To2D());
                        }
                        Variables.IsAirborne = false;
                    });
                }
            }
        }

        private static void OnUpdate()
        {
            if (ObjectManager.GetLocalPlayer().IsDead || ObjectManager.GetLocalPlayer().IsRecalling())
                return;

            EvolveCheck();

            if (Variables.Menu["assMenu"]["assasinate"].As<MenuKeyBind>().Enabled)
            {
                JumpManager.Assasinate();
            }

            switch (Variables.Orbwalker.Mode)
            {
                    case OrbwalkingMode.Combo:
                        Combo();
                        break;
                    case OrbwalkingMode.Mixed:
                        Harass();
                        break;
                    case OrbwalkingMode.Laneclear:
                        Laneclear();
                        break;

                    //TODO add custom orbwalking mode.
            }
        }

        private static void Combo()
        {
            var searchRange = ObjectManager.GetLocalPlayer().AttackRange;

            if (Variables.Spells[SpellSlot.Q].Ready)
            {
                searchRange += Variables.Spells[SpellSlot.Q].Range;
            }

            if (Variables.Spells[SpellSlot.E].Ready)
            {
                searchRange += Variables.Spells[SpellSlot.E].Range;
            }
            else if (Variables.Spells[SpellSlot.W].Ready)
            {
                searchRange += Variables.Spells[SpellSlot.W].Range;
            }

            var target = TargetSelector.GetTarget(searchRange);

            foreach (var hero in GameObjects.EnemyHeroes.Where(x => x.IsValidTarget(searchRange) && !x.HasSpellShield() && !x.IsInvulnerable))
            {
                if (hero.IsIsolated() || target.Health <= ObjectManager.GetLocalPlayer().GetBurstDamage(target))
                    target = hero;
            }

            if (target != null)
            {
                if (target.IsIsolated() || target.Health <= ObjectManager.GetLocalPlayer().GetBurstDamage(target))
                {
                    var distance = ObjectManager.GetLocalPlayer().Distance(target.ServerPosition);

                    if (Variables.Menu["combo"]["useQ"].Enabled && Variables.Spells[SpellSlot.Q].Ready &&
                        !Variables.IsAirborne && distance <= Variables.Spells[SpellSlot.Q].Range)
                    {
                        Variables.Spells[SpellSlot.Q].Cast();
                    }

                    if (Variables.Menu["combo"]["useW"].Enabled && Variables.Spells[SpellSlot.W].Ready &&
                        !Variables.HasEvolvedW && distance <= Variables.Spells[SpellSlot.E].Range)
                    {
                        var prediction = Variables.Spells[SpellSlot.W].GetPrediction(target);
                        if (prediction.HitChance >= Extensions.GetHitchance("wHitchance"))
                        {
                            Variables.Spells[SpellSlot.W].Cast(prediction.CastPosition);
                        }
                    }

                    if (Variables.Menu["combo"]["useE"].Enabled && Variables.Spells[SpellSlot.E].Ready &&
                        !Variables.IsAirborne && distance <= Variables.Spells[SpellSlot.E].Range && distance >
                        Variables.Spells[SpellSlot.Q].Range + (0.4 * ObjectManager.GetLocalPlayer().MoveSpeed))
                    {
                        var jumpPosition = Extensions.GetJumpPosition(target);
                        if (jumpPosition.ShouldJump)
                        {
                            Variables.Spells[SpellSlot.E].Cast(jumpPosition.Position);
                        }
                    }

                    if (Variables.Menu["combo"]["useEGapcloseQ"].Enabled && Variables.Spells[SpellSlot.Q].Ready &&
                        Variables.Spells[SpellSlot.E].Ready &&
                        distance > Variables.Spells[SpellSlot.Q].Range +
                        (0.4 * ObjectManager.GetLocalPlayer().MoveSpeed) && distance <=
                        Variables.Spells[SpellSlot.E].Range + Variables.Spells[SpellSlot.Q].Range)
                    {
                        var jumpPosition = Extensions.GetJumpPosition(target);
                        if (jumpPosition.ShouldJump)
                        {
                            Variables.Spells[SpellSlot.E].Cast(jumpPosition.Position);
                        }

                        if (Variables.Menu["combo"]["useRLongGap"].Enabled && Variables.Spells[SpellSlot.R].Ready)
                        {
                            Variables.Spells[SpellSlot.R].Cast();
                        }
                    }

                    if (Variables.Spells[SpellSlot.R].Ready && !Variables.Spells[SpellSlot.Q].Ready &&
                        !Variables.Spells[SpellSlot.W].Ready && !Variables.Spells[SpellSlot.E].Ready &&
                        Variables.Menu["combo"]["useR"].Enabled &&
                        ObjectManager.GetLocalPlayer().CountEnemyHeroesInRange(500) > 0)
                    {
                        Variables.Spells[SpellSlot.R].Cast();
                    }

                    //Evolved Spells

                    if (Variables.Spells[SpellSlot.W].Ready && Variables.HasEvolvedW &&
                        distance <= Variables.WeSpell.Range && Variables.Menu["combo"]["useW"].Enabled)
                    {
                        var prediction = Variables.WeSpell.GetPrediction(target);
                        if (prediction.HitChance >= Extensions.GetHitchance("wHitchance"))
                        {
                            CastWE(target, prediction.UnitPosition.To2D(), 0, Extensions.GetHitchance("wHitchance"));
                        }
                        if (prediction.HitChance >= HitChance.Collision)
                        {
                            var pCollision = prediction.CollisionObjects;
                            var x = pCollision.FirstOrDefault(predCollisionChar => predCollisionChar.Distance(target) <= 30);
                            if (x != null)
                            {
                                Variables.Spells[SpellSlot.W].Cast(x.Position);
                            }
                        }
                    }

                    // TODO items and smite??

                }
            }
        }

        private static void Harass()
        {
            
        }

        private static void Laneclear()
        {
            
        }

        private static void OnRender()
        {
        }

        public static void ExecuteAssasinationCombo(Obj_AI_Hero target)
        {
            if (target != null)
            {
                var distance = ObjectManager.GetLocalPlayer().Distance(target);

                if (Variables.Spells[SpellSlot.Q].Ready && distance <= Variables.Spells[SpellSlot.Q].Range)
                {
                    Variables.Spells[SpellSlot.Q].CastOnUnit(target);
                }

                if (Variables.Spells[SpellSlot.W].Ready && distance <= Variables.Spells[SpellSlot.W].Range && !Variables.HasEvolvedW)
                {
                    var prediction = Variables.Spells[SpellSlot.W].GetPrediction(target);
                    if (prediction.HitChance >= Extensions.GetHitchance("wHitchance"))
                    {
                        Variables.Spells[SpellSlot.W].Cast(prediction.CastPosition);
                    }
                } else if (Variables.Spells[SpellSlot.W].Ready && Variables.HasEvolvedW &&
                           distance <= Variables.WeSpell.Range)
                {
                    var prediction = Variables.WeSpell.GetPrediction(target);
                    CastWE(target, prediction.UnitPosition.To2D(), 0);
                }

            }
        }

        private static void CastWE(Obj_AI_Hero unit, Vector2 unitPosition, int minTargets = 0, HitChance hc = HitChance.Medium)
        {
            var points = new List<Vector2>();
            var hitBoxes = new List<int>();

            var startPoint = ObjectManager.GetLocalPlayer().ServerPosition.To2D();
            var originalDirection = Variables.Spells[SpellSlot.W].Range * (unitPosition - startPoint).Normalized();

            foreach (var enemy in GameObjects.EnemyHeroes)
            {
                if (!enemy.IsValidTarget() || enemy.NetworkId == unit.NetworkId)
                    continue;

                var pos = Variables.WeSpell.GetPrediction(enemy);

                if (pos.HitChance < hc)
                    continue;

                points.Add(pos.UnitPosition.To2D());
                hitBoxes.Add((int)enemy.BoundingRadius + 275);
            }

            var posiblePositions = new List<Vector2>();

            for (var i = 0; i < 3; i++)
            {
                if (i == 0)
                    posiblePositions.Add(unitPosition + originalDirection.Rotated(0));
                if (i == 1)
                    posiblePositions.Add(startPoint + originalDirection.Rotated(Variables.WAngle));
                if (i == 2)
                    posiblePositions.Add(startPoint + originalDirection.Rotated(-Variables.WAngle));
            }


            if (startPoint.Distance(unitPosition) < 900)
            {
                for (var i = 0; i < 3; i++)
                {
                    var pos = posiblePositions[i];
                    var direction = (pos - startPoint).Normalized().Perpendicular();
                    var k = (2f / 3f * (unit.BoundingRadius + Variables.Spells[SpellSlot.W].Width));
                    posiblePositions.Add(startPoint - k * direction);
                    posiblePositions.Add(startPoint + k * direction);
                }
            }

            var bestPosition = new Vector2();
            var bestHit = -1;

            foreach (var position in posiblePositions)
            {
                var hits = CountHits(position, points, hitBoxes);
                if (hits > bestHit)
                {
                    bestPosition = position;
                    bestHit = hits;
                }
            }

            if (bestHit + 1 <= minTargets)
                return;

            Variables.Spells[SpellSlot.W].Cast(bestPosition.To3D());
        }

        public static int CountHits(Vector2 position, List<Vector2> points, List<int> hitBoxes)
        {
            var result = 0;

            var startPoint = ObjectManager.GetLocalPlayer().ServerPosition.To2D();
            var originalDirection = Variables.Spells[SpellSlot.W].Range * (position - startPoint).Normalized();
            var originalEndPoint = startPoint + originalDirection;

            for (var i = 0; i < points.Count; i++)
            {
                var point = points[i];

                for (var k = 0; k < 3; k++)
                {
                    var endPoint = new Vector2();
                    if (k == 0)
                        endPoint = originalEndPoint;
                    if (k == 1)
                        endPoint = startPoint + originalDirection.Rotated(Variables.WAngle);
                    if (k == 2)
                        endPoint = startPoint + originalDirection.Rotated(-Variables.WAngle);

                    if (point.Distance(startPoint, endPoint, true, true) < (Variables.Spells[SpellSlot.W].Width + hitBoxes[i]) * (Variables.Spells[SpellSlot.W].Width + hitBoxes[i]))
                    {
                        result++;
                        break;
                    }
                }
            }
            return result;
        }


        private static void SetSkillshots()
        {
            Variables.Spells[SpellSlot.W].SetSkillshot(0.225f, 80f, 828.5f, true, SkillshotType.Line);
            Variables.WeSpell.SetSkillshot(0.225f, 100f, 828.5f, true, SkillshotType.Line);
            Variables.Spells[SpellSlot.E].SetSkillshot(0.25f, 300f, 1500f, false, SkillshotType.Circle);

            //TODO maybe add items
        }

        private static void EvolveCheck()
        {
            if (!Variables.HasEvolvedQ && ObjectManager.GetLocalPlayer().HasBuff("khazixqevo"))
            {
                Variables.Spells[SpellSlot.Q].Range = 375f;
                Variables.HasEvolvedQ = true;
            }
            if (!Variables.HasEvolvedW && ObjectManager.GetLocalPlayer().HasBuff("khazixwevo"))
            {
                Variables.HasEvolvedW = true;
                Variables.Spells[SpellSlot.W].SetSkillshot(0.225f, 100f, 828.5f, true, SkillshotType.Line);
            }
            if (!Variables.HasEvolvedE && ObjectManager.GetLocalPlayer().HasBuff("khazixeevo"))
            {
                Variables.Spells[SpellSlot.E].Range = 900f;
                Variables.HasEvolvedE = true;
            }
        }
    }
}
