using S7Lib;
using System;

namespace Test {
    internal class Program {
        public static void Main(string[] args) {
            S7Plc plc = new S7Plc(ip: "192.168.0.10", cpuType: 201);
            bool s1 = plc.R_DB_Bit(1, 1, 0);
            Console.WriteLine(s1);
            plc.W_DB_Bits(1, 8, 1, new bool[] { true, true }, 4);
            plc.Close();
        }
    }
}
