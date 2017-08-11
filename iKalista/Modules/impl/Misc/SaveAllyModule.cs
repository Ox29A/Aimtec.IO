using System;
using System.Collections.Generic;
using System.Linq;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Util.Cache;
using iKalista.Utils;
using ZLib.Base;
using ZLib.Handlers;
using GameObjects = iKalista.Utils.GameObjects;

namespace iKalista.Modules.impl.Misc
{
    internal class SaveAllyModule : IUpdateModule, IEventModule<Unit, PredictDamageEventArgs>
    {
        public static Obj_AI_Hero SoulBoundAlly;

        
        public void OnLoad()
        {

        }

        public string GetName() => "Save Ally Module";

        public bool ShouldExecute() => Variables.Spells[SpellSlot.R].Ready &&
                                       Variables.Menu["com.ikalista.combo"]["saveAlly"].Enabled;

        public void Execute()
        {
            if (SoulBoundAlly == null)
                SoulBoundAlly =
                    GameObjects.AllyHeroes.ToList().Find(
                        h => !h.IsMe && h.Buffs.Any(b => b.Caster.IsMe && b.Name == "kalistacoopstrikeally"));
        }

        public ModuleType GetModuleType() => ModuleType.OnUpdate;


        public void Execute(Unit unit, PredictDamageEventArgs args)
        {
            //Cast unit to OBJ_AI_Hero fam.
            var sender = unit.Instance as Obj_AI_Hero;

            // Our unit is null, don't do anything now.
            if (sender == null)
                return;

            var shouldCast = SoulBoundAlly != null && sender.ChampionName.Equals(SoulBoundAlly.ChampionName);

            if (shouldCast && ObjectManager.GetLocalPlayer().Distance(SoulBoundAlly) <=
                Variables.Spells[SpellSlot.E].Range)
            {
                if (unit.Events.Contains(EventType.Danger) || unit.Events.Contains(EventType.Ultimate))
                {
                    if (sender.CountEnemyChampionsInRange(2000) > 0)
                    {
                        Variables.Spells[SpellSlot.R].Cast();
                        return;
                    }
                }

                var incomingDamagePercent = unit.IncomeDamage / sender.MaxHealth * 100;

                if (unit.IncomeDamage >= sender.Health || incomingDamagePercent >= 50 || sender.HealthPercent() <=
                    Variables.Menu["com.ikalista.combo"]["allyPercent"].Value)
                {
                    Variables.Spells[SpellSlot.R].Cast();
                }
            }
        }

        public void Execute(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs args)
        {
            /*if (!sender.IsEnemy)
                return;

            if (SoulBoundAlly == null)
                return;

            if ((!(sender is Obj_AI_Hero) || args.SpellData.ConsideredAsAutoAttack) && args.Target != null &&
                args.Target.NetworkId == SoulBoundAlly.NetworkId)
            {
                IncDamage[
                    SoulBoundAlly.ServerPosition.Distance(sender.ServerPosition) / args.SpellData.MissileSpeed +
                    Game.TickCount] = (float) sender.GetAutoAttackDamage(SoulBoundAlly);
            }
            else
            {
                var attacker = sender as Obj_AI_Hero;
                if (attacker == null)
                    return;

                var slot = attacker.GetSpellSlotFromName(args.SpellData.Name);

                if (slot == SpellSlot.Unknown)
                    return;

                //TODO summoner spells damage
                if (slot == SpellSlot.Q || slot == SpellSlot.W || slot == SpellSlot.E || slot == SpellSlot.R)
                    if (args.Target != null && args.Target.NetworkId == SoulBoundAlly.NetworkId ||
                        args.End.Distance(SoulBoundAlly.ServerPosition) <
                        Math.Pow(args.SpellData.LineWidth, 2))
                        InstDamage[Game.TickCount + 2] = (float) attacker.GetSpellDamage(SoulBoundAlly, slot);
            }*/
        }
    }
}
