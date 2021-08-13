using JK.Base.Logs;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JK.Base.SerialPorts
{
    /// <summary>
    /// 연결지향
    /// </summary>
    public class ConnectionOrientedSerialProt : IDisposable
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
        /// Error Message
        /// </summary>
        public string ErrorMessage { get; private set; } = string.Empty;
        /// <summary>
        /// Data Received EventHandler
        /// </summary>
        public SerialDataReceivedEventHandler DataReceived;

        public ConnectionOrientedSerialProt(string portName, SerialDataReceivedEventHandler dataReceived, int baudrate = 9600)
        {
            this.PortName = portName;
            this.Baudrate = baudrate;
            this.DataReceived = dataReceived;
        }


        public bool Open()
        {
            try
            {
                Prepare();

                SerialPort.Open();
                SerialPort.DiscardInBuffer();
                SerialPort.DiscardOutBuffer();
                SerialPort.DataReceived += DataReceived;

                return true;
            }
            catch (Exception e)
            {
                ErrorMessage = "Port Opne Fail";
                Dispose();
                Log.ei(e, "Port Opne Fail");
                return false;
            }
        }

        public void Prepare()
        {
            try
            {
                Dispose();
                ErrorMessage = string.Empty;

                SerialPort = new SerialPort(PortName, Baudrate);
            }
            catch (Exception e)
            {
                Dispose();
                Log.ei(e, "Prepare Fail");
            }
        }

        public void Dispose()
        {
            try
            {
                if (SerialPort != null)
                {
                    if (SerialPort.IsOpen)
                        SerialPort.Close();

                    SerialPort.DataReceived -= DataReceived;
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

        public bool Request(byte[] msg)
        {
            try
            {
                SerialPort.Write(msg, 0, msg.Length);

                return true;
            }
            catch (Exception e)
            {
                Prepare();
                ErrorMessage = "Port Request Fail";

                Log.ei(e, "Port Request Fail");
                return false;
            }
        }
    }
}
