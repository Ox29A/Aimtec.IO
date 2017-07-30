namespace iLulu.Managers
{
    using System.Drawing;
    using System.Linq;

    using Aimtec;
    using Aimtec.SDK.Util.Cache;

    using iLulu.Utils;

    public class PixManager
    {
        public static Obj_AI_Base Pix { get; set; }

        public static void OnLoad()
        {
            Pix = GameObjects.Get<Obj_AI_Base>().FirstOrDefault(x => x.IsAlly && x.Name == "RobotBuddy");

            Game.OnUpdate += UpdatePix;
            Render.OnRender += DrawPix;
            Obj_AI_Base.OnProcessSpellCast += OnSpellCast;
        }

        private static void OnSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs e)
        {
            if (sender.IsMe && e.SpellSlot == SpellSlot.E)
            {
                // TODO update source position hur
            }
        }

        private static void UpdatePix()
        {
            if (Pix == null)
                Pix = GameObjects.Get<Obj_AI_Base>().FirstOrDefault(x => x.IsAlly && x.Name == "RobotBuddy");
        }

        private static void DrawPix()
        {
            Render.Circle(Pix.Position + new Vector3(0, 0, 15), 100, 1, Color.BlueViolet);
        }
    }
}
