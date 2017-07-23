using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Aimtec;
using Aimtec.SDK.Orbwalking;
using iKalista.Modules.impl.Combo;
using iKalista.Modules.impl.Misc;
using iKalista.Utils;

namespace iKalista.Modules
{
    internal static class ModuleMemeger
    {
        private static readonly List<IOnUpdateModule> UpdateModules = new List<IOnUpdateModule>
        {
            new KillstealModule(),
            new AutoEModule(),
            new JungleStealModule(),
            new AutoQModule(),
            new BalistaModule(),
            new SaveAllyModule()
        };

        private static readonly List<IPreAttackModule> PreAttackModules = new List<IPreAttackModule>
        {
            new ForceTargetModule()
        };

        private static readonly List<IPostAttackModule> PostAttackModules = new List<IPostAttackModule>();

        private static readonly List<IUnkillableMinionModule> UnkillableMinionModule = new List<IUnkillableMinionModule>()
        {
            new UnkillableMinionModuleModule()
        };

        private static readonly List<ISpellCastModule> SpellCastModules = new List<ISpellCastModule>
        {
            new SaveAllyModule()
        };

        public static void Initialize()
        {
            Game.OnUpdate += OnUpdate;
            Orbwalker.Implementation.PreAttack += OnPreAttack;
            Orbwalker.Implementation.PostAttack += OnPostAttack;
            Variables.Orbwalker.OnNonKillableMinion += OnNonKillableMinion;
            Obj_AI_Base.OnProcessSpellCast += OnSpellCast;

            foreach (var module in UpdateModules)
            {
                module.OnLoad();
            }
        }

        private static void OnSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs e)
        {
            foreach (var spellCastModule in SpellCastModules.Where(
                x => x.ShouldExecute() && x.GetModuleType() == ModuleType.OnProcessSpell))
                spellCastModule.OnSpellCast(sender, e);
        }

        private static void OnPreAttack(object sender, PreAttackEventArgs preAttackEventArgs)
        {
            foreach (var preAttackModule in PreAttackModules.Where(
                x => x.ShouldExecute() && x.GetModuleType() == ModuleType.PreAttack))
                preAttackModule.OnPreAttack(sender as Obj_AI_Base, preAttackEventArgs);
        }

        private static void OnPostAttack(object sender, PostAttackEventArgs postAttackEventArgs)
        {
            foreach (var postAttackModule in PostAttackModules.Where(
                x => x.ShouldExecute() && x.GetModuleType() == ModuleType.PostAttack))
                postAttackModule.OnPostAttack(sender as Obj_AI_Base, postAttackEventArgs);
        }

        private static void OnNonKillableMinion(object sender, NonKillableMinionEventArgs e)
        {
            foreach (var unkillableModule in UnkillableMinionModule.Where(
                x => x.ShouldExecute() && x.GetModuleType() == ModuleType.OnUnkillableMinion))
                unkillableModule.OnNonKillableMinions(sender, e);
        }

        private static void OnUpdate()
        {
            foreach (var onUpdateModule in UpdateModules.Where(
                x => x.ShouldExecute() && x.GetModuleType() == ModuleType.OnUpdate))
            {
                onUpdateModule.Execute();
            }
        }
    }
}