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
                    s.ConnectToSensor(host, 502);
                    s.BeginTick();
                    button1.Text = "关闭数据采集";
                }
                else
                {
                    s.CloseSensor();
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

            string p1 = @"softred.png";
            string p2 = @"softred.png";
            string p3 = @"softred.png";
            string p4 = @"softred.png";
            string p5 = @"softred.png";
            label6.Text = "有人";
            label7.Text = "有人";
            label8.Text = "有人";
            label9.Text = "有人";
            label10.Text = "有人";
            if (sensorInfos[7].ToString() == "0")
            {
                p1 = @"softgreen.png";
                label6.Text = "无人";
            }
            if (sensorInfos[6].ToString() == "0")
            {
                p2 = @"softgreen.png";
                label7.Text = "无人";
            }
            if (sensorInfos[5].ToString() == "0")
            {
                p3 = @"softgreen.png";
                label8.Text = "无人";
            }
            if (sensorInfos[4].ToString() == "0")
            {
                p4 = @"softgreen.png";
                label9.Text = "无人";
            }
            if (sensorInfos[3].ToString() == "0")
            {
                p5 = @"softgreen.png";
                label10.Text = "无人";
            }


            pictureBox1.Image = Image.FromFile(p1);
            pictureBox2.Image = Image.FromFile(p2);
            pictureBox3.Image = Image.FromFile(p3);
            pictureBox4.Image = Image.FromFile(p4);
            pictureBox5.Image = Image.FromFile(p5);

        }

    }
}
