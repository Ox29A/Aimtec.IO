using System;
using System.Linq;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Damage.JSON;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Orbwalking;
using Aimtec.SDK.Util.Cache;
using iKalista.Utils;
using GameObjects = iKalista.Utils.GameObjects;

namespace iKalista.Modules.impl.Combo
{
    internal class AutoEModule : IUpdateModule
    {
        public ModuleType GetModuleType()
        {
            return ModuleType.OnUpdate;
        }

        public void OnLoad()
        {
            Console.WriteLine("Loaded Module: "+this.GetName());
        }

        public string GetName()
        {
            return "Auto E Module";
        }

        public bool ShouldExecute()
        {
            return Variables.Spells[SpellSlot.E].Ready;
        }

        public void Execute()
        {
            if (Variables.Orbwalker.Mode == OrbwalkingMode.Combo)
            {
                //Console.WriteLine("Execute");
                if (Variables.Menu["com.ikalista.combo"]["useE"].Enabled)
                {
                    foreach (
                        var target in
                        GameObjects.EnemyHeroes.Where(
                            x => x.IsValid && x.HasRendBuff() && x.IsValidTarget(Variables.Spells[SpellSlot.E].Range)))
                        if (target.GetRendBuffCount() >= Variables.Menu["com.ikalista.combo"]["eStacks"].Value)
                        {
                            Variables.Spells[SpellSlot.E].Cast();
                        }
                }

                if (Variables.Menu["com.ikalista.combo"]["useELeaving"].Enabled)
                {
                    var target =
                        GameObjects.EnemyHeroes
                            .FirstOrDefault(
                                x => x.HasRendBuff() && x.IsValidTarget(Variables.Spells[SpellSlot.E].Range));
                    if (target == null) return;
                    var damage = Math.Ceiling(ObjectManager.GetLocalPlayer().GetSpellDamage(target, SpellSlot.E) * 100 /
                                              target.Health);
                    if (damage >= Variables.Menu["com.ikalista.combo"]["eLeavingPercent"].Value &&
                        target.ServerPosition.DistanceSquared(ObjectManager.GetLocalPlayer().ServerPosition) >
                        Math.Pow(Variables.Spells[SpellSlot.E].Range * 0.8, 2))
                    {
                        Variables.Spells[SpellSlot.E].Cast();
                    }
                }
            }

            if (Variables.Menu["com.ikalista.combo"]["autoEMinChamp"].Enabled && Variables.Orbwalker.Mode != OrbwalkingMode.None)
            {
                var enemy =
                    GameObjects.EnemyHeroes.Where(hero => hero.HasRendBuff()).OrderBy(hero => hero.Distance(ObjectManager.GetLocalPlayer())).FirstOrDefault();
                if (enemy != null && !(ObjectManager.GetLocalPlayer().Distance(enemy.ServerPosition) < Math.Pow(Variables.Spells[SpellSlot.E].Range + 200, 2)))
                    return;
                if (
                    ObjectManager.Get<Obj_AI_Minion>()
                        .Any(
                            x =>
                                x.IsValidTarget(Variables.Spells[SpellSlot.E].Range) && x.HasRendBuff() && x.IsRendKillable()))
                {
                    Variables.Spells[SpellSlot.E].Cast();
                }
            }
        }
    }
}