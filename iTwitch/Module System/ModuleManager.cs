using System.Collections.Generic;
using System.Linq;
using Aimtec;
using iTwitch.Module_System.Enumerations;
using iTwitch.Module_System.Misc;
using iTwitch.Module_System.Spell_Casting;

namespace iTwitch.Module_System
{
    internal class ModuleManager
    {
        private readonly IOrderedEnumerable<IModule> _moduleList =
            new List<IModule> {new ThrowCasketModule(), new ExpungeModule(), new StealthRecall()}
                .OrderByDescending(x => x.GetPriority());

        public void OnLoad()
        {
            foreach (var module in _moduleList)
                module.OnLoad();

            Game.OnUpdate += OnUpdate;
        }

        private void OnUpdate()
        {
            foreach (var module in _moduleList.Where(x => x.CanExecute() &&
                                                          x.GetModuleType().HasFlag(ModuleType.OnUpdate)))
                module.Execute();
        }
    }
}