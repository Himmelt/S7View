using S7.Net;
using System;
using System.Collections.Generic;
using System.Text;

namespace S7Lib
{
    public static class S7API
    {
        private static readonly List<string> LOG = new List<string>();
        private static readonly List<string> IP = new List<string>();
        private static readonly Dictionary<short, Plc> ID_PLC = new Dictionary<short, Plc>();
        private static readonly Dictionary<short, List<string>> PLC_LOG = new Dictionary<short, List<string>>();

        public static string GetLog()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var line in LOG)
            {
                sb.AppendLine(line);
            }
            return sb.ToString();
        }
        public static string GetLog(short plcId)
        {
            if (PLC_LOG.TryGetValue(plcId, out var LOG))
            {
                StringBuilder sb = new StringBuilder();
                foreach (var line in LOG)
                {
                    sb.AppendLine(line);
                }
                return sb.ToString();
            }
            return "";
        }
        private static void Log(string msg)
        {
            if (LOG.Count > 20)
            {
                LOG.RemoveAt(0);
            }
            LOG.Add(DateTime.Now.ToString("[HH:mm:ss.fff] ") + msg);
        }
        private static void Log(short plcId, string msg)
        {
            if (!PLC_LOG.ContainsKey(plcId))
            {
                PLC_LOG.Add(plcId, new List<string>());
            }
            List<string> LOG = PLC_LOG[plcId];
            if (LOG.Count > 20)
            {
                LOG.RemoveAt(0);
            }
            LOG.Add(DateTime.Now.ToString("[HH:mm:ss.fff] ") + msg);
        }
        public static void W_DB_Byte(short plcId, int db, int start, byte data)
        {
            if (ID_PLC.TryGetValue(plcId, out Plc plc))
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
            if (ID_PLC.TryGetValue(plcId, out Plc plc))
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
            if (ID_PLC.TryGetValue(plcId, out Plc plc))
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
            if (ID_PLC.TryGetValue(plcId, out Plc plc))
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
            if (ID_PLC.TryGetValue(plcId, out Plc plc))
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
            if (ID_PLC.TryGetValue(plcId, out Plc plc))
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
            if (ID_PLC.TryGetValue(plcId, out Plc plc))
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
            if (ID_PLC.TryGetValue(plcId, out Plc plc))
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
            if (ID_PLC.TryGetValue(plcId, out Plc plc))
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
            if (ID_PLC.TryGetValue(plcId, out Plc plc))
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
            if (ID_PLC.TryGetValue(plcId, out Plc plc))
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
            if (ID_PLC.TryGetValue(plcId, out Plc plc))
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
            if (ID_PLC.TryGetValue(plcId, out Plc plc))
            {
                try
                {
                    return (double[])plc.Read(DataType.DataBlock, db, start, VarType.LReal, count);
                }
                catch (Exception e)
                {
                    Log(plcId, e.Message);
                }
            }
            Log("ID:" + plcId + " 的 PLC 不存在.");
            return new double[count];
        }
        public static bool IsConnected(short plcId)
        {
            if (ID_PLC.TryGetValue(plcId, out Plc plc))
            {
                try
                {
                    return plc.IsConnected;
                }
                catch (Exception)
                {
                }
            }
            Log("ID:" + plcId + " 的 PLC 不存在.");
            return false;
        }
        public static bool IsRunning(short plcId)
        {
            if (ID_PLC.TryGetValue(plcId, out Plc plc))
            {
                try
                {
                    return plc.ReadStatus() == 0x08;
                }
                catch (Exception)
                {
                }
            }
            Log("ID:" + plcId + " 的 PLC 不存在.");
            return false;
        }
        public static void Open(short id, string ip, short type, short rack, short slot)
        {
            if (ID_PLC.ContainsKey(id))
            {
                Log("项目中已存在相同 ID:" + id + " 的 PLC.");
                return;
            }
            if (IP.Contains(ip))
            {
                Log("项目中已创建相同 IP:" + ip + " 的 PLC.");
                return;
            }
            try
            {
                Plc plc = new Plc((CpuType)type, ip, rack, slot);
                IP.Add(ip);
                ID_PLC[id] = plc;
                plc.ReadTimeout = 500;
                plc.Open();
                Log(id, "PLC 已连接.");
            }
            catch (Exception e)
            {
                Log("PLC ID:" + id + ",IP:" + ip + " 连接失败.");
                Log(e.Message);
            }
        }

        public static bool IsRegistered(short plcId)
        {
            return ID_PLC.ContainsKey(plcId);
        }

        public static void Run(short plcId, out bool isRunning, out bool isConnected, out string msg)
        {
            isRunning = isConnected = false;
            if (ID_PLC.TryGetValue(plcId, out Plc plc))
            {
                try
                {
                    if (!plc.IsConnected)
                    {
                        plc.Open();
                    }
                    isConnected = plc.IsConnected;
                    isRunning = IsRunning(plcId);
                }
                catch (Exception e)
                {
                    Log(plcId, "PLC 连接异常.");
                    Log(plcId, e.Message);
                }
            }
            else
            {
                Log(plcId, "ID:" + plcId + " 的 PLC 不存在.");
            }
            msg = GetLog(plcId);
        }

        /// <summary>
        /// 初始化，以清除静态存储的数据
        /// 每个 VI 项目在代码的入口处 必须 先调用一次初始化
        /// 初始化完成后再执行后续业务逻辑
        /// </summary>
        public static void Init()
        {
            IP.Clear();
            foreach (var plc in ID_PLC.Values)
            {
                plc.Close();
            }
            ID_PLC.Clear();
        }

        /// <summary>
        /// 释放资源，以清除静态存储的数据
        /// 每个 VI 项目在代码的 结束 处 应当 调用一次释放资源
        /// </summary>
        public static void Release()
        {
            Init();
        }
    }
}
