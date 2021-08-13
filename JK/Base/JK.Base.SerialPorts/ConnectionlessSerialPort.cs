using JK.Base.Logs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JK.Base.SerialPorts
{
    /// <summary>
    /// 비연결성
    /// </summary>
    public class ConnectionlessSerialPort : IDisposable
    {
        /// <summary>
        /// SerialPort
        /// </summary>
        public SerialPort SerialPort { get; private set; } = null;
        /// <summary>
        /// PortName
        /// </summary>
        public string PortName { get; set; } = string.Empty;
        /// <summary>
        /// Baudrate
        /// </summary>
        public int Baudrate { get; set; } = 9600;
        /// <summary>
        /// Wirte Timeout (ms)
        /// </summary>
        public int WirteTimeout { get; set; } = 10 * 1000;
        /// <summary>
        /// Error Message
        /// </summary>
        public string ErrorMessage { get; private set; } = string.Empty;

        public ConnectionlessSerialPort(string portName, int baudrate = 9600, int wirteTimeout = 10 * 1000)
        {
            this.PortName = portName;
            this.Baudrate = baudrate;
            this.WirteTimeout = wirteTimeout;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            try
            {
                ErrorMessage = string.Empty;

                if (SerialPort != null)
                {
                    if (SerialPort.IsOpen)
                        SerialPort.Close();

                    SerialPort.Dispose();
                    SerialPort = null;
                }
            }
            catch (Exception e)
            {
                Log.ei(e, "SerialPort Dispose Fail");
            }
        }

        /// <summary>
        /// Data Write
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="readTimeout"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public bool Write(byte[] msg, out byte[] response)
        {
            response = new byte[] { };

            try
            {
                try
                {
                    Dispose();

                    SerialPort = new SerialPort(PortName, Baudrate);
                    SerialPort.Open();
                    SerialPort.DiscardInBuffer();
                    SerialPort.DiscardOutBuffer();
                }
                catch (Exception e)
                {
                    ErrorMessage = "SerialPort Opne Fail";
                    SerialPort?.Close();

                    Log.ei(e, "SerialPort Opne Fail");

                    return false;
                }

                using (var ms = new MemoryStream())
                {
                    using (var mrEvent = new ManualResetEventSlim(false))
                    {
                        SerialPort.Write(msg, 0, msg.Length);

                        var sw = new Stopwatch();
                        sw.Start();

                        while (!mrEvent.Wait(100))
                        {
                            int readByte = SerialPort.BytesToRead;
                            byte[] bytes = new byte[readByte];

                            SerialPort.Read(bytes, 0, readByte);
                            ms.Write(bytes, 0, bytes.Length);

                            if (bytes[bytes.Length - 1] == HexCodes.ETX)
                            {
                                mrEvent.Set();

                            }
                            else if (sw.ElapsedMilliseconds >= WirteTimeout)
                            {
                                mrEvent.Set();
                                ErrorMessage = "SerialPort Write Timeout";
                                return false;
                            }
                        }

                        sw.Stop();
                    }

                    response = ms.ToArray();
                }

                return true;
            }
            catch (Exception e)
            {
                Dispose();
                ErrorMessage = "SerialPort Write Fail";

                Log.ei(e, "SerialPort Write Fail");

                return false;
            }
            finally
            {
                SerialPort?.Close();
            }
        }
    }
}
