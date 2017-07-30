using System.Collections.Generic;
using System.Linq;
using Aimtec;
using Aimtec.SDK.Orbwalking;
using iKalista.Modules.impl.Combo;
using iKalista.Modules.impl.Misc;
using iKalista.Utils;

namespace iKalista.Modules
{
    internal static class ModuleMemeger
    {
        private static readonly List<IModule> ModuleList = new List<IModule>
        {
            new KillstealModule(),
            new AutoEModule(),
            new JungleStealModule(),
            new AutoQModule(),
            new BalistaModule(),
            new SaveAllyModule(),
            new UnkillableMinionModuleModule(),
            new ForceTargetModule()
        };

        public static void Initialize()
        {
            Game.OnUpdate += OnUpdate;
            Orbwalker.Implementation.PreAttack += OnPreAttack;
            Orbwalker.Implementation.PostAttack += OnPostAttack;
            Variables.Orbwalker.OnNonKillableMinion += OnNonKillableMinion;
            Obj_AI_Base.OnProcessSpellCast += OnSpellCast;

            foreach (var module in ModuleList)
                module.OnLoad();
        }

        private static void OnSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs e)
        {
            foreach (var m in ModuleList.Where(x => x.ShouldExecute()))
                if (m.GetType().GetInterfaces().Contains(typeof(ISpellCastModule)))
                {
                    var module = m as ISpellCastModule;
                    module?.OnSpellCast(sender, e);
                }
        }

        private static void OnPreAttack(object sender, PreAttackEventArgs preAttackEventArgs)
        {
            foreach (var m in ModuleList.Where(x => x.ShouldExecute()))
                if (m.GetType().GetInterfaces().Contains(typeof(IPreAttackModule)))
                {
                    var module = m as IPreAttackModule;
                    module?.OnPreAttack(sender as Obj_AI_Base, preAttackEventArgs);
                }
        }

        private static void OnPostAttack(object sender, PostAttackEventArgs postAttackEventArgs)
        {
            foreach (var m in ModuleList.Where(x => x.ShouldExecute()))
                if (m.GetType().GetInterfaces().Contains(typeof(IPostAttackModule)))
                {
                    var module = m as IPostAttackModule;
                    module?.OnPostAttack(sender as Obj_AI_Base, postAttackEventArgs);
                }
        }

        private static void OnNonKillableMinion(object sender, NonKillableMinionEventArgs e)
        {
            foreach (var m in ModuleList.Where(x => x.ShouldExecute()))
                if (m.GetType().GetInterfaces().Contains(typeof(IUnkillableMinionModule)))
                {
                    var module = m as IUnkillableMinionModule;
                    module?.OnNonKillableMinions(sender, e);
                }
        }

        private static void OnUpdate()
        {
            foreach (var m in ModuleList.Where(x => x.ShouldExecute()))
                if (m.GetType().GetInterfaces().Contains(typeof(IOnUpdateModule)))
                {
                    var module = m as IOnUpdateModule;
                    module?.Execute();
                }
        }
    }
}