using Aimtec;
using Aimtec.SDK.Orbwalking;

namespace iKalista.Modules
{
    internal interface IModule
    {
        /// <summary>
        ///     On Load
        /// </summary>
        void OnLoad();

        /// <summary>
        ///     Gets the Module Name
        /// </summary>
        /// <returns>string</returns>
        string GetName();

        /// <summary>
        ///     Checks if the module can be executed
        /// </summary>
        /// <returns></returns>
        bool ShouldExecute();
    }

    internal interface IUpdateModule : IModule
    {
        void Execute();
    }

    internal interface IEventModule<in T, in TE> : IModule
    {
        void Execute(T sender, TE args);
    }
}