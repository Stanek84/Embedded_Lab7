using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Embedded1
{
    public partial class Form2 : Form
    {
        int startflag = 0;
        int flag_sensor;
        string RxString;
        string temp;

        private void Form2_Load(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
                serialPort1.Close();

            serialPort1.PortName = "COM3";
            serialPort1.BaudRate = 9600;
        }

        public Form2()
        {
            InitializeComponent();
        }

        private void Serial_Start_Click(object sender, EventArgs e)
        {
            serialPort1.PortName = "COM3";
            serialPort1.BaudRate = 9600;

            serialPort1.Open();
            if (serialPort1.IsOpen)
            {
                // startSerial.Enabled = false;
                // serialStop.Enabled = true;
                textBox1.ReadOnly = false;
            }

        }

        private void Serial_Stop_Click(object sender, EventArgs e)
        {
            serialPort1.Close();
            //  startSerial.Enabled = true;
            //  serialStop.Enabled = false;
            textBox1.ReadOnly = true;
        }

        private void Read_in_TS_Channel_Click(object sender, EventArgs e)
        {
            WebClient client = new WebClient();


            client.DownloadString("http://api.thingspeak.com/channels/1566867/feed.json");
            label1.Text = client.DownloadString("http://api.thingspeak.com/channels/1566864/field/field1/last.text");


        }

        private void label1_Click(object sender, EventArgs e) //label1_Click
        {
            if (serialPort1.IsOpen)
                serialPort1.Close();

            serialPort1.PortName = "COM3";
            serialPort1.BaudRate = 9600;

        }

        private void Current_Data_Click(object sender, EventArgs e)
        {
            textBox1.AppendText(RxString);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!string.Equals(textBox1.Text, ""))
            {
                if (serialPort1.IsOpen) serialPort1.Close();
                try
                {
                      if (RxString[0] == 'B')
                      {
                          flag_sensor = 10;

                      }

                    const string WRITEKEY = "P6OO9YMV02PI4NJX";
                    string strUpdateBase = "http://api.thingspeak.com/update";


                    string strUpdateURI = strUpdateBase + "?api_key=" + WRITEKEY;
                    string strField1 = textBox1.Text;

                    HttpWebRequest ThingsSpeakReq;
                    HttpWebResponse ThingsSpeakResp;



                    if (flag_sensor == 11)
                    {
                        strUpdateURI += "&field1=" + strField1;
                    }
                    if (flag_sensor == 12)
                    {
                        strUpdateURI += "&field2=" + strField1;
                        flag_sensor = 10;
                    }


                    flag_sensor++;
                    ThingsSpeakReq = (HttpWebRequest)WebRequest.Create(strUpdateURI);

                    ThingsSpeakResp = (HttpWebResponse)ThingsSpeakReq.GetResponse();
                    ThingsSpeakResp.Close();

                    if (!(string.Equals(ThingsSpeakResp.StatusDescription, "OK")))
                    {
                        Exception exData = new Exception(ThingsSpeakResp.StatusDescription);
                        throw exData;
                    }

                }
                catch (Exception ex)
                {

                }
                textBox1.Text = "";


                serialPort1.Open();
            }

        }
        private void SerialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            RxString = serialPort1.ReadExisting();
            this.Invoke(new EventHandler(Current_Data_Click));


        }

        private void Form2_Load_1(object sender, EventArgs e)
        {

        }
    }

}
