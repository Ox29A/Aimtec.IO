namespace iKhazix
{
    using System;

    using Aimtec.SDK.Events;

    public class Program
    {
        public static void Main(string[] args)
        {
            GameEvents.GameStart += Khazix.OnLoad;
        }

    }
}
