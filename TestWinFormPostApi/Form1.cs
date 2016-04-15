using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestWinFormPostApi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private SerialPort mySerialPort = new SerialPort("COM1", 9600, Parity.None, 8, StopBits.One);
        private void button1_Click(object sender, EventArgs e)
        {
            SerialPort mySerialPort = new SerialPort();
            mySerialPort.BaudRate = 9600;
            mySerialPort.Parity = Parity.None;
            mySerialPort.StopBits = StopBits.One;
            mySerialPort.DataBits = 8;
            mySerialPort.Handshake = Handshake.None;
            mySerialPort.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
            mySerialPort.Open();
            string value = "After Test Post API (Result From API)"; //This value is sent to the API and again returned the same from it
            if ((mySerialPort.ReadExisting()) == String.Empty) txtOutput2.Text = "Testing Port";
            var text = DataReceivedHandler(value);
            txtOutput.Text = text.Result; 
            mySerialPort.Close();
        }

        /// <summary>
        /// Event Handler Method Add if required
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            // Show all the incoming data in the port's buffer
            //if ((mySerialPort.ReadByte()) == 0) txtOutput2.Text = "Testing Port";
        }

        private static async Task<string> DataReceivedHandler(string value)
        {
            using (var client = new HttpClient())
            {
                var url = new Uri("http://localhost:58941/api/values?value="); //Change the Url to the Host (This is the default Values APi Controller)
                var result = client.PostAsJsonAsync(url, value);
                result.Result.EnsureSuccessStatusCode();
                if (result.Result.IsSuccessStatusCode)
                {
                    string testResult = await result.Result.Content.ReadAsStringAsync();
                    return testResult;
                }
                return await Task.Delay(10000)
                                .ContinueWith(t => "Hello");
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
        }
    }
}
