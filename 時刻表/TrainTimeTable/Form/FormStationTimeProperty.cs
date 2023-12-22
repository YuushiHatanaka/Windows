using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrainTimeTable.Property;

namespace TrainTimeTable
{
    public partial class FormStationTimeProperty : Form
    {
        public StationTimeProperty Property { get; set; } = new StationTimeProperty();

        public FormStationTimeProperty(Property.StationTimeProperty stationTimeProperty)
        {
            InitializeComponent();
        }

        private void FormStationTimeProperty_Load(object sender, System.EventArgs e)
        {
            tableLayoutPanelMain.Dock = DockStyle.Fill;
            tableLayoutPanelButton.Dock = DockStyle.Fill;
            buttonOK.Dock = DockStyle.Fill;
            buttonCancel.Dock = DockStyle.Fill;
        }

        private void buttonOK_Click(object sender, System.EventArgs e)
        {
            // 正常終了
            DialogResult = DialogResult.OK;
        }

        private void buttonCancel_Click(object sender, System.EventArgs e)
        {
            // キャンセル
            DialogResult = DialogResult.Cancel;
        }
    }
}
