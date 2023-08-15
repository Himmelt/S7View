using S7Lib;
using System;

namespace Test {
    internal class Program {
        public static void Main(string[] args) {
            S7Plc plc = new S7Plc("192.168.10.10");
            string s1 = plc.R_DB_CString(6000, 42, 17);
            string s2 = plc.R_DB_CString(6000, 42, 58);
            Console.WriteLine(s1.Length);
            Console.WriteLine(s2.Length);
            plc.W_DB_Bits(6000, 0, 1, new bool[] { true, true }, 4);
            plc.Close();
        }
    }
}
