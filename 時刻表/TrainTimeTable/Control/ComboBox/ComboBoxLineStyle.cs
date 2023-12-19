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
using System.Xml.Linq;

namespace TrainTimeTable.Control
{
    /// <summary>
    /// ComboBoxLineStyleクラス
    /// </summary>
    public class ComboBoxLineStyle : ComboBox
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
        public ComboBoxLineStyle()
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

            for (DashStyle i = DashStyle.Solid; i <= DashStyle.DashDotDot; i++)
            {
                Items.Add(new DashStyleInfo(i));
            }

            if (Items.Count > 0)
            {
                SelectedIndex = 0;
            }
        }

        public DashStyle GetSelected()
        {
            if (SelectedIndex < 0)
            {
                return DashStyle.Solid;
            }
            return ((DashStyleInfo)(Items[SelectedIndex])).Style;
        }

        public void SetSelected(DashStyle style)
        {
            int index = FindString(new DashStyleInfo(style).ToString());
            if (index >= 0)
            {
                SelectedIndex = index;
            }
        }
    }
}
