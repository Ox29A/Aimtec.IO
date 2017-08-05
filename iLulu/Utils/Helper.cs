namespace iLulu.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;

    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Util.Cache;

    public static class Helper
    {

        private static readonly string[] Attacks =
            {
                "caitlynheadshotmissile", "frostarrow", "garenslash2",
                "kennenmegaproc", "masteryidoublestrike", "quinnwenhanced",
                "renektonexecute", "renektonsuperexecute",
                "rengarnewpassivebuffdash", "trundleq", "xenzhaothrust",
                "xenzhaothrust2", "xenzhaothrust3", "viktorqbuff",
                "lucianpassiveshot"
            };

        private static readonly string[] NoAttacks =
            {
                "volleyattack", "volleyattackwithsound",
                "jarvanivcataclysmattack", "monkeykingdoubleattack",
                "shyvanadoubleattack", "shyvanadoubleattackdragon",
                "zyragraspingplantattack", "zyragraspingplantattack2",
                "zyragraspingplantattackfire", "zyragraspingplantattack2fire",
                "viktorpowertransfer", "sivirwattackbounce", "asheqattacknoonhit",
                "elisespiderlingbasicattack", "heimertyellowbasicattack",
                "heimertyellowbasicattack2", "heimertbluebasicattack",
                "annietibbersbasicattack", "annietibbersbasicattack2",
                "yorickdecayedghoulbasicattack", "yorickravenousghoulbasicattack",
                "yorickspectralghoulbasicattack", "malzaharvoidlingbasicattack",
                "malzaharvoidlingbasicattack2", "malzaharvoidlingbasicattack3",
                "kindredwolfbasicattack", "gravesautoattackrecoil"
            };

        public static Obj_AI_Hero GetBestWTarget()
        {
            var enemy = GameObjects.EnemyHeroes
                .Where(h => h.IsValidTarget(Variables.Spells[SpellSlot.W].Range))
                .MaxBy(o => Variables.Menu[o.ChampionName + "WPriority"].Value);

            return enemy == null || Variables.Menu[enemy.ChampionName + "WPriority"].Value == 0 ? null : enemy;
        }

        public static bool IsAutoAttack(this SpellData spellData)
        {
            return IsAutoAttack(spellData.Name);
        }

        public static bool IsAutoAttack(string name)
        {
            return name.ToLower().Contains("attack") && !NoAttacks.Contains(name.ToLower())
                   || Attacks.Contains(name.ToLower());
        }

        public static void DrawLineInWorld(Vector3 start, Vector3 end, int width, Color color)
        {
            Vector2 screenOutStart, screenOutEnd; 
            Render.WorldToScreen(start, out screenOutStart);
            Render.WorldToScreen(end, out screenOutEnd);

            // Render.Line(screenOutStart., from[1], to[0], to[1], width, color);
            Render.Line(screenOutStart.X, screenOutStart.Y, screenOutEnd.X, screenOutEnd.Y, color);

            // Drawing.DrawLine(from.X, from.Y, to.X, to.Y, width, color);
        }

        /// <summary>
        ///     Searches for the max or default element.
        /// </summary>
        /// <typeparam name="T">
        ///     The type.
        /// </typeparam>
        /// <typeparam name="TR">
        ///     The comparing type.
        /// </typeparam>
        /// <param name="container">
        ///     The container.
        /// </param>
        /// <param name="valuingFoo">
        ///     The comparing function.
        /// </param>
        /// <returns></returns>
        public static T MaxOrDefault<T, TR>(this IEnumerable<T> container, Func<T, TR> valuingFoo)
            where TR : IComparable
        {
            var enumerator = container.GetEnumerator();
            if (!enumerator.MoveNext())
            {
                return default(T);
            }

            var maxElem = enumerator.Current;
            var maxVal = valuingFoo(maxElem);

            while (enumerator.MoveNext())
            {
                var currVal = valuingFoo(enumerator.Current);

                if (currVal.CompareTo(maxVal) > 0)
                {
                    maxVal = currVal;
                    maxElem = enumerator.Current;
                }
            }

            return maxElem;
        }

        public static bool IsMovementImpaired(this Obj_AI_Hero hero)
        {
            return hero.HasBuffOfType(BuffType.Flee) || hero.HasBuffOfType(BuffType.Charm)
                   || hero.HasBuffOfType(BuffType.Slow) || hero.HasBuffOfType(BuffType.Snare)
                   || hero.HasBuffOfType(BuffType.Stun) || hero.HasBuffOfType(BuffType.Taunt);
        }

        public static Obj_AI_Hero GetBestWeTarget()
        {
            var ally =
                GameObjects.AllyHeroes
                    .OrderByDescending(h => Variables.Menu["speedy"][h.ChampionName + "WEPriority"].Value)
                    .FirstOrDefault();

            return ally == null || Variables.Menu["speedy"][ally.ChampionName + "WEPriority"].Value == 0
                       ? null
                       : ally;
        }
    }
}