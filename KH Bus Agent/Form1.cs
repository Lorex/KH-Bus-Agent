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
using System.IO;

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
        private void RefEstimate(string Route,bool History,bool AutoRef10s)
        {
            if (comboBox1.Text == "")
                return;

            string link = "http://122.146.229.210/xmlbus2/GetEstimateTime.xml?routeIds=" + Route;
            //Load XML
            doc.Load(link);

            //Select Node List
            XmlNodeList NodeLists = doc.SelectNodes("BusDynInfo/BusInfo/Route/EstimateTime");
            
            if (History == false)
                dataGridView1.Rows.Clear();

            //ReadNode
            foreach(XmlNode Single in NodeLists)
            {
                string GoBack = (Single.Attributes["GoBack"].Value == "1") ? "去程" : "返程";
                string StopName = Single.Attributes["StopName"].Value;
                string Value = Single.Attributes["Value"].Value;
                string CarID = (Single.Attributes["carId"].Value == "") ? "未發車" : Single.Attributes["carId"].Value;

                DateTime dt = DateTime.Now;
                dt = dt.AddMinutes((Single.Attributes["Value"].Value == "null") ? 0 : Convert.ToDouble(Single.Attributes["Value"].Value));

                string ArriveTime = (Single.Attributes["Value"].Value == "null") ? "未發車" : dt.ToString("HH : mm");

                switch(Value)
                {
                    case "null":
                        Value = "未發車";
                        break;
                    case "0":
                        Value = "進站中";
                        if(!AutoRef10s)
                            dataGridView2.Rows.Add(Route,GoBack, StopName, ArriveTime, CarID);
                        break;
                    default :
                        Value = Value + " 分鐘";
                        break;

                }
                if(History == false)
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
            RefEstimate(comboBox1.Text,false,false);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            RefEstimate(comboBox1.Text,false,true);
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            foreach(string line in comboBox1.Items)
            {
                RefEstimate(line, true,false);
            }
            timer1.Enabled = true;
        }

        private void dataGridView2_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            dataGridView2.FirstDisplayedScrollingRowIndex = dataGridView2.Rows.Count - 1;
            XMLWrite();
        }

        private void XMLWrite()
        {
            XmlDocument doc = new XmlDocument();

            //check if file exists
            string path = @"D:\Log";
            string filename = DateTime.Now.ToString("yyyy-MM-dd") + ".xml";
            if (!System.IO.File.Exists(System.IO.Path.Combine(path, filename)))
            {
                //root node
                XmlElement root = doc.CreateElement("CarHistory");
                doc.AppendChild(root);

                // secondary node
                XmlElement sec = doc.CreateElement("List");
                sec.SetAttribute("Date",DateTime.Now.ToString("yyyyy-MM-dd"));
                root.AppendChild(sec);

                //create file
                if (!System.IO.Directory.Exists(path))
                    System.IO.Directory.CreateDirectory(path);
                System.IO.File.Create(filename);
                doc.Save(System.IO.Path.Combine(path, filename));
            }

            //load xml
            doc.Load(System.IO.Path.Combine(path, filename));
            XmlNode node = doc.SelectSingleNode("CarHistory/List");
            if (node == null)
                return;

            //create node
            XmlElement main = doc.CreateElement("History");
            main.SetAttribute("Line", dataGridView2[0, dataGridView2.Rows.Count - 1].Value.ToString());
            main.SetAttribute("GoBack", dataGridView2[1, dataGridView2.Rows.Count - 1].Value.ToString());
            main.SetAttribute("StopName", dataGridView2[2, dataGridView2.Rows.Count - 1].Value.ToString());
            main.SetAttribute("Time", dataGridView2[3, dataGridView2.Rows.Count - 1].Value.ToString());
            main.SetAttribute("CarID", dataGridView2[4, dataGridView2.Rows.Count - 1].Value.ToString());
            node.AppendChild(main);
            doc.Save(System.IO.Path.Combine(path, filename));
        }
    }

}