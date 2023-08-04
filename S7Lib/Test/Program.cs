using S7Lib;
using System;

namespace Test
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            S7API.Open(0, "192.168.0.1", 40, 0, 0);
            Console.WriteLine(S7API.IsRunning(0));
        }
    }
}
