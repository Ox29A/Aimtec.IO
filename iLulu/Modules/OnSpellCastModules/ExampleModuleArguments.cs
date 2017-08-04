namespace iLulu.Modules.OnSpellCastModules
{
    using Aimtec;
    using Aimtec.SDK.Extensions;

    using iLulu.Interfaces;
    using iLulu.Utils;

    public class ExampleModuleArguments : IEventModule<Obj_AI_Base, Obj_AI_BaseMissileClientDataEventArgs>
    {
        public bool CanExecute() => true;

        public void Execute(Obj_AI_Base sender1, Obj_AI_BaseMissileClientDataEventArgs args)
        {
           
        }

        public string GetDescription() => "Casts W / E on initiator spells";

        public string GetName() => "Auto Spell Initiator";

        public void OnLoad()
        {
        }
    }
}
