using System;
using System.Collections.Generic;
using System.Linq;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Util.Cache;
using iKalista.Utils;
using GameObjects = iKalista.Utils.GameObjects;

namespace iKalista.Modules.impl.Misc
{
    internal class SaveAllyModule : IUpdateModule, IEventModule<Obj_AI_Base, Obj_AI_BaseMissileClientDataEventArgs>
    {
        private static readonly Dictionary<float, float> IncDamage = new Dictionary<float, float>();
        private static readonly Dictionary<float, float> InstDamage = new Dictionary<float, float>();
        public static Obj_AI_Hero SoulBoundAlly;

        public static float IncomingDamage
        {
            get { return IncDamage.Sum(e => e.Value) + InstDamage.Sum(e => e.Value); }
        }

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

            if (SoulBoundAlly.HealthPercent() <
                Variables.Menu["com.ikalista.combo"]["allyPercent"].Value &&
                SoulBoundAlly.CountEnemyChampionsInRange(500) > 0 ||
                IncomingDamage > SoulBoundAlly.Health)
                Variables.Spells[SpellSlot.R].Cast();

            foreach (var entry in IncDamage.Where(entry => entry.Key < Game.TickCount).ToArray())
                IncDamage.Remove(entry.Key);

            foreach (var entry in InstDamage.Where(entry => entry.Key < Game.TickCount).ToArray())
                InstDamage.Remove(entry.Key);
        }

        public ModuleType GetModuleType() => ModuleType.OnUpdate;

        public void Execute(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs args)
        {
            if (!sender.IsEnemy)
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
            }
        }
    }
}
