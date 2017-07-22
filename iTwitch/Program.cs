using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aimtec;
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
