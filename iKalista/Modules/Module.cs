namespace iKalista.Modules
{
    internal interface IModule
    {
        string GetName();

        bool ShouldExecute();

        void Execute();

        ModuleType GetModuleType();
    }
}