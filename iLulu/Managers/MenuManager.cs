namespace iLulu.Managers
{
    using System.Linq;

    using Aimtec.SDK.Menu;
    using Aimtec.SDK.Menu.Components;
    using Aimtec.SDK.Orbwalking;
    using Aimtec.SDK.Util;
    using Aimtec.SDK.Util.Cache;

    using iLulu.Utils;

    public class MenuManager
    {

        public static void Load()
        {
            Variables.Menu = new Menu("com.ilulu", "iLulu - The Pixy Lickur", true);

            Variables.Orbwalker = new Orbwalker();
            Variables.Orbwalker.Attach(Variables.Menu);

            ZLib.ZLib.Attach(Variables.Menu);

            var comboSettings = new Menu("combo", ">> Combo Options");
            {
                var qSettings = new Menu("q", ">> Q Settings");
                {
                    qSettings.Add(new MenuBool("useQ", "Use Q"));
                    qSettings.Add(new MenuBool("useQImpaired", "Use Q on imparied targets"));
                    comboSettings.Add(qSettings);
                }

                var wMenu = new Menu("w", ">> W Settings");
                {
                    wMenu.Add(new MenuBool("useW", "Use W on Enemies"));
                    var polymorphMenu = new Menu("poly", ">> Polymorph Settings");
                    {
                        foreach (var hero in GameObjects.EnemyHeroes)
                        {
                            polymorphMenu.Add(new MenuSlider(hero.ChampionName + "WPriority", hero.ChampionName, 1, 0, 5));
                        }
                        wMenu.Add(polymorphMenu);
                    }
                    comboSettings.Add(wMenu);
                }

                var eSettings = new Menu("e", ">> E Settings");
                {
                    eSettings.Add(new MenuBool("useE", "Use E on enemies", false));
                    eSettings.Add(new MenuBool("eq", "Use E-Q when no allies around"));
                    eSettings.Add(new MenuSeperator("1123", "Will only use E on enemies if no allies around"));
                    comboSettings.Add(eSettings);
                }

                var rSettings = new Menu("r", ">> R Settings");
                {
                    rSettings.Add(new MenuBool("useR", "Use R"));
                    rSettings.Add(new MenuSlider("rAmount", "Auto Ult x enemies (0 to disable)", 3, 0, 5));
                    comboSettings.Add(rSettings);
                }
                Variables.Menu.Add(comboSettings);
            }

            var autoShieldMenu = new Menu("autoShield", ">> Shield Settings");
            {
                autoShieldMenu.Add(new MenuBool("useE", "Use E"));
                var priorityMenu = new Menu("prior", ">> Priority Settings");
                {
                    foreach (var hero in GameObjects.AllyHeroes)
                    {
                        priorityMenu.Add(
                            new MenuSlider(hero.ChampionName + "EPriority", hero.ChampionName + " Min Health %", 20));
                    }
                }
                autoShieldMenu.Add(priorityMenu);
                Variables.Menu.Add(autoShieldMenu);
            }

            var speedyMenu = new Menu("speedy", ">> Speedy Gonzales");
            {
                foreach (var ally in GameObjects.AllyHeroes.Where(x => !x.IsMe))
                {
                    speedyMenu.Add(
                        new MenuSlider(ally.ChampionName + "WEPriority", ally.ChampionName + " Priority", 1, 0, 5));
                }
                speedyMenu.Add(new MenuSeperator("$"));
                speedyMenu.Add(new MenuKeyBind("key", "Key", KeyCode.Z, KeybindType.Press));
                Variables.Menu.Add(speedyMenu);
            }

            var miscMenu = new Menu("misc", ">> Misc Options");
            {
                var specialSpells = new Menu("spec", ">> Special Spells to cast W");
                {
                    foreach (var hero in GameObjects.AllyHeroes.Where(x => !x.IsMe))
                    {
                        foreach (var spell in Variables.SpecialSpells)
                        {
                            if (hero.ChampionName != spell.Key) continue;

                            var champMenu = new Menu(hero.ChampionName, hero.ChampionName);
                            {
                                champMenu.Add(new MenuBool(spell.Value, spell.Value));
                            }
                            specialSpells.Add(champMenu);
                        }
                        miscMenu.Add(specialSpells);
                    }
                }
                var initiatorsList = new Menu("init", ">> Initiators to speed up");
                {
                    foreach (var hero in GameObjects.AllyHeroes.Where(x => !x.IsMe))
                    {
                        foreach (var spell in Variables.InitiatorsList)
                        {
                            if (hero.ChampionName != spell.Key) continue;

                            var champMenu = new Menu(hero.ChampionName, hero.ChampionName);
                            {
                                champMenu.Add(new MenuBool(spell.Value, spell.Value));
                            }
                            initiatorsList.Add(champMenu);
                        }
                        miscMenu.Add(initiatorsList);
                    }
                }
                Variables.Menu.Add(miscMenu);
            }

            Gapcloser.Attach(Variables.Menu, ">> Gapcloser Options");

            /*var evadeMenu = new Menu("evade", ">> Evade Menu");
            {
                EvadeManager.Attach(evadeMenu);
                Variables.Menu.Add(evadeMenu);
            }*/

            Variables.Menu.Attach();
        }

    }
}
