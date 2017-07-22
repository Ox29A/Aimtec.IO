using System;
using Aimtec;
using Aimtec.SDK.Menu;
using Aimtec.SDK.Menu.Components;
using Aimtec.SDK.Orbwalking;
using Aimtec.SDK.Prediction.Skillshots;
using Aimtec.SDK.Util;
using iTwitch.Module_System;
using iTwitch.Utils;

namespace iTwitch
{
    internal class Twitch
    {
        private ModuleManager _moduleManager;

        #region Events

        private void OnDraw(EventArgs args)
        {
        }

        #endregion

        #region Public Methods and Operators

        public void OnGameLoad()
        {
            if (ObjectManager.GetLocalPlayer().ChampionName != "Twitch")
                return;

            //TODO dmg indicator
            LoadSpells();
            LoadMenu();

            //Game.OnTick += OnUpdate;
            //Drawing.OnDraw += OnDraw;
            _moduleManager = new ModuleManager();
            _moduleManager.OnLoad();
        }

        private void LoadMenu()
        {
            Variables.Menu = new Menu("com.itwitch", "iTwitch", true);

            Variables.Orbwalker = new Orbwalker();
            Variables.Orbwalker.Attach(Variables.Menu);

            var comboMenu = new Menu("com.itwitch.combo", "Combo Options");
            {
                comboMenu.Add(new MenuBool("useW", "Use W"));
                comboMenu.Add(new MenuBool("useE", "Use E"));
                comboMenu.Add(new MenuSlider("eSlider", "Stacks to Cast", 0, 0, 6));
                Variables.Menu.Add(comboMenu);
            }

            var miscMenu = new Menu("com.itwitch.misc", "Misc Options");
            {
                miscMenu.Add(new MenuBool("ebeforedeath", "E Before Death"));
                miscMenu.Add(new MenuBool("autoYo", "Youmuus with R"));
                miscMenu.Add(new MenuBool("noWTurret", "No W Under Turrets"));
                miscMenu.Add(new MenuSlider("noWAA", "No W if x AA can kill", 0, 0, 10));
                miscMenu.Add(new MenuBool("noWR", " No W while R Active", false));
                miscMenu.Add(new MenuKeyBind("stealthrecall", "Stealth Recall", KeyCode.T, KeybindType.Press));
                Variables.Menu.Add(miscMenu);
            }

            Variables.Menu.Attach();
        }

        private void LoadSpells()
        {
            Variables.Spells[SpellSlot.W].SetSkillshot(0.25f, 120f, 1400f, false, SkillshotType.Circle);
        }

        #endregion
    }
}