using Aimtec;
using Aimtec.SDK.Orbwalking;

namespace iKalista.Modules
{
    internal interface IModule
    {
        void OnLoad();

        string GetName();

        bool ShouldExecute();

        ModuleType GetModuleType();
    }

    internal interface IOnUpdateModule : IModule
    {
        void Execute();
    }

    internal interface IPostAttackModule : IModule
    {
        void OnPostAttack(Obj_AI_Base sender, PostAttackEventArgs args);
    }

    internal interface IPreAttackModule : IModule
    {
        void OnPreAttack(Obj_AI_Base sender, PreAttackEventArgs args);
    }

    internal interface ISpellCastModule : IModule
    {
        void OnSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs args);
    }

    internal interface IUnkillableMinionModule : IModule
    {
        void OnNonKillableMinions(object sender, NonKillableMinionEventArgs args);
    }
}