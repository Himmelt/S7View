﻿using S7.Net;
using System;
using System.Threading.Tasks;

namespace S7Lib {

    public class S7Plc {

        private readonly Plc plc;

        public S7Plc(string ip) : this(ip, 200, 1500, 0, 0) { }

        public S7Plc(string ip, ushort timeout = 200) : this(ip, timeout, 1500, 0, 0) { }

        public S7Plc(string ip, ushort timeout = 200, ushort cpuType = 1500, byte rack = 0, byte slot = 0) {
            if (timeout <= 0) timeout = 200;
            CpuType _type;
            switch (cpuType) {
                case 200:
                    _type = CpuType.S7200; break;
                case 300:
                    _type = CpuType.S7300; break;
                case 400:
                    _type = CpuType.S7400; break;
                case 1200:
                    _type = CpuType.S71200; break;
                default:
                    _type = CpuType.S71500; break;
            }
            plc = new Plc(_type, ip, rack, slot);
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

        private string GetAddress(DataType dataType, ushort db, ushort start) {
            switch (dataType) {
                case DataType.Input:
                    return "EB" + start;
                case DataType.Output:
                    return "AB" + start;
                case DataType.Memory:
                    return "MB" + start;
                case DataType.DataBlock:
                    return "DB" + db + ".DBB" + start;
                case DataType.Counter:
                    return "Z" + start;
                case DataType.Timer:
                    return "T" + start;
                default:
                    return dataType.ToString() + db + "." + start;
            }
        }

        private object ReadValue(DataType dataType, ushort db, ushort start, VarType type, ushort count = 1) {
            try {
                Task<object> task = plc.ReadAsync(dataType, db, start, type, count);
                task.Wait(plc.ReadTimeout);
                if (!task.IsCompleted || task.IsFaulted) {
                    throw new Exception("PLC 读取超时：" + GetAddress(dataType, db, start));
                }
                return task.Result;
            } catch (Exception e) {
                throw new Exception("PLC 读取错误：" + GetAddress(dataType, db, start), e);
            }
        }

        private object R_E_Value(ushort start, VarType type, ushort count = 1) {
            return ReadValue(DataType.Input, 0, start, type, count);
        }

        public bool R_E_Bit(ushort start, byte bit) {
            byte data = R_E_Byte((ushort)(start + bit / 8));
            return data.SelectBit(bit % 8);
        }

        public bool[] R_E_Bits(ushort start, byte bit, ushort count) {
            if (count <= 0) return new bool[0];
            int _bit = bit % 8;
            int _offset = bit / 8;
            int size = (count + _bit + 7) / 8;
            byte[] bytes = R_E_Bytes((ushort)(start + _offset), (ushort)size);
            bool[] result = new bool[count];
            for (int i = 0; i < count; i++) {
                int index = (i + _bit) / 8;
                result[i] = bytes[index].SelectBit((i + _bit) % 8);
            }
            return result;
        }

        public byte R_E_Byte(ushort start) {
            return (byte)R_E_Value(start, VarType.Byte);
        }

        public byte[] R_E_Bytes(ushort start, ushort count) {
            if (count <= 0) return new byte[0];
            object obj = R_E_Value(start, VarType.Byte, count);
            return count > 1 ? (byte[])obj : new byte[] { (byte)obj };
        }

        public UInt16 R_E_Word(ushort start) {
            return (UInt16)R_E_Value(start, VarType.Word);
        }

        public UInt16[] R_E_Words(ushort start, ushort count) {
            if (count <= 0) return new UInt16[0];
            object obj = R_E_Value(start, VarType.Word, count);
            return count > 1 ? (UInt16[])obj : new UInt16[] { (UInt16)obj };
        }

        public UInt32 R_E_DWord(ushort start) {
            return (UInt32)R_E_Value(start, VarType.DWord);
        }

        public UInt32[] R_E_DWords(ushort start, ushort count) {
            return (UInt32[])R_E_Value(start, VarType.DWord, count);
        }

        public Int16 R_E_Int16(ushort start) {
            return (Int16)R_E_Value(start, VarType.Int);
        }

        public Int16[] R_E_Int16s(ushort start, ushort count) {
            return (Int16[])R_E_Value(start, VarType.Int, count);
        }

        public Int32 R_E_Int32(ushort start) {
            return (Int32)R_E_Value(start, VarType.DInt);
        }

        public Int32[] R_E_Int32s(ushort start, ushort count) {
            return (Int32[])R_E_Value(start, VarType.DInt, count);
        }

        public float R_E_Float(ushort start) {
            return (float)R_E_Value(start, VarType.Real);
        }

        public float[] R_E_Floats(ushort start, ushort count) {
            return (float[])R_E_Value(start, VarType.Real, count);
        }

        public double R_E_Double(ushort start) {
            return (double)R_E_Value(start, VarType.LReal);
        }

        public double[] R_E_Doubles(ushort start, ushort count) {
            return (double[])R_E_Value(start, VarType.LReal, count);
        }

        private object R_A_Value(ushort start, VarType type, ushort count = 1) {
            return ReadValue(DataType.Output, 0, start, type, count);
        }

        public bool R_A_Bit(ushort start, byte bit) {
            byte data = R_A_Byte((ushort)(start + bit / 8));
            return data.SelectBit(bit % 8);
        }

        public bool[] R_A_Bits(ushort start, byte bit, ushort count) {
            if (count <= 0) return new bool[0];
            int _bit = bit % 8;
            int _offset = bit / 8;
            int size = (count + _bit + 7) / 8;
            byte[] bytes = R_A_Bytes((ushort)(start + _offset), (ushort)size);
            bool[] result = new bool[count];
            for (int i = 0; i < count; i++) {
                int index = (i + _bit) / 8;
                result[i] = bytes[index].SelectBit((i + _bit) % 8);
            }
            return result;
        }

        public byte R_A_Byte(ushort start) {
            return (byte)R_A_Value(start, VarType.Byte);
        }

        public byte[] R_A_Bytes(ushort start, ushort count) {
            if (count <= 0) return new byte[0];
            object obj = R_A_Value(start, VarType.Byte, count);
            return count > 1 ? (byte[])obj : new byte[] { (byte)obj };
        }

        public UInt16 R_A_Word(ushort start) {
            return (UInt16)R_A_Value(start, VarType.Word);
        }

        public UInt16[] R_A_Words(ushort start, ushort count) {
            if (count <= 0) return new UInt16[0];
            object obj = R_A_Value(start, VarType.Word, count);
            return count > 1 ? (UInt16[])obj : new UInt16[] { (UInt16)obj };
        }

        public UInt32 R_A_DWord(ushort start) {
            return (UInt32)R_A_Value(start, VarType.DWord);
        }

        public UInt32[] R_A_DWords(ushort start, ushort count) {
            return (UInt32[])R_A_Value(start, VarType.DWord, count);
        }

        public Int16 R_A_Int16(ushort start) {
            return (Int16)R_A_Value(start, VarType.Int);
        }

        public Int16[] R_A_Int16s(ushort start, ushort count) {
            return (Int16[])R_A_Value(start, VarType.Int, count);
        }

        public Int32 R_A_Int32(ushort start) {
            return (Int32)R_A_Value(start, VarType.DInt);
        }

        public Int32[] R_A_Int32s(ushort start, ushort count) {
            return (Int32[])R_A_Value(start, VarType.DInt, count);
        }

        public float R_A_Float(ushort start) {
            return (float)R_A_Value(start, VarType.Real);
        }

        public float[] R_A_Floats(ushort start, ushort count) {
            return (float[])R_A_Value(start, VarType.Real, count);
        }

        public double R_A_Double(ushort start) {
            return (double)R_A_Value(start, VarType.LReal);
        }

        public double[] R_A_Doubles(ushort start, ushort count) {
            return (double[])R_A_Value(start, VarType.LReal, count);
        }

        private object R_M_Value(ushort start, VarType type, ushort count = 1) {
            return ReadValue(DataType.Memory, 0, start, type, count);
        }

        public bool R_M_Bit(ushort start, byte bit) {
            byte data = R_M_Byte((ushort)(start + bit / 8));
            return data.SelectBit(bit % 8);
        }

        public bool[] R_M_Bits(ushort start, byte bit, ushort count) {
            if (count <= 0) return new bool[0];
            int _bit = bit % 8;
            int _offset = bit / 8;
            int size = (count + _bit + 7) / 8;
            byte[] bytes = R_M_Bytes((ushort)(start + _offset), (ushort)size);
            bool[] result = new bool[count];
            for (int i = 0; i < count; i++) {
                int index = (i + _bit) / 8;
                result[i] = bytes[index].SelectBit((i + _bit) % 8);
            }
            return result;
        }

        public byte R_M_Byte(ushort start) {
            return (byte)R_M_Value(start, VarType.Byte);
        }

        public byte[] R_M_Bytes(ushort start, ushort count) {
            if (count <= 0) return new byte[0];
            object obj = R_M_Value(start, VarType.Byte, count);
            return count > 1 ? (byte[])obj : new byte[] { (byte)obj };
        }

        public UInt16 R_M_Word(ushort start) {
            return (UInt16)R_M_Value(start, VarType.Word);
        }

        public UInt16[] R_M_Words(ushort start, ushort count) {
            if (count <= 0) return new UInt16[0];
            object obj = R_M_Value(start, VarType.Word, count);
            return count > 1 ? (UInt16[])obj : new UInt16[] { (UInt16)obj };
        }

        public UInt32 R_M_DWord(ushort start) {
            return (UInt32)R_M_Value(start, VarType.DWord);
        }

        public UInt32[] R_M_DWords(ushort start, ushort count) {
            return (UInt32[])R_M_Value(start, VarType.DWord, count);
        }

        public Int16 R_M_Int16(ushort start) {
            return (Int16)R_M_Value(start, VarType.Int);
        }

        public Int16[] R_M_Int16s(ushort start, ushort count) {
            return (Int16[])R_M_Value(start, VarType.Int, count);
        }

        public Int32 R_M_Int32(ushort start) {
            return (Int32)R_M_Value(start, VarType.DInt);
        }

        public Int32[] R_M_Int32s(ushort start, ushort count) {
            return (Int32[])R_M_Value(start, VarType.DInt, count);
        }

        public float R_M_Float(ushort start) {
            return (float)R_M_Value(start, VarType.Real);
        }

        public float[] R_M_Floats(ushort start, ushort count) {
            return (float[])R_M_Value(start, VarType.Real, count);
        }

        public double R_M_Double(ushort start) {
            return (double)R_M_Value(start, VarType.LReal);
        }

        public double[] R_M_Doubles(ushort start, ushort count) {
            return (double[])R_M_Value(start, VarType.LReal, count);
        }

        private object R_DB_Value(ushort db, ushort start, VarType type, ushort count = 1) {
            return ReadValue(DataType.DataBlock, db, start, type, count);
        }

        public bool R_DB_Bit(ushort db, ushort start, byte bit) {
            byte data = R_DB_Byte(db, (ushort)(start + bit / 8));
            return data.SelectBit(bit % 8);
        }

        public bool[] R_DB_Bits(ushort db, ushort start, byte bit, ushort count) {
            if (count <= 0) return new bool[0];
            int _bit = bit % 8;
            int _offset = bit / 8;
            int size = (count + _bit + 7) / 8;
            byte[] bytes = R_DB_Bytes(db, (ushort)(start + _offset), (ushort)size);
            bool[] result = new bool[count];
            for (int i = 0; i < count; i++) {
                int index = (i + _bit) / 8;
                result[i] = bytes[index].SelectBit((i + _bit) % 8);
            }
            return result;
        }

        public byte R_DB_Byte(ushort db, ushort start) {
            return (byte)R_DB_Value(db, start, VarType.Byte);
        }

        public byte[] R_DB_Bytes(ushort db, ushort start, ushort count) {
            if (count <= 0) return new byte[0];
            object obj = R_DB_Value(db, start, VarType.Byte, count);
            return count > 1 ? (byte[])obj : new byte[] { (byte)obj };
        }

        public UInt16 R_DB_Word(ushort db, ushort start) {
            return (UInt16)R_DB_Value(db, start, VarType.Word);
        }

        public UInt16[] R_DB_Words(ushort db, ushort start, ushort count) {
            if (count <= 0) return new UInt16[0];
            object obj = R_DB_Value(db, start, VarType.Word, count);
            return count > 1 ? (UInt16[])obj : new UInt16[] { (UInt16)obj };
        }

        public UInt32 R_DB_DWord(ushort db, ushort start) {
            return (UInt32)R_DB_Value(db, start, VarType.DWord);
        }

        public UInt32[] R_DB_DWords(ushort db, ushort start, ushort count) {
            return (UInt32[])R_DB_Value(db, start, VarType.DWord, count);
        }

        public Int16 R_DB_Int16(ushort db, ushort start) {
            return (Int16)R_DB_Value(db, start, VarType.Int);
        }

        public Int16[] R_DB_Int16s(ushort db, ushort start, ushort count) {
            return (Int16[])R_DB_Value(db, start, VarType.Int, count);
        }

        public Int32 R_DB_Int32(ushort db, ushort start) {
            return (Int32)R_DB_Value(db, start, VarType.DInt);
        }

        public Int32[] R_DB_Int32s(ushort db, ushort start, ushort count) {
            return (Int32[])R_DB_Value(db, start, VarType.DInt, count);
        }

        public float R_DB_Float(ushort db, ushort start) {
            return (float)R_DB_Value(db, start, VarType.Real);
        }

        public float[] R_DB_Floats(ushort db, ushort start, ushort count) {
            return (float[])R_DB_Value(db, start, VarType.Real, count);
        }

        public double R_DB_Double(ushort db, ushort start) {
            return (double)R_DB_Value(db, start, VarType.LReal);
        }

        public double[] R_DB_Doubles(ushort db, ushort start, ushort count) {
            return (double[])R_DB_Value(db, start, VarType.LReal, count);
        }

        public string R_DB_CString(ushort db, ushort start, ushort size, bool trim = false) {
            string result = (string)R_DB_Value(db, start, VarType.String, size);
            int i = result.IndexOf('\0');
            if (i >= 0) {
                result = result.Substring(0, i);
            }
            return trim ? result.Trim() : result;
        }

        public string R_DB_S7String(ushort db, ushort start, ushort size, bool trim = false) {
            string result = (string)R_DB_Value(db, start, VarType.S7String, size);
            return trim ? result.Trim() : result;
        }

        public string R_DB_S7WString(ushort db, ushort start, ushort size, bool trim = false) {
            string result = (string)R_DB_Value(db, start, VarType.S7WString, size);
            return trim ? result.Trim() : result;
        }

        private void WriteValue(DataType dataType, ushort db, ushort start, object value) {
            if (value == null) {
                throw new Exception("PLC 无法写入 null ：" + GetAddress(dataType, db, start));
            }
            try {
                Task task = plc.WriteAsync(dataType, db, start, value);
                task.Wait(plc.WriteTimeout);
                if (!task.IsCompleted || task.IsFaulted) {
                    throw new Exception("PLC 写入超时：" + GetAddress(dataType, db, start));
                }
            } catch (Exception e) {
                throw new Exception("PLC 写入错误：" + GetAddress(dataType, db, start), e);
            }
        }

        private void W_E_Value(ushort start, object value) {
            WriteValue(DataType.Input, 0, start, value);
        }

        public void W_E_Byte(ushort start, byte value) {
            W_E_Value(start, value);
        }

        public void W_E_Bytes(ushort start, byte[] value, ushort count) {
            if (value == null || value.Length <= 0 || count <= 0) return;
            if (count < value.Length) {
                byte[] tmp = new byte[count];
                Array.Copy(value, 0, tmp, 0, count);
                W_E_Value(start, tmp);
            } else {
                W_E_Value(start, value);
            }
        }

        private void W_DB_Value(ushort db, ushort start, object value) {
            WriteValue(DataType.DataBlock, db, start, value);
        }

        private void W_DB_Values<T>(ushort db, ushort start, T[] value, ushort count) {
            if (value == null || value.Length <= 0 || count <= 0) return;
            if (count < value.Length) {
                T[] tmp = new T[count];
                Array.Copy(value, 0, tmp, 0, count);
                W_DB_Value(db, start, tmp);
            } else {
                W_DB_Value(db, start, value);
            }
        }

        public void W_DB_Byte(ushort db, ushort start, byte value) {
            W_DB_Value(db, start, value);
        }

        public void W_DB_Bytes(ushort db, ushort start, byte[] value, ushort count) {
            W_DB_Values(db, start, value, count);
        }

        public void W_DB_Int16s(ushort db, ushort start, Int16[] value, ushort count) {
            W_DB_Values(db, start, value, count);
        }

        public void W_DB_Int32s(ushort db, ushort start, Int32[] value, ushort count) {
            W_DB_Values(db, start, value, count);
        }

        public void W_DB_Floats(ushort db, ushort start, float[] value, ushort count) {
            W_DB_Values(db, start, value, count);
        }

        public void W_DB_Doubles(ushort db, ushort start, double[] value, ushort count) {
            W_DB_Values(db, start, value, count);
        }
    }
}
