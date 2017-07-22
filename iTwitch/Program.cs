using Aimtec.SDK.Events;

namespace iTwitch
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var twitch = new Twitch();
            GameEvents.GameStart += twitch.OnGameLoad;
        }
    }
}