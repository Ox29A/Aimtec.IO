using System;
using Aimtec;
using Aimtec.SDK.Util;
using iTwitch.Module_System.Enumerations;
using iTwitch.Utils;

namespace iTwitch.Module_System.Misc
{
    internal class StealthRecall : IModule
    {
        public string GetName() => "Stealth Recall Module";

        public string GetDescription() => "Handles the stealth recall aspect of the assembly";

        public void OnLoad()
        {
            Console.WriteLine("Stealth recall module loaded.");

            SpellBook.OnCastSpell += OnSpellCast;
        }

        public bool CanExecute() => Variables.Spells[SpellSlot.Q].Ready;

        public void Execute()
        {
            if (Variables.Menu["com.itwitch.misc"]["stealthrecall"].Enabled)
                ObjectManager.GetLocalPlayer().SpellBook.CastSpell(SpellSlot.Recall);
        }

        public ModulePriority GetPriority() => ModulePriority.Low;

        public ModuleType GetModuleType() => ModuleType.OnUpdate;

        private void OnSpellCast(Obj_AI_Base sender, SpellBookCastSpellEventArgs e)
        {
            if (!sender.IsMe)
                return;

            if (e.Slot == SpellSlot.Recall && Variables.Spells[SpellSlot.Q].Ready)
                DelayAction.Queue(
                    (int) ObjectManager.GetLocalPlayer().SpellBook.GetSpell(SpellSlot.Q).SpellData.SpellCastTime + 300,
                    () => ObjectManager.GetLocalPlayer().SpellBook.CastSpell(SpellSlot.Recall));
        }
    }
}