using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aimtec;
using Aimtec.SDK.Util;
using iTwitch.Module_System.Enumerations;
using iTwitch.Utils;

namespace iTwitch.Module_System.Misc
{
    class StealthRecall : IModule
    {
        public string GetName()
        {
            return "Stealth Recall Module";
        }

        public string GetDescription()
        {
            return "Handles the stealth recall aspect of the assembly";
        }

        public void OnLoad()
        {
            Console.WriteLine("Stealth recall module loaded.");

            SpellBook.OnCastSpell += OnSpellCast;
        }

        public bool CanExecute()
        {
            return Variables.Spells[SpellSlot.Q].Ready && Variables.Menu["com.itwitch.misc"]["stealthrecall"].Enabled;
        }

        public void Execute()
        {
        }

        private void OnSpellCast(Obj_AI_Base sender, SpellBookCastSpellEventArgs e)
        {
            if (!sender.IsMe)
                return;

            if (e.Slot == SpellSlot.Recall && Variables.Spells[SpellSlot.Q].Ready)
            {
                DelayAction.Queue((int)ObjectManager.GetLocalPlayer().SpellBook.GetSpell(SpellSlot.Q).SpellData.SpellCastTime + 300,
                    () => ObjectManager.GetLocalPlayer().SpellBook.CastSpell(SpellSlot.Recall));
            }
        }

        public ModulePriority GetPriority()
        {
            return ModulePriority.Low;
        }

        public ModuleType GetModuleType()
        {
            return ModuleType.OnUpdate;
        }
    }
}
