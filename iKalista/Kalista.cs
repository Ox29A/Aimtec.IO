using System.Linq;
using Aimtec;
using Aimtec.SDK.Menu;
using Aimtec.SDK.Menu.Components;
using Aimtec.SDK.Orbwalking;
using Aimtec.SDK.Prediction.Skillshots;
using iKalista.Modules;
using iKalista.Utils;
using ZLib.Base;

namespace iKalista
{
    internal class Kalista
    {
        #region Public Methods and Operators

        public void OnGameLoad()
        {
            if (ObjectManager.GetLocalPlayer().ChampionName != "Kalista")
                return;

            //TODO dmg indicator
            Variables.Orbwalker = new Orbwalker();

            LoadSpells();
            LoadMenu();

            ModuleMemeger.Initialize();
        }

        private void LoadMenu()
        {
            Variables.Menu = new Menu("com.ikalista", "iKalista", true);

            Variables.Orbwalker.Attach(Variables.Menu);

            var comboMenu = new Menu("com.ikalista.combo", ">> Combo Options");
            {
                comboMenu.Add(new MenuBool("useQ", "Use Q"));
                comboMenu.Add(new MenuSeperator("sep")); //--                

                comboMenu.Add(new MenuBool("useE", "Use E"));
                comboMenu.Add(new MenuSlider("eStacks", "Min Percent to cast E Leaving", 10, 0, 25));

                comboMenu.Add(new MenuBool("useELeaving", "Use E when target leaving range"));
                comboMenu.Add(new MenuSlider("eLeavingPercent", "Min Percent to cast E Leaving", 50, 10));
                comboMenu.Add(new MenuBool("autoEMinChamp", "Auto E Minion > Champion"));
                comboMenu.Add(new MenuSeperator("sep2"));

                comboMenu.Add(new MenuBool("useR", "Save Ally with R"));
                comboMenu.Add(new MenuSlider("allyPercent", "Percent Health for ally to save", 20, 5));

                /*var dangerSpells = new Menu("dangerSpells", "Dangerous Spells");
                {
                    foreach (var spell in ZLib.ZLib.CachedSpells.Where(x => x.EventTypes.Contains(EventType.Danger)))
                    {
                        dangerSpells.Add(new MenuBool(spell.SpellName, spell.ChampionName + " (" + spell.Slot + ")"));
                    }
                    comboMenu.Add(dangerSpells);
                }*/

                Variables.Menu.Add(comboMenu);
            }

            var jungleMenu = new Menu("com.ikalista.jungle", ">> Jungle Steal Settings");
            {
                jungleMenu.Add(new MenuBool("enabled", "Use Jungle Steal"));
                jungleMenu.Add(new MenuBool("small", "Steal Small Minions"));
                jungleMenu.Add(new MenuBool("large", "Steal Large Minions"));
                jungleMenu.Add(new MenuBool("legendary", "Steal Legendary Minions"));
                Variables.Menu.Add(jungleMenu);
            }

            var harassMenu = new Menu("com.ikalista.harass", ">> Harass Options");
            {
            }

            var miscMenu = new Menu("com.ikalista.misc", ">> Misc Options");
            {
                miscMenu.Add(new MenuBool("forceW", "Focus Targets With Passive Buff"));
                miscMenu.Add(new MenuBool("autoEUnkillable", "Auto E unkillable minions"));
                Variables.Menu.Add(miscMenu);
            }

            Variables.Menu.Attach();
        }

        private void LoadSpells()
        {
            Variables.Spells[SpellSlot.Q].SetSkillshot(0.25f, 40f, 1200f, true, SkillshotType.Line);
            Variables.Spells[SpellSlot.R].SetSkillshot(0.50f, 1500f, float.MaxValue, false, SkillshotType.Circle);
        }

        #endregion
    }
}