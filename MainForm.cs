using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Collections.Specialized;

namespace СCOMTransmitter
{
    public partial class MainForm : Form
    {


        string[] portnames;
        int boud;
        int delay;
        string message;
        SerialPort actport;
        System.Threading.Timer acttimer;
        bool generator;
        bool mousemove;
        long counter;
        int mousepos;

        public object ConfigurationManager { get; private set; }
        public object ConfigurationUserLevel { get; private set; }

        public void Refresh()
        {
            portnames = SerialPort.GetPortNames();
            this.SetCOMs(portnames);
        }
        public void Connect(string portname)
        {
            SerialPort port = new SerialPort( portname , boud, Parity.None, 8, StopBits.One);
            port.Open();
            actport = port;
            TimerCallback tm = new TimerCallback(Send);
            System.Threading.Timer timer = new System.Threading.Timer(tm, null, 0, delay*1000);
            acttimer = timer;
        }
        public void Send(object obj)
        {
            if (generator == false && mousemove == false)
            {
                actport.WriteLine(message);
            }
            else if (generator == true && mousemove == false)
            {
                actport.WriteLine(counter.ToString());
                counter++;
                
            }
            else if (mousemove == true)
            {
                int val = (mousepos - Cursor.Position.X);
                char c;
                byte pack;
                string vl = "p";
                byte[] obr =new byte[2];
                obr[0] = Convert.ToByte(112);
                if (val > 127)
                {
                    val = 127;                  
                }     
                if (val >0) obr[1] = Convert.ToByte(val);
                if (val < 0)
                {
                    if (val < -127) val = -127;
                    val = -val;
                    obr[1]= Convert.ToByte(val+127);
;
                } 
                
                actport.Write(obr, 0, 2);
                mousepos = Cursor.Position.X;

            }


        }


        public MainForm()
        {
            InitializeComponent();
            this.Refresh();
            boud =  9600;
            delay = 2;
            message = "Hello";
            generator = false;
            counter = 0;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            message = textBox1.Text;
           
        }


        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (actport != null)
            {
                actport.Close();
                actport = null;
                counter = 0;
                acttimer.Dispose();
                acttimer = null;
            }
            this.Connect(listBox1.SelectedItem.ToString());
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Refresh();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
          boud= Convert.ToInt32(comboBox1.SelectedItem.ToString());
            if (actport!=null)
            {
                actport.Close();
                actport.BaudRate= boud;
                actport.Open();

            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            int del;
                if (Int32.TryParse(textBox3.Text, out del))
            {
                if (del >=1)
                {
                    delay = del;
                    this.textBox3.ForeColor = Color.Black;
                    acttimer.Change(0, delay);
                }
                else
                {
                    this.textBox3.ForeColor = Color.Red;
                }

            }
            else
            {
                this.textBox3.ForeColor = Color.Red;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (generator)
            {
                generator = false;
            }
            else
            {
                generator = true;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (mousemove)
            {
                mousemove = false;
            }
            else
            {
                mousemove = true;
            }

        }
    }
}
