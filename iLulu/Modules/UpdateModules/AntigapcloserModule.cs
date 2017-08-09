namespace iLulu.Modules.UpdateModules
{
    #region Imports

    using System;
    using System.Linq;

    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Util.Cache;

    using iLulu.Interfaces;
    using iLulu.Utils;

    #endregion

    public class AntigapcloserModule : IEventModule<Obj_AI_Hero, GapcloserArgs>
    {
        public bool CanExecute()
        {
            return Variables.Spells[SpellSlot.E].Ready;
        }

        public void Execute(Obj_AI_Hero target, GapcloserArgs args)
        {
            if (target != null && target.IsValidTarget(Variables.Spells[SpellSlot.W].Range))
            {
                switch (args.Type)
                {
                    case SpellType.Dash:
                    case SpellType.SkillShot:
                    case SpellType.Targeted:
                    case SpellType.Melee:
                        if (args.HaveShield)
                            return;

                        if (GameObjects.AllyHeroes.Any(
                            x => x.Distance(args.EndPosition) <= 400 && x.Distance(ObjectManager.GetLocalPlayer())
                                 <= Variables.Spells[SpellSlot.W].Range))
                        {
                            Variables.Spells[SpellSlot.W].CastOnUnit(target);
                        }

                        break;
                }
            }
        }

        public string GetDescription()
        {
            return "DOES ANTI GAPCLOSER";
        }

        public string GetName()
        {
            return "Anti Gapcloser";
        }

        public void OnLoad()
        {
            Console.WriteLine("Gapcloser loaded");
        }
    }
}