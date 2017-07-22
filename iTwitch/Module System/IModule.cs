using iTwitch.Module_System.Enumerations;

namespace iTwitch.Module_System
{
    internal interface IModule
    {
        /// <summary>
        ///     Gets the specified Module Name.
        /// </summary>
        /// <returns>The Module Name</returns>
        string GetName();

        /// <summary>
        ///     Gets the specified Module Description.
        /// </summary>
        /// <returns></returns>
        string GetDescription();

        /// <summary>
        ///     Runs as the <see cref="IModule" /> gets loaded in game, also use this for declaring Orbwalker actions
        ///     such as Orbwalking.OnAfterAttack.
        /// </summary>
        void OnLoad();

        /// <summary>
        ///     Checks if the conditions have been met so the <see cref="IModule" /> can run.
        /// </summary>
        /// <returns>true or false ofc</returns>
        bool CanExecute();

        /// <summary>
        ///     Executes the given <see cref="IModule" />
        /// </summary>
        void Execute();

        /// <summary>
        ///     Gets the specified <see cref="ModulePriority" /> for the given module.
        /// </summary>
        /// <returns></returns>
        ModulePriority GetPriority();

        /// <summary>
        ///     Gets the specified <see cref="ModuleType" /> for the given module.
        /// </summary>
        /// <returns></returns>
        ModuleType GetModuleType();
    }
}