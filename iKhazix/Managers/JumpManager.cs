using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.TargetSelector;
using Aimtec.SDK.Util;
using Aimtec.SDK.Util.Cache;
using iKhazix.Utils;

namespace iKhazix.Managers
{
    public class JumpManager
    {
        public Obj_AI_Hero AssasinationTarget;

        public bool MidAssasination;

        public Vector3 PreJumpPos;

        public float StartAssasinationTick;

        public bool HasEvolvedJump => Variables.HasEvolvedE;


        public void Assasinate()
        {
            if (HasEvolvedJump)
            {
                if (MidAssasination)
                {
                    if (AssasinationTarget == null || AssasinationTarget.IsDead ||
                        Game.TickCount - StartAssasinationTick > 2500)
                    {
                        MidAssasination = false;

                        if (Variables.Spells[SpellSlot.E].Ready)
                        {
                            var mode = Variables.Menu["combo"]["jumpMode"].Value; // 0 = old pos, 1 = mouse pos
                            var point = mode == 0 ? PreJumpPos : Game.CursorPos;

                            if (point.Distance(ObjectManager.GetLocalPlayer().ServerPosition) >
                                Variables.Spells[SpellSlot.E].Range)
                            {
                                PreJumpPos = ObjectManager.GetLocalPlayer()
                                    .ServerPosition.Extend(point, Variables.Spells[SpellSlot.E].Range);
                            }
                        }
                    }
                    else
                    {
                        Khazix.ExecuteAssasinationCombo(AssasinationTarget);
                    }
                }
                else if (Variables.Spells[SpellSlot.E].Ready)
                {
                    var selectedTarget = TargetSelector.GetSelectedTarget();
                    var bestEnemy =
                        selectedTarget != null && selectedTarget.IsInRange(Variables.Spells[SpellSlot.E].Range) &&
                        ObjectManager.GetLocalPlayer().GetBurstDamage(selectedTarget) >= selectedTarget.Health
                            ? selectedTarget
                            : GameObjects.EnemyHeroes
                                .Where(x => x.IsValidTarget(Variables.Spells[SpellSlot.E].Range) &&
                                            x.Health <= ObjectManager.GetLocalPlayer().GetBurstDamage(x))
                                .MaxBy(
                                    x => TargetSelector.Implementation.Config["TargetsMenu"][
                                            "priority" + selectedTarget?.ChampionName]
                                        .Value);

                    if (bestEnemy != null)
                    {
                        PreJumpPos = ObjectManager.GetLocalPlayer().ServerPosition;
                        Variables.Spells[SpellSlot.E].Cast(bestEnemy.ServerPosition);
                        AssasinationTarget = bestEnemy;
                        MidAssasination = true;
                        StartAssasinationTick = Game.TickCount;
                        DelayAction.Queue(2500, () => MidAssasination = false);
                    }
                }
            }
        }
    }
}