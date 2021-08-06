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
    public class ConnectionlessSerialPort
    {
        public SerialPort SerialPort { get; private set; } = null;

        public string PortName { get; set; } = string.Empty;

        public int Baudrate { get; set; } = 9600;

        public string ErrorMessage { get; private set; } = string.Empty;


        public void Prepare()
        {
            try
            {
                Release();
                ErrorMessage = string.Empty;

                SerialPort = new SerialPort(PortName, Baudrate);
            }
            catch (Exception e)
            {
                Log.ei(e, "Prepare Fail");
            }
        }

        public void Release()
        {
            try
            {

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
                Log.ei(e, "Release Fail");
            }
        }

        public bool IsOpen
        {
            get
            {
                if (SerialPort != null)
                {
                    return SerialPort.IsOpen;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool Request(byte[] msg, int readTimeout, out byte[] response)
        {
            response = new byte[] { };

            try
            {
                Prepare();

                try
                {
                    SerialPort.Open();
                    SerialPort.DiscardInBuffer();
                    SerialPort.DiscardOutBuffer();
                }
                catch (Exception e)
                {
                    ErrorMessage = "Port Opne Fail";
                    SerialPort.Close();

                    Log.ei(e, "Port Opne Fail");

                    return false;
                }

                using (var ms = new MemoryStream())
                {
                    using (var _oReceiveEvent = new ManualResetEventSlim(false))
                    {
                        SerialPort.Write(msg, 0, msg.Length);

                        var sw = new Stopwatch();
                        sw.Start();

                        while (!_oReceiveEvent.Wait(100))
                        {
                            int readByte = SerialPort.BytesToRead;
                            byte[] bytes = new byte[readByte];

                            SerialPort.Read(bytes, 0, readByte);
                            ms.Write(bytes, 0, bytes.Length);

                            if (bytes[bytes.Length - 1] == HexCodes.ETX)
                            {
                                _oReceiveEvent.Set();

                            }
                            else if (sw.ElapsedMilliseconds >= readTimeout)
                            {
                                _oReceiveEvent.Set();
                                ErrorMessage = "Request Timeout";
                                return false;
                            }
                        }

                        sw.Stop();
                    }

                    response = ms.ToArray();

                    return true;
                }
            }
            catch (Exception e)
            {
                Prepare();
                ErrorMessage = "Port Request Fail";

                Log.ei(e, "Port Request Fail");

                return false;
            }
            finally
            {
                SerialPort?.Close();
            }
        }
    }
}
