namespace iLulu.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Aimtec;

    using iLulu.Interfaces;
    using iLulu.Modules.Combo;
    using iLulu.Modules.OnSpellCastModules;
    using iLulu.Modules.UpdateModules;

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
                                                                new AutoSaver(),
                                                                new AutoCaster(),
                                                                new SpeedyGonzales(),
                                                                // Spell Cast Modules
                                                                new SpecialSpellsHelperModule(),
                                                                new AutoSpellInitiators()
                                                            };

        /// <summary>
        ///     The on load.
        /// </summary>
        public static void OnLoad()
        {
            foreach (var module in Modules) module.OnLoad();

            Game.OnUpdate += OnUpdate;
            Obj_AI_Base.OnProcessSpellCast += OnSpellCast;
        }

        private static void OnSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs args)
        {
            foreach (var module in Modules.Where(
                x => x.CanExecute() && x.GetType().GetInterfaces().Contains(typeof(ISpellCastModule))))
            {
                var spellCastModule = module as ISpellCastModule;
                spellCastModule?.Execute(sender, args);
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