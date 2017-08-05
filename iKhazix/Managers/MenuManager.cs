namespace iKhazix.Managers
{
    using Aimtec.SDK.Menu;
    using Aimtec.SDK.Menu.Components;
    using Aimtec.SDK.Orbwalking;

    using iLulu.Utils;

    public class MenuManager
    {
        public static void Initialize()
        {
            // TODO 
            Variables.Menu = new Menu("iKhazix", "iKhazix", true);

            Variables.Orbwalker = new Orbwalker();
            Variables.Orbwalker.Attach(Variables.Menu);

            var comboMenu = new Menu("combo", ">> Combo Options");
            {
                comboMenu.Add(new MenuBool("useQ", ">> Use Q in combo"));
                comboMenu.Add(new MenuBool("useW", ">> Use W in combo"));
                comboMenu.Add(new MenuBool("useE", ">> Use E in combo"));
                comboMenu.Add(new MenuBool("useR", ">> Use R in combo"));
                Variables.Menu.Add(comboMenu);
            }

            var harassMenu = new Menu("harass", ">> Harass Options");
            {
                harassMenu.Add(new MenuBool("useQ", ">> Use Q in harass"));
                harassMenu.Add(new MenuBool("useW", ">> Use W in harass"));
                Variables.Menu.Add(harassMenu);
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
