using System;
using System.Collections.Generic;
using System.Linq;
using Aimtec;
using Aimtec.SDK.Orbwalking;
using iKalista.Modules.impl.Combo;
using iKalista.Modules.impl.Misc;

namespace iKalista.Modules
{
    internal static class ModuleMemeger
    {
        private static readonly List<IModule> Modules = new List<IModule>
        {
            new AutoEModule(),
            new AutoQModule(),
            new KillstealModule()
        };

        public static void Initialize()
        {
            Game.OnUpdate += OnUpdate;
            Orbwalker.Implementation.PreAttack += OnPreAttack;
            Orbwalker.Implementation.PostAttack += OnPostAttack;

            foreach (var module in Modules)
                Console.WriteLine("Module: " + module.GetName() + " Type: " + module.GetModuleType() +
                                  " succesfully loaded.");
        }

        private static void OnPreAttack(object sender, PreAttackEventArgs preAttackEventArgs)
        {
            if (Modules.Count <= 0)
                return;

            foreach (var module in Modules.Where(x => x.ShouldExecute() && x.GetModuleType() == ModuleType.PreAttack))
                module.Execute();
        }

        private static void OnPostAttack(object sender, PostAttackEventArgs postAttackEventArgs)
        {
            if (Modules.Count <= 0)
                return;

            foreach (var module in Modules.Where(x => x.ShouldExecute() && x.GetModuleType() == ModuleType.PostAttack))
                module.Execute();
        }

        private static void OnUpdate()
        {
            if (Modules.Count <= 0)
                return;

            foreach (var module in Modules.Where(x => x.ShouldExecute() && x.GetModuleType() == ModuleType.OnUpdate))
                module.Execute();
        }
    }
}