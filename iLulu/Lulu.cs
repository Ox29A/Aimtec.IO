namespace iLulu
{
    using System;
    using System.Linq;

    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Prediction.Skillshots;
    using Aimtec.SDK.Util.Cache;

    using iLulu.Managers;
    using iLulu.Utils;

    using ZLib;
    using ZLib.Base;
    using ZLib.Handlers;

    public class Lulu
    {
        private Obj_AI_Hero Player { get; set; }

        public void OnLoad()
        {
            Player = ObjectManager.GetLocalPlayer();
            
            MenuManager.Load();
            ModuleManager.OnLoad();
            SetSkillshots();
            PixManager.OnLoad();

            Game.OnUpdate += OnUpdate;
            Render.OnRender += OnRender;

            ZLib.OnPredictDamage += ZLibOnOnPredictDamage;
        }

        private static void ZLibOnOnPredictDamage(Unit unit, PredictDamageEventArgs args)
        {
            if (Variables.Spells[SpellSlot.E].Ready && unit.Instance.IsAlly && unit.Instance.Distance(ObjectManager.GetLocalPlayer()) <= Variables.Spells[SpellSlot.E].Range)
            {
                var damage = args.HpInstance.PredictedDmg;

                if (damage >= unit.Instance.Health && unit.Events.Contains(EventType.Danger) 
                    || unit.Events.Contains(EventType.Ultimate))
                {
                    Variables.Spells[SpellSlot.E].CastOnUnit(unit.Instance);
                    Console.WriteLine(damage);
                }
            }
        }

        private void SetSkillshots()
        {
            Variables.Spells[SpellSlot.Q].SetSkillshot(0.25f, 60, 1450, false, SkillshotType.Line);
        }

        private void OnUpdate()
        {
            /*if (Variables.Menu["speedy"]["key"].Enabled)
            {
                var target = GameObjects.AllyHeroes
                    .OrderByDescending(h => Variables.Menu["speedy"][h.ChampionName + "WEPriority"].Value)
                    .FirstOrDefault();

                if (target != null)
                {
                    if (Variables.Spells[SpellSlot.W].Ready)
                    {
                        Variables.Spells[SpellSlot.W].CastOnUnit(target);
                    }
                    if (Variables.Spells[SpellSlot.E].Ready)
                    {
                        Variables.Spells[SpellSlot.E].CastOnUnit(target);
                    }
                }
                else
                {
                    Console.WriteLine("TARGET IS SHIT");
                }
            }*/
        }

        private void OnRender()
        {

        }
    }
}
