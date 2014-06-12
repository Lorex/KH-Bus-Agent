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
        private void RefEstimate()
        {

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
        }
    }

}
