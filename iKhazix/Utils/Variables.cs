namespace iLulu.Utils
{
    using System.Collections.Generic;

    using Aimtec;
    using Aimtec.SDK.Menu;
    using Aimtec.SDK.Orbwalking;

    using Spell = Aimtec.SDK.Spell;

    public class Variables
    {
        public static Dictionary<string, string> InitiatorsList =
            new Dictionary<string, string>
                {
                    { "Aatrox", "Aatroxq" },
                    { "Alistar", "Headbutt" },
                    { "Fiddlesticks", "Crowstorm" },
                    { "LeeSin", "Blindmonkqtwo" },
                    { "MonkeyKing", "Monkeykingnimbus" },
                    { "Vi", "Viq" },
                    { "Thresh", "Threshqleap" }
                };

        public static Dictionary<string, string> SpecialSpells =
            new Dictionary<string, string>
                {
                    { "KogMaw", "KogMawBioArcaneBarrage" },
                    { "Twitch", "TwitchFullAutomatic" },
                    { "Tristana", "TristanaQ" }
                };

        public static Dictionary<SpellSlot, Spell> Spells =
            new Dictionary<SpellSlot, Spell>
                {
                    { SpellSlot.Q, new Spell(SpellSlot.Q) },
                    { SpellSlot.W, new Spell(SpellSlot.W) },
                    { SpellSlot.E, new Spell(SpellSlot.E) },
                    { SpellSlot.R, new Spell(SpellSlot.R) }
                };

        public static Dictionary<string, string> SpellsToBlock =
            new Dictionary<string, string>
                {
                    { "Syndra", "SyndraR" },
                    { "Tristana", "TristanaR" },
                    { "Brand", "BrandR" }
                };
                
        public static Menu Menu { get; set; }

        public static Orbwalker Orbwalker { get; set; }
    }
}