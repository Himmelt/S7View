using S7.Net;
using System;
using System.Collections.Generic;

namespace S7Lib
{
    public static class S7API
    {
        private static List<string> plcs_ips = new List<string>();
        private static Dictionary<short, Plc> plc_ids = new Dictionary<short, Plc>();
        public static void W_DB_Byte(short plcId, int db, int start, byte data)
        {
            if (plc_ids.TryGetValue(plcId, out Plc plc))
            {
                if (!plc.IsConnected)
                {
                    plc.Open();
                }
                plc.Write(DataType.DataBlock, db, start, data);
                return;
            }
            throw new PlcException(ErrorCode.ConnectionError, "ID:" + plcId + " 的 PLC 不存在.");
        }
        public static void W_DB_Float(short plcId, int db, int start, float data)
        {
            if (plc_ids.TryGetValue(plcId, out Plc plc))
            {
                if (!plc.IsConnected)
                {
                    plc.Open();
                }
                plc.Write(DataType.DataBlock, db, start, data);
                return;
            }
            throw new PlcException(ErrorCode.ConnectionError, "ID:" + plcId + " 的 PLC 不存在.");
        }
        public static void W_DB_Double(short plcId, int db, int start, double data)
        {
            if (plc_ids.TryGetValue(plcId, out Plc plc))
            {
                if (!plc.IsConnected)
                {
                    plc.Open();
                }
                plc.Write(DataType.DataBlock, db, start, data);
                return;
            }
            throw new PlcException(ErrorCode.ConnectionError, "ID:" + plcId + " 的 PLC 不存在.");
        }
        public static byte R_DB_Byte(short plcId, int db, int start)
        {
            if (plc_ids.TryGetValue(plcId, out Plc plc))
            {
                if (!plc.IsConnected)
                {
                    plc.Open();
                }
                return (byte)plc.Read(DataType.DataBlock, db, start, VarType.Byte, 1);
            }
            throw new PlcException(ErrorCode.ConnectionError, "ID:" + plcId + " 的 PLC 不存在.");
        }
        public static byte[] R_DB_Bytes(short plcId, int db, int start, int count)
        {
            if (plc_ids.TryGetValue(plcId, out Plc plc))
            {
                if (!plc.IsConnected)
                {
                    plc.Open();
                }
                return (byte[])plc.Read(DataType.DataBlock, db, start, VarType.Byte, count);
            }
            throw new PlcException(ErrorCode.ConnectionError, "ID:" + plcId + " 的 PLC 不存在.");
        }

        public static Int16 R_DB_Int(short plcId, int db, int start)
        {
            if (plc_ids.TryGetValue(plcId, out Plc plc))
            {
                if (!plc.IsConnected)
                {
                    plc.Open();
                }
                return (Int16)plc.Read(DataType.DataBlock, db, start, VarType.Int, 1);
            }
            throw new PlcException(ErrorCode.ConnectionError, "ID:" + plcId + " 的 PLC 不存在.");
        }
        public static Int16[] R_DB_Ints(short plcId, int db, int start, int count)
        {
            if (plc_ids.TryGetValue(plcId, out Plc plc))
            {
                if (!plc.IsConnected)
                {
                    plc.Open();
                }
                return (Int16[])plc.Read(DataType.DataBlock, db, start, VarType.Int, count);
            }
            throw new PlcException(ErrorCode.ConnectionError, "ID:" + plcId + " 的 PLC 不存在.");
        }

