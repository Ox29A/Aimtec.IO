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
            foreach (var module in ModuleList)
                module.OnLoad();

            Game.OnUpdate += OnUpdate;
            Orbwalker.Implementation.PreAttack += RunEventModule;
            Orbwalker.Implementation.PostAttack += RunEventModule;
            Variables.Orbwalker.OnNonKillableMinion += RunEventModule;
            Obj_AI_Base.OnProcessSpellCast += RunEventModule;
            ZLib.ZLib.OnPredictDamage += RunEventModule;
        }

        private static void OnUpdate()
        {
            foreach (var module in ModuleList.Where(
                x => x.ShouldExecute() && x.GetType().GetInterfaces().Contains(typeof(IUpdateModule))))
            {
                var updateModule = module as IUpdateModule;
                updateModule?.Execute();
            }
        }

        public static void RunEventModule<T, TE>(T sender, TE args)
        {
            foreach (var module in ModuleList.Where(x => x.ShouldExecute() && x.GetType().GetInterfaces().Contains(typeof(IEventModule<T, TE>))))
            {
                (module as IEventModule<T, TE>)?.Execute(sender, args);
            }
        }
    }
}