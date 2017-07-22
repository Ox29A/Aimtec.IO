using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aimtec;
using Aimtec.SDK.Menu;
using Aimtec.SDK.Orbwalking;
using Spell = Aimtec.SDK.Spell;

namespace iTwitch.Utils
{
    internal class Variables
    {
        public static readonly Dictionary<SpellSlot, Spell> Spells = new Dictionary<SpellSlot, Spell>
        {
            {SpellSlot.Q, new Spell(SpellSlot.Q)},
            {SpellSlot.W, new Spell(SpellSlot.W, 950f)},
            {SpellSlot.E, new Spell(SpellSlot.E, 1100)},
            {SpellSlot.R, new Spell(SpellSlot.R)}
        };

        public static Menu Menu { get; set; }
        public static Orbwalker Orbwalker { get; set; }



    }
}
