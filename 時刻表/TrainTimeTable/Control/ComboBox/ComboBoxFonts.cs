using log4net;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrainTimeTable.Common;
using TrainTimeTable.Property;

namespace TrainTimeTable.Control
{
    /// <summary>
    /// ComboBoxFontsクラス
    /// </summary>
    public class ComboBoxFonts : ComboBox
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
        /// <param name="fonts"></param>
        public ComboBoxFonts(FontProperties fonts)
            : base()
        {
            DropDownStyle = ComboBoxStyle.DropDownList;

            BeginUpdate();

            Initialization(fonts);

            EndUpdate();
        }

        private void Initialization(FontProperties fonts)
        {
            Items.Clear();

            foreach (var name in fonts.Keys)
            {
                Items.Add(name);
            }

            if (Items.Count > 0)
            {
                SelectedIndex = 0;
            }
        }

        public string GetSelected()
        {
            if (SelectedIndex < 0)
            {
                return "";
            }
            return Items[SelectedIndex].ToString();
        }

        public void SetSelected(string name)
        {
            int index = FindString(name);
            if (index >= 0)
            {
                SelectedIndex = index;
            }
        }
    }
}
