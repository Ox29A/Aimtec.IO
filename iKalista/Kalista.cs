using Aimtec;
using Aimtec.SDK.Menu;
using Aimtec.SDK.Menu.Components;
using Aimtec.SDK.Orbwalking;
using Aimtec.SDK.Prediction.Skillshots;
using iKalista.Modules;
using iKalista.Utils;

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
                var qSettings = new Menu("com.ikalista.combo.q", ">> Q Settings");
                {
                    qSettings.Add(new MenuBool("useQ", "Use Q"));
                    qSettings.Add(new MenuSeperator("sep")); //--
                    comboMenu.Add(qSettings);
                }

                //TODO 

                var eSettings = new Menu("com.ikalista.combo.e", ">> E Settings");
                {
                    eSettings.Add(new MenuBool("useE", "Use E"));
                    eSettings.Add(new MenuBool("useELeaving", "Use E when target leaving range"));
                    eSettings.Add(new MenuSlider("eLeavingPercent", "Min Percent to cast E Leaving", 50, 10));
                    eSettings.Add(new MenuBool("autoEMinChamp", "Auto E Minion > Champion"));
                    comboMenu.Add(eSettings);
                }

                var ultSettings = new Menu("com.ikalista.combo.r", ">> Soulbound Settings");
                {
                    ultSettings.Add(new MenuBool("useR", "Save Ally with R"));
                    ultSettings.Add(new MenuSlider("allyPercent", "Percent Health for ally to save", 20, 5));
                    comboMenu.Add(ultSettings);
                }
                Variables.Menu.Add(comboMenu);
            }
            var harassMenu = new Menu("com.ikalista.harass", ">> Harass Options");
            {
            }

            var miscMenu = new Menu("com.ikalista.misc", ">> Misc Options");
            {
                miscMenu.Add(new MenuBool("forceW", "Focus Targets With Passive Buff"));
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