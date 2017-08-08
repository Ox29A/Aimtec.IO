namespace iLulu.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Aimtec;
    using Aimtec.SDK.Orbwalking;

    using iLulu.Interfaces;
    using iLulu.Modules.Combo;
    using iLulu.Modules.OnSpellCastModules;
    using iLulu.Modules.UpdateModules;
    using iLulu.Utils;

    using ZLib;

    /// <summary>
    ///     The module manager.
    /// </summary>
    internal class ModuleManager
    {
        /// <summary>
        ///     The modules.
        /// </summary>
        private static readonly List<IModule> Modules = new List<IModule>
                                                            {
                                                                new CastQModule(),
                                                                new CastWModule(),
                                                                new CastEModule(),
                                                                new CastRModule(),

                                                                new AutoCaster(),
                                                                new AutoSaver(),
                                                                new SpeedyGonzales(),
                                                                new SpecialSpellsHelperModule(),
                                                                new AntigapcloserModule()
                                                            };

        /// <summary>
        ///     The on load.
        /// </summary>
        public static void OnLoad()
        {
            foreach (var module in Modules) module.OnLoad();

            Game.OnUpdate += OnUpdate;
            Obj_AI_Base.OnProcessSpellCast += RunEventModule;
            Orbwalker.Implementation.PostAttack += RunEventModule;
            ZLib.OnPredictDamage += RunEventModule;
            Gapcloser.OnGapcloser += RunEventModule;
        }

        public static void RunEventModule<T, TE>(T sender, TE args)
        {
            foreach (var module in Modules.Where(x => x.CanExecute() && x.GetType().GetInterfaces().Contains(typeof(IEventModule<T, TE>))))
            {
                (module as IEventModule<T, TE>)?.Execute(sender, args);
            }
        }

        /// <summary>
        ///     The on update.
        /// </summary>
        private static void OnUpdate()
        {
            foreach (var module in Modules.Where(
                x => x.CanExecute() && x.GetType().GetInterfaces().Contains(typeof(IUpdateModule))))
            {
                var updateModule = module as IUpdateModule;
                updateModule?.Execute();
            }
        }
    }
}