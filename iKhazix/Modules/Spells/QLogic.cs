namespace iKhazix.Modules.Spells
{
    using System;

    using Aimtec;
    using Aimtec.SDK.Orbwalking;

    using iKhazix.Interfaces;

    using iLulu.Utils;

    public class QLogic : IUpdateModule
    {
        public void OnLoad()
        {
            Console.WriteLine("QLogic Module Loaded");
        }

        public string GetName()
        {
            return "QLogic Module";
        }

        public string GetDescription()
        {
            return "Handles all the Q Logic";
        }

        public bool CanExecute()
        {
            return Variables.Spells[SpellSlot.Q].Ready;
        }

        public void Execute()
        {
            switch (Variables.Orbwalker.Mode)
            {
                    case OrbwalkingMode.Combo:
                        ComboLogic();
                    break;
                    case OrbwalkingMode.Mixed: break;
                    case OrbwalkingMode.Laneclear: break;
            }
        }

        private void ComboLogic()
        {
            
        }
    }
}
