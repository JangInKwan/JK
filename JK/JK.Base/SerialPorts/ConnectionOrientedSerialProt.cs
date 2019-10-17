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
    public class ConnectionOrientedSerialProt
    {
        public SerialPort SerialPort { get; private set; } = null;

        public string PortName { get; set; } = string.Empty;

        public int Baudrate { get; set; } = 9600;

        public string ErrorMessage { get; private set; } = string.Empty;

        public SerialDataReceivedEventHandler DataReceived;


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
                Release();
                Log.ei("Port Opne Fail", e);
                return false;
            }
        }

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
                Release();
                Log.ei("Prepare fail", e);
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

                    SerialPort.DataReceived -= DataReceived;
                    SerialPort.Dispose();
                    SerialPort = null;
                }
            }
            catch (Exception e)
            {
                Log.ei("Release fail", e);
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
                return false;
            }
        }
    }
}
