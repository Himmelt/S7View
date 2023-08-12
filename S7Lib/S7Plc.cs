using S7.Net;
using System;
using System.Threading.Tasks;

namespace S7Lib {

    public class S7Plc {

        private readonly Plc plc;

        public S7Plc(string ip) : this(ip, 500) { }

        public S7Plc(string ip, short timeout) {
            plc = new Plc(CpuType.S71500, ip, 0, 0);
            plc.ReadTimeout = plc.WriteTimeout = timeout;
            Task task = plc.OpenAsync();
            task.Wait(timeout);
            if (!task.IsCompleted || task.IsFaulted) {
                throw new Exception("PLC " + ip + " 连接超时(" + timeout + "ms)");
            }
        }

        public bool IsRunning() {
            return plc.ReadStatus() == 0x08;
        }

        public bool IsConnected() {
            return plc.IsConnected;
        }

        private object R_DB_Value(short db, short start, VarType type, short count, byte bit = 0) {
            try {
                Task<object> task = plc.ReadAsync(DataType.DataBlock, db, start, type, count, bit);
                task.Wait(plc.ReadTimeout);
                if (!task.IsCompleted || task.IsFaulted) {
                    throw new Exception("PLC 读取超时：DB" + db + ".DBX" + start + "." + bit);
                }
                return task.Result;
            } catch (Exception e) {
                throw new Exception("PLC 读取错误：DB" + db + ".DBX" + start + "." + bit, e);
            }
        }

        public bool R_DB_Bit(short db, short start, byte bit) {
            return (bool)R_DB_Value(db, start, VarType.Bit, 1, bit);
        }

        public bool[] R_DB_Bits(short db, short start, short count, byte bit) {
            return (bool[])R_DB_Value(db, start, VarType.Bit, count, bit);
        }

        public byte R_DB_Byte(short db, short start) {
            return (byte)R_DB_Value(db, start, VarType.Byte, 1);
        }

        public byte[] R_DB_Bytes(short db, short start, short count) {
            return (byte[])R_DB_Value(db, start, VarType.Byte, count);
        }

        public UInt16 R_DB_Word(short db, short start) {
            return (UInt16)R_DB_Value(db, start, VarType.Word, 1);
        }

        public UInt16[] R_DB_Words(short db, short start, short count) {
            return (UInt16[])R_DB_Value(db, start, VarType.Word, count);
        }

        public UInt32 R_DB_DWord(short db, short start) {
            return (UInt32)R_DB_Value(db, start, VarType.DWord, 1);
        }

        public UInt32[] R_DB_DWords(short db, short start, short count) {
            return (UInt32[])R_DB_Value(db, start, VarType.DWord, count);
        }

        public Int16 R_DB_Int16(short db, short start) {
            return (Int16)R_DB_Value(db, start, VarType.Int, 1);
        }

        public Int16[] R_DB_Int16s(short db, short start, short count) {
            return (Int16[])R_DB_Value(db, start, VarType.Int, count);
        }

        public Int32 R_DB_Int32(short db, short start) {
            return (Int32)R_DB_Value(db, start, VarType.DInt, 1);
        }

        public Int32[] R_DB_Int32s(short db, short start, short count) {
            return (Int32[])R_DB_Value(db, start, VarType.DInt, count);
        }

        public float R_DB_Float(short db, short start) {
            return (float)R_DB_Value(db, start, VarType.Real, 1);
        }

        public float[] R_DB_Floats(short db, short start, short count) {
            return (float[])R_DB_Value(db, start, VarType.Real, count);
        }

        public double R_DB_Double(short db, short start) {
            return (double)R_DB_Value(db, start, VarType.LReal, 1);
        }

        public double[] R_DB_Doubles(short db, short start, short count) {
            return (double[])R_DB_Value(db, start, VarType.LReal, count);
        }

        public string R_DB_CString(short db, short start, short size) {
            return (string)R_DB_Value(db, start, VarType.String, size);
        }

        public string R_DB_S7String(short db, short start, short size) {
            return (string)R_DB_Value(db, start, VarType.S7String, size);
        }

        public string R_DB_S7WString(short db, short start, short size) {
            return (string)R_DB_Value(db, start, VarType.S7WString, size);
        }
    }
}
