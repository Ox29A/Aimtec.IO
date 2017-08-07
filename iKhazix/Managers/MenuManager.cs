using Aimtec.SDK.Prediction.Skillshots;
using Aimtec.SDK.Util;
using iKhazix.Utils;

namespace iKhazix.Managers
{
    using Aimtec.SDK.Menu;
    using Aimtec.SDK.Menu.Components;
    using Aimtec.SDK.Orbwalking;

    public class MenuManager
    {
        public static void Initialize()
        {
            // TODO 
            Variables.Menu = new Menu("iKhazix", "iKhazix", true);

            Variables.Orbwalker = new Orbwalker();
            Variables.Orbwalker.Attach(Variables.Menu);

            var assasinationMenu = new Menu("assMenu", "Assasination");
            {
                assasinationMenu.Add(new MenuKeyBind("assasinate", "Assasination Key", KeyCode.T, KeybindType.Press));
                assasinationMenu.Add(new MenuList("assMode", "Return Point", new[] {"Old Position", "Mouse Position"}, 0));
            }

            var comboMenu = new Menu("combo", ">> Combo Options");
            {
                comboMenu.Add(new MenuBool("useQ", ">> Use Q in combo"));
                comboMenu.Add(new MenuBool("useW", ">> Use W in combo"));
                comboMenu.Add(
                    new MenuList("wHitchance", ">> W Hitchance", new[] {"Low", "Medium", "High", "Very High"}, 1));
                comboMenu.Add(new MenuBool("useE", ">> Use E in combo"));
                comboMenu.Add(new MenuList("jumpMode", ">> Jump Mode", new[] {"Current Position", "Prediction"}, 0));
                comboMenu.Add(new MenuBool("useEGapcloseQ", ">> Use E to gapclose for Q", false));
                comboMenu.Add(new MenuBool("useEGapcloseW", ">> Use E to gapclose for W", false));
                comboMenu.Add(new MenuBool("useRLongGap", ">> Use R after long gapcloses", false));
                Variables.Menu.Add(comboMenu);
            }

            var harassMenu = new Menu("harass", ">> Harass Options");
            {
                harassMenu.Add(new MenuBool("useQ", ">> Use Q in harass"));
                harassMenu.Add(new MenuBool("useW", ">> Use W in harass"));
                Variables.Menu.Add(harassMenu);
            }
            
            var doubleJump = new Menu("doubleJump", ">> Double Jump Options");
            {
                doubleJump.Add(new MenuBool("enabled", ">> Enabled"));
                doubleJump.Add(new MenuSlider("EDelay", ">> Delay Between Jumps", 250, 250, 500));
                doubleJump.Add(new MenuList("jumpMode", ">> Jump Mode",
                    new[] {"Default (towards nexus)", "Custom - Settings below"}, 0));
                doubleJump.Add(new MenuBool("save", ">> Save Double Jump Abilities", false));
                doubleJump.Add(new MenuBool("noaa", ">> Wait for Q instead of auto attacks", false));
                doubleJump.Add(new MenuBool("jCursor", ">> Jump to cursor (true) or false for script logic"));
                doubleJump.Add(new MenuBool("secondJump", ">> Do Second Jump!"));
                doubleJump.Add(new MenuBool("jCursor2", ">> Second Jump to Cursor (true) or false for script logic"));
            }

            var safetyMenu = new Menu("safety", ">> Safety Options");
            {
                safetyMenu.Add(new MenuBool("enabled", ">> Enabled"));
                safetyMenu.Add(new MenuBool("autoEscape", ">> Use E to flee when LOW HP"));
                safetyMenu.Add(new MenuBool("countCheck", ">> Min Ally ratio to enemies to jump"));
                safetyMenu.Add(new MenuSlider("ratio", ">> Ally:Enemy Ratio (/5)", 2, 0, 5));
                safetyMenu.Add(new MenuBool("turretCheck", ">> Avoid Turrets"));
                safetyMenu.Add(new MenuSlider("minHealth", ">> Health %", 35));
                safetyMenu.Add(new MenuBool("noAutoStealth", ">> No Auto Attacks while stealthed"));
                Variables.Menu.Add(safetyMenu);
            }

            // TODO laneclear
            var miscMenu = new Menu("misc", ">> Misc Options");
            {
                // TODO options?
            }

            Variables.Menu.Attach();
        }
    }
}
