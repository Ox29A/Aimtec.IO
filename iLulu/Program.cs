using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aimtec.SDK.Events;

namespace iLulu
{
    class Program
    {
        static void Main(string[] args)
        {
            var lulu = new Lulu();
            GameEvents.GameStart += lulu.OnLoad;
        }
    }
}
