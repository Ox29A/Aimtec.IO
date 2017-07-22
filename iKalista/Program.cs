using System;
using Aimtec.SDK.Events;

namespace iKalista
{
    internal class Program
    {
        /** 
        *   TODO: combo : auto E , auto Q 
            harass : harass with minions + E , auto Q , E if x stacks 
            Misc : auto R on blitz Q , auto W on drake blue and red , etc 
            use items like botrk etc 
            E should calculate factors like shields , dmg modifiers , exhaust etc 
            Q through minions for harass 
            Q to cancel AA
            last hit with E
            if enemy out of range should dash off minons to reach him 
            add wall dash spots using Q
            E dmg drawing  ofc
            use E before die
            save ally with ult
            the best feature that should be focused at is the harassing using minions with E
            Use E farming jungle
        */
        private static void Main(string[] args)
        {
            var kalista = new Kalista();
            GameEvents.GameStart += kalista.OnGameLoad;
        }
    }
}