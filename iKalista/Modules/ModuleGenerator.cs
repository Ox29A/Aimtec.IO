using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aimtec.SDK.Menu;
using Aimtec.SDK.Menu.Components;

namespace iKalista.Modules
{
    class ModuleGenerator
    {

        public static void GenerateMenuForModules(List<IModule> modules, Menu rootMenu)
        {
            var modulesMenu = new Menu("com.ikalista.modules", "Modules");
            foreach (var module in modules)
            {
                modulesMenu.Add(new MenuBool(module.GetName(), module.GetName()));
            }

            rootMenu.Add(modulesMenu);
        }

    }
}
