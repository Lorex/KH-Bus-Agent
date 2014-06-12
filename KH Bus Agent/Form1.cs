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
            // Load XML Document
            XmlDocument doc = new XmlDocument();
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
    }

}