        public static Int32 R_DB_DInt(short plcId, int db, int start)
        {
            if (plc_ids.TryGetValue(plcId, out Plc plc))
            {
                if (!plc.IsConnected)
                {
                    plc.Open();
                }
                return (Int32)plc.Read(DataType.DataBlock, db, start, VarType.DInt, 1);
            }
            throw new PlcException(ErrorCode.ConnectionError, "ID:" + plcId + " 的 PLC 不存在.");
        }
        public static Int32[] R_DB_DInts(short plcId, int db, int start, int count)
        {
            if (plc_ids.TryGetValue(plcId, out Plc plc))
            {
                if (!plc.IsConnected)
                {
                    plc.Open();
                }
                return (Int32[])plc.Read(DataType.DataBlock, db, start, VarType.Int, count);
            }
            throw new PlcException(ErrorCode.ConnectionError, "ID:" + plcId + " 的 PLC 不存在.");
        }
        public static float R_DB_Float(short plcId, int db, int start)
        {
            if (plc_ids.TryGetValue(plcId, out Plc plc))
            {
                if (!plc.IsConnected)
                {
                    plc.Open();
                }
                return (float)plc.Read(DataType.DataBlock, db, start, VarType.Real, 1);
            }
            throw new PlcException(ErrorCode.ConnectionError, "ID:" + plcId + " 的 PLC 不存在.");
        }
        public static float[] R_DB_Floats(short plcId, int db, int start, int count)
        {
            if (plc_ids.TryGetValue(plcId, out Plc plc))
            {
                if (!plc.IsConnected)
                {
                    plc.Open();
                }
                return (float[])plc.Read(DataType.DataBlock, db, start, VarType.Real, count);
            }
            throw new PlcException(ErrorCode.ConnectionError, "ID:" + plcId + " 的 PLC 不存在.");
        }
        public static double R_DB_Double(short plcId, int db, int start)
        {
            if (plc_ids.TryGetValue(plcId, out Plc plc))
            {
                if (!plc.IsConnected)
                {
                    plc.Open();
                }
                return (double)plc.Read(DataType.DataBlock, db, start, VarType.LReal, 1);
            }
            throw new PlcException(ErrorCode.ConnectionError, "ID:" + plcId + " 的 PLC 不存在.");
        }
        public static double[] R_DB_Doubles(short plcId, int db, int start, int count)
        {
            if (plc_ids.TryGetValue(plcId, out Plc plc))
            {
                if (!plc.IsConnected)
                {
                    plc.Open();
                }
                return (double[])plc.Read(DataType.DataBlock, db, start, VarType.LReal, count);
            }
            throw new PlcException(ErrorCode.ConnectionError, "ID:" + plcId + " 的 PLC 不存在.");
        }
        public static bool IsConnected(short plcId)
        {
            if (plc_ids.TryGetValue(plcId, out Plc plc))
            {
                return plc.IsConnected;
            }
            throw new PlcException(ErrorCode.ConnectionError, "ID:" + plcId + " 的 PLC 不存在.");
        }
        public static bool IsRunning(short plcId)
        {
            if (plc_ids.TryGetValue(plcId, out Plc plc))
            {
                return plc.ReadStatus() == 0x08;
            }
            throw new PlcException(ErrorCode.ConnectionError, "ID:" + plcId + " 的 PLC 不存在.");
        }

        public static void GetStatus(short plcId, out bool isRunning, out bool isConnected, out string msg)
        {
            if (plc_ids.TryGetValue(plcId, out Plc plc))
            {
                try
                {
                    isRunning = plc.ReadStatus() == 0x08;
                    isConnected = plc.IsConnected;
                    msg = "Msg 测试";
                    return;
                }
                catch (Exception e)
                {
                    isRunning = isConnected = false;
                    msg = e.Message;
                    return;
                }
            }
            isRunning = isConnected = false;
            msg = "ID:" + plcId + " 的 PLC 不存在.";
        }
        public static void Open(short plcId, string ip, short type, short rack, short slot, out bool isRunning, out bool isConnected, out string msg)
        {
            if (plc_ids.ContainsKey(plcId))
            {
                isRunning = isConnected = false;
                msg = "项目中已创建相同 ID:" + plcId + " 的 PLC.";
                return;
            }
            if (plcs_ips.Contains(ip))
            {
                isRunning = isConnected = false;
                msg = "项目中已创建相同 IP:" + ip + " 的 PLC.";
                return;
            }
            try
            {
                Plc plc = new Plc((CpuType)type, ip, rack, slot);
                plcs_ips.Add(ip);
                plc_ids[plcId] = plc;
                plc.ReadTimeout = 500;
                plc.Open();
                isConnected = plc.IsConnected;
                isRunning = plc.ReadStatus() == 0x08;
                msg = "PLC 已连接.";
            }
            catch (Exception e)
            {
                isConnected = false;
                isRunning = false;
                msg = "PLC 连接失败.\nID:" + plcId + ",IP:" + ip + "\n" + e.Message;
            }
        }
        public static void Open(short plcId, string ip, short type, short rack, short slot)
        {
            Open(plcId, ip, type, rack, slot, out _, out _, out _);
        }

        public static void AutoConnect(short plcId)
        {
            if (plc_ids.TryGetValue(plcId, out Plc plc))
            {
                try
                {
                    if (!plc.IsConnected)
                    {
                        plc.Open();
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        public static void Init()
        {
            plcs_ips.Clear();
            foreach (var plc in plc_ids.Values)
            {
                plc.Close();
            }
            plc_ids.Clear();
        }
    }
}
