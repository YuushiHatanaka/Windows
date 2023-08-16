using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Common.Font.DiaPro.ControlTest
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            tableLayoutPanel1.Dock = DockStyle.Fill;
            listView1.Dock = DockStyle.Fill;
            listView2.Dock = DockStyle.Fill;

            DiaProFont diaProFont1 = new DiaProFont();
            listView1.Font = diaProFont1.GetFont(@"Font\DiaPro-Regular.ttf", 10);

            foreach (string value in diaProFont1.Keys)
            {
                ListViewItem listViewItem = new ListViewItem();
                listViewItem.Text = value;
                listViewItem.SubItems.Add(diaProFont1[value]);
                listView1.Items.Add(listViewItem);
            }

            DiaProFont diaProFont2 = new DiaProFont();
            listView2.Font = diaProFont2.GetFont(@"Font\DiaPro-Bold.ttf", 10);

            foreach (string value in diaProFont2.Keys)
            {
                ListViewItem listViewItem = new ListViewItem();
                listViewItem.Text = value;
                listViewItem.SubItems.Add(diaProFont2[value]);
                listView2.Items.Add(listViewItem);
            }
        }
    }
}
