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

namespace TrainTimeTable.Control
{
    /// <summary>
    /// ComboBoxStopStationClearlyIndicatedクラス
    /// </summary>
    public class ComboBoxStopStationClearlyIndicated : ComboBox
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
        public ComboBoxStopStationClearlyIndicated()
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

            for (StopMarkDrawType i = StopMarkDrawType.Nothing; i <= StopMarkDrawType.DrawOnStop; i++)
            {
                Items.Add(i.GetStringValue());
            }

            if (Items.Count > 0)
            {
                SelectedIndex = 0;
            }
        }
    }
}
