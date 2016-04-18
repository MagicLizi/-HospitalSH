using HospitalSH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Hostpital
{
    class Sensor
    {
        public Form1 mainForm;

        Socket socket;

        static int id = 0;

        static object _object = new object();

        System.Timers.Timer tmr;
        public void ConnectToSensor(string host, int port)
        {

            IPAddress myIP = IPAddress.Parse(host);
            IPEndPoint hostEP = new IPEndPoint(myIP, port);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(hostEP);
            Console.WriteLine("连接传感器成功...");
            Console.WriteLine();

        }

        public void CloseSensor()
        {
            tmr.Stop();
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();

        }

        public void BeginTick()
        {
            Console.WriteLine("开始监控轮询传感器数据...");
            Console.WriteLine();
            tmr = new Timer();
            tmr.Interval = 2000;
            tmr.Elapsed += new ElapsedEventHandler(tmr_Elapsed);
            tmr.Start();
        }

        void tmr_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                Console.WriteLine("编号{0}：获取传感器信息...", id);
                id++;
                GetSensorInfo();
            }
            catch(Exception error)
            {

            }
        }

        public void GetSensorInfo()
        {
            lock (_object)
            {
                byte[] sendCommand = new byte[12];
                sendCommand[0] = 0x00;
                sendCommand[1] = 0x01;
                sendCommand[2] = 0x00;
                sendCommand[3] = 0x00;
                sendCommand[4] = 0x00;
                sendCommand[5] = 0x06;
                sendCommand[6] = 0xFF;
                sendCommand[7] = 0x02;
                sendCommand[8] = 0x00;
                sendCommand[9] = 0xC8;
                sendCommand[10] = 0x00;
                sendCommand[11] = 0x08;
                socket.Send(sendCommand);

                byte[] result = new byte[1024];
                int receiveLength = socket.Receive(result);
                string sensorInfo = DecimalToBinary(result[9]);
                ShowSensorInfo(sensorInfo);
            }
        }

        public string DecimalToBinary(int decimalNum)
        {
            string binaryNum = Convert.ToString(decimalNum, 2);
            if (binaryNum.Length < 8)
            {
                int length = binaryNum.Length;
                for (int i = 0; i < 8 - length; i++)
                {
                    binaryNum = '0' + binaryNum;
                }
            }
            return binaryNum;
        }

        // 0 断开 1闭合
        public void ShowSensorInfo(string sensorInfo)
        {
            Console.WriteLine("接收最新传感器二进制数据：{0}", sensorInfo);
            string DI1 = "断开";
            string DI2 = "断开";
            string DI3 = "断开";
            string DI4 = "断开";
            string DI5 = "断开";
            if (sensorInfo[7].ToString() == "1")
            {
                DI1 = "闭合";
            }
            if (sensorInfo[6].ToString() == "1")
            {
                DI2 = "闭合";
            }
            if (sensorInfo[5].ToString() == "1")
            {
                DI3 = "闭合";
            }
            if (sensorInfo[5].ToString() == "1")
            {
                DI4 = "闭合";
            }
            if (sensorInfo[3].ToString() == "1")
            {
                DI5 = "闭合";
            }

            string content = "编号" + id + "：获取传感器信息..."+"\r\n" +
                             "接收最新传感器二进制数据：" + sensorInfo + "\r\n" +
                             "DI1:" + DI1 + "\r\n" +
                             "DI2:" + DI2 + "\r\n" +
                             "DI3:" + DI3 + "\r\n" +
                             "DI4:" + DI4 + "\r\n" +
                             "DI5:" + DI5 + "\r\n";
            mainForm.RefreshTextShow(content, sensorInfo);

            Console.WriteLine("DI1:" + DI1);
            Console.WriteLine("DI2:" + DI2);
            Console.WriteLine("DI3:" + DI3);
            Console.WriteLine("DI4:" + DI4);
            Console.WriteLine("DI5:" + DI5);
            Console.WriteLine("---------------------------------------");
            Console.WriteLine();
        }
    }
}
