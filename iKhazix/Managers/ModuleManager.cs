namespace iKhazix.Managers
{
    using System.Collections.Generic;
    using System.Linq;

    using Aimtec;
    using Aimtec.SDK.Orbwalking;

    using iKhazix.Interfaces;

    public class ModuleManager
    {
        /// <summary>
        ///     The modules.
        /// </summary>
        private static readonly List<IModule> Modules = new List<IModule>
                                                            {
                                                                // TODO
                                                            };

        /// <summary>
        ///     The on load.
        /// </summary>
        public static void OnLoad()
        {
            foreach (var module in Modules)
                module.OnLoad();

            Game.OnUpdate += OnUpdate;
            Obj_AI_Base.OnProcessSpellCast += RunEventModule;
            Orbwalker.Implementation.PostAttack += RunEventModule;
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
