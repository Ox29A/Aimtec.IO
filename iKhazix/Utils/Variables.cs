using System;
using System.Collections.Generic;
using Aimtec;
using Aimtec.SDK.Menu;
using Aimtec.SDK.Orbwalking;

namespace iKhazix.Utils
{
    using Spell = Aimtec.SDK.Spell;

    public class Variables
    {
       
        public static Dictionary<SpellSlot, Spell> Spells =
            new Dictionary<SpellSlot, Spell>
                {
                    { SpellSlot.Q, new Spell(SpellSlot.Q, 325f) },
                    { SpellSlot.W, new Spell(SpellSlot.W, 1000f) },
                    { SpellSlot.E, new Spell(SpellSlot.E, 700f) },
                    { SpellSlot.R, new Spell(SpellSlot.R) }
                };

        public static Spell WeSpell = new Spell(SpellSlot.W, 1000f);

        public static bool HasEvolvedQ { get; set; }
        public static bool HasEvolvedW { get; set; }
        public static bool HasEvolvedE { get; set; }
        public static bool HasEvolvedR { get; set; }
                
        public static Menu Menu { get; set; }

        public static Orbwalker Orbwalker { get; set; }
        public static bool IsAirborne { get; set; }

        public static float WAngle = 22 * (float)Math.PI / 180;

        public static Vector3 FirstJumpPoint { get; set; }

        public static Vector3 SecondJumpPoint { get; set; }

        public static Vector3 NexusPosition { get; set; }


    }
}