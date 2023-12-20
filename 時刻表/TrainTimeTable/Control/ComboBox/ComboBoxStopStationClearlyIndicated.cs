﻿using System;
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
using System.Xml.Linq;

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

        public StopMarkDrawType GetSelected()
        {
            if (SelectedIndex < 0)
            {
                return StopMarkDrawType.None;
            }
            switch (Items[SelectedIndex].ToString())
            {
                case "明示しない":
                    return StopMarkDrawType.Nothing;
                case "停車駅を明示":
                    return StopMarkDrawType.DrawOnStop;
                default:
                    return StopMarkDrawType.None;
            }
        }

        public void SetSelected(StopMarkDrawType type)
        {
            int index = FindString(type.GetStringValue());
            if (index >= 0)
            {
                SelectedIndex = index;
            }
        }
    }
}
