using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using TrainTimeTable.EventArgs;
using TrainTimeTable.Common;
using log4net;
using System.Reflection;
using System.Drawing.Drawing2D;

namespace TrainTimeTable.Control
{
    /// <summary>
    /// ComboBoxDiagramTrainInformationクラス
    /// </summary>
    public class ComboBoxDiagramTrainInformation : ComboBox
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        private static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ComboBoxDiagramTrainInformation()
            : base()
        {
            DropDownStyle = ComboBoxStyle.DropDownList;

            BeginUpdate();

            Initialization();

            EndUpdate();
        }

        private void Initialization()
        {
            Items.Clear();

            for (DiagramTrainInformation i = DiagramTrainInformation.DisplayIfItIsTheFirstTrain; i <= DiagramTrainInformation.DoNotShow; i++)
            {
                Items.Add(i.GetStringValue());
            }

            if (Items.Count > 0)
            {
                SelectedIndex = 0;
            }
        }

        public DiagramTrainInformation GetSelected()
        {
            if (SelectedIndex < 0)
            {
                return DiagramTrainInformation.DoNotShow;
            }
            switch(Items[SelectedIndex].ToString())
            {
                case "始発なら表示":
                    return DiagramTrainInformation.DisplayIfItIsTheFirstTrain;
                case "常に表示":
                    return DiagramTrainInformation.AlwaysVisible;
                case "表示しない":
                    return DiagramTrainInformation.DoNotShow;
                default:
                    return DiagramTrainInformation.None;
            }
        }

        public void SetSelected(DiagramTrainInformation info)
        {
            int index = FindString(info.GetStringValue());
            if (index >= 0)
            {
                SelectedIndex = index;
            }
        }
    }
}
