namespace iLulu.Modules.Combo
{
    using System;
    using System.Linq;

    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Util.Cache;

    using iLulu.Interfaces;
    using iLulu.Utils;

    internal class CastRModule : IUpdateModule
    {
        public void OnLoad()
        {
            Console.WriteLine("Cast R Module Loaded");
        }

        public string GetName() => "Cast R Module";

        public string GetDescription() => "Cast R on allies / self";

        public bool CanExecute() => Variables.Spells[SpellSlot.R].Ready && Variables.Menu["combo"]["r"]["useR"].Enabled;

        public void Execute()
        {
            var ally = GameObjects.AllyHeroes.FirstOrDefault(x => x != null);

            if (ally == null || ObjectManager.GetLocalPlayer().Distance(ally) < Variables.Spells[SpellSlot.R].Range)
                return;

            if (ally.CountEnemyHeroesInRange(300) >= Variables.Menu["combo"]["r"]["rAmount"].Value)
                Variables.Spells[SpellSlot.R].CastOnUnit(ally);
        }
    }
}