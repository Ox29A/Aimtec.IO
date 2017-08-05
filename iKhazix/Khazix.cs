namespace iKhazix
{
    using iKhazix.Managers;

    public class Khazix
    {

        public static void OnLoad()
        {
            MenuManager.Initialize();
            ModuleManager.OnLoad();
            SetSkillshots();
        }

        private static void SetSkillshots()
        {
            // TODO set skillshots?
        }
    }
}
