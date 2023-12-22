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
    public partial class FormTrainProperty : Form
    {
        public TrainProperty Property { get; set; } = new TrainProperty();

        public FormTrainProperty(Property.TrainProperty property)
        {
            InitializeComponent();
        }

        private void FormTrainProperty_Load(object sender, System.EventArgs e)
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
