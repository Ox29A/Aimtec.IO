using System.Collections.Generic;
using Aimtec;
using Aimtec.SDK.Menu;
using Aimtec.SDK.Orbwalking;
using Spell = Aimtec.SDK.Spell;

namespace iKalista.Utils
{
    internal class Variables
    {
        public static readonly Dictionary<SpellSlot, Spell> Spells = new Dictionary<SpellSlot, Spell>
        {
            {SpellSlot.Q, new Spell(SpellSlot.Q, 1130)},
            {SpellSlot.W, new Spell(SpellSlot.W, 5200)},
            {SpellSlot.E, new Spell(SpellSlot.E, 950)},
            {SpellSlot.R, new Spell(SpellSlot.R, 1200)}
        };

        public static Menu Menu { get; set; }
        public static Orbwalker Orbwalker { get; set; }
    }
}