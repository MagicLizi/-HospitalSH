using Hostpital;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HospitalSH
{
    public partial class Form1 : Form
    {
        bool test = false;

        Sensor s;
        public Form1()
        {
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (button1.Text == "打开数据采集")
                {
                    //直接读取出字符串
                    string host = System.IO.File.ReadAllText(@"sensorConfig.txt");

                    s = new Sensor();
                    s.mainForm = this;
                    if(test)
                    {
                        s.TestTick();
                    }
                    else
                    {
                        s.ConnectToSensor(host, 502);
                        s.BeginTick();
                    }
                    button1.Text = "关闭数据采集";
                }
                else
                {
                    if(test)
                    {
                        s.StopTest();
                    }
                    else
                    {
                        s.CloseSensor();
                    }
                    button1.Text = "打开数据采集";
                }
            }
            catch(Exception error)
            {
                MessageBox.Show(error.ToString());
            }
        }

        delegate void RefreshTextShowDelegate(string showContent, string sensorInfos);

        public void MainRefreshTextShow(string showContent, string sensorInfos)
        {
            this.Invoke(new RefreshTextShowDelegate(RefreshTextShow), showContent, sensorInfos);
        }

        public void RefreshTextShow(string showContent,string sensorInfos)
        {
            richTextBox1.Text = showContent +"\r\n";
            richTextBox1.Select(richTextBox1.Text.Length, 1);
            richTextBox1.ScrollToCaret();

            string p1 = @"red.png";
            string p2 = @"red.png";
            string p3 = @"red.png";
            string p4 = @"red.png";
            string p5 = @"red.png";
            //label6.Text = "有人";
            //label7.Text = "有人";
            //label8.Text = "有人";
            //label9.Text = "有人";
            //label10.Text = "有人";
            if (sensorInfos[7].ToString() == "0")
            {
                p1 = @"green.png";
                //label6.Text = "无人";
            }
            if (sensorInfos[6].ToString() == "0")
            {
                p2 = @"green.png";
               // label7.Text = "无人";
            }
            if (sensorInfos[5].ToString() == "0")
            {
                p3 = @"green.png";
               // label8.Text = "无人";
            }
            if (sensorInfos[4].ToString() == "0")
            {
                p4 = @"green.png";
                //label9.Text = "无人";
            }
            if (sensorInfos[3].ToString() == "0")
            {
                p5 = @"green.png";
               // label10.Text = "无人";
            }


            label2.Image = Image.FromFile(p1);
            label3.Image = Image.FromFile(p2);
            label4.Image = Image.FromFile(p3);
            label5.Image = Image.FromFile(p4);
            label6.Image = Image.FromFile(p5);

        }

    }
}
