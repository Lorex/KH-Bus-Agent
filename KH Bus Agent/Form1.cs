using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace KH_Bus_Agent
{
    public partial class Form1 : Form
    {
        XmlDocument doc = new XmlDocument();
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            RefRoute();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            RefRoute();
        }
        private void RefRoute()
        {
            comboBox1.Items.Clear();
            //Load XML
            doc.Load("http://122.146.229.210/xmlbus2/StaticData/GetRoute.xml");

            //Select Node List
            XmlNodeList NodeLists = doc.SelectNodes("BusDynInfo/BusInfo/Route");

            //Read Node
            foreach (XmlNode Single in NodeLists)
            {
                comboBox1.Items.Add(Single.Attributes["ID"].Value);
            }
        }
        private void RefEstimate(string Route)
        {
            dataGridView1.Rows.Clear();

            string link = "http://122.146.229.210/xmlbus2/GetEstimateTime.xml?routeIds=" + Route;
            //Load XML
            doc.Load(link);

            //Select Node List
            XmlNodeList NodeLists = doc.SelectNodes("BusDynInfo/BusInfo/Route/EstimateTime");

            //ReadNode
            foreach(XmlNode Single in NodeLists)
            {
                string GoBack = (Single.Attributes["GoBack"].Value == "1") ? "去程" : "返程";
                string StopName = Single.Attributes["StopName"].Value;
                string Value = (Single.Attributes["Value"].Value == "null") ? "未發車" : Single.Attributes["Value"].Value + " 分鐘";
                DateTime dt = DateTime.Now;
                dt = dt.AddMinutes((Single.Attributes["Value"].Value == "null") ?0:Convert.ToDouble(Single.Attributes["Value"].Value));

                string ArriveTime = (Single.Attributes["Value"].Value == "null") ? "未發車" : dt.ToString("HH : mm") ;             
                string CarID = (Single.Attributes["carId"].Value == "") ? "未發車" : Single.Attributes["carId"].Value;

                dataGridView1.Rows.Add(GoBack,StopName,Value,ArriveTime,CarID);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Load XML
            doc.Load("http://122.146.229.210/xmlbus2/StaticData/GetRoute.xml");

            //Select Node List
            XmlNodeList NodeLists = doc.SelectNodes("BusDynInfo/BusInfo/Route");

            //Read Node
            foreach (XmlNode Single in NodeLists)
            {
                if (Single.Attributes["ID"].Value == comboBox1.Text)
                {
                    label5.Text = Single.Attributes["ddesc"].Value;
                    label6.Text = Single.Attributes["departureZh"].Value;
                    label7.Text = Single.Attributes["destinationZh"].Value;
                }
            }
            RefEstimate(comboBox1.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RefEstimate(comboBox1.Text);
        }
    }

}
