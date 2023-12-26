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

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ComboBoxStopStationClearlyIndicated()
            : base()
        {
            // ロギング
            Logger.Debug("=>>>> ComboBoxStopStationClearlyIndicated::ComboBoxStopStationClearlyIndicated()");

            // 設定
            DropDownStyle = ComboBoxStyle.DropDownList;

            // 更新開始
            BeginUpdate();

            // 初期化
            Initialization();

            // 更新終了
            EndUpdate();

            // ロギング
            Logger.Debug("<<<<= ComboBoxStopStationClearlyIndicated::ComboBoxStopStationClearlyIndicated()");
        }
        #endregion

        #region privateメソッド
        /// <summary>
        ///  初期化
        /// </summary>
        private void Initialization()
        {
            // ロギング
            Logger.Debug("=>>>> ComboBoxStopStationClearlyIndicated::Initialization()");

            // リストを全てクリア
            Items.Clear();

            // StopMarkDrawTypeを繰り返す
            for (StopMarkDrawType i = StopMarkDrawType.Nothing; i <= StopMarkDrawType.DrawOnStop; i++)
            {
                // 登録
                Items.Add(i.GetStringValue());
            }

            // 登録数判定
            if (Items.Count > 0)
            {
                // 選択インデックス初期化
                SelectedIndex = 0;
            }

            // ロギング
            Logger.Debug("<<<<= ComboBoxStopStationClearlyIndicated::Initialization()");
        }
        #endregion

        #region publicメソッド
        /// <summary>
        /// 選択要素取得
        /// </summary>
        /// <returns></returns>
        public StopMarkDrawType GetSelected()
        {
            // ロギング
            Logger.Debug("=>>>> ComboBoxStopStationClearlyIndicated::GetSelected()");

            // 結果初期化
            StopMarkDrawType result = StopMarkDrawType.None;

            // 選択インデックス判定
            if (SelectedIndex >= 0)
            {
                // 選択文字列で分岐する
                switch (Items[SelectedIndex].ToString())
                {
                    case "明示しない":
                        // 結果設定
                        result = StopMarkDrawType.Nothing;
                        break;
                    case "停車駅を明示":
                        // 結果設定
                        result = StopMarkDrawType.DrawOnStop;
                        break;
                    default:
                        break;
                }
            }

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= ComboBoxStopStationClearlyIndicated::GetSelected()");

            // 返却
            return result;
        }

        /// <summary>
        /// 選択設定
        /// </summary>
        /// <param name="type"></param>
        public void SetSelected(StopMarkDrawType type)
        {
            // ロギング
            Logger.Debug("=>>>> ComboBoxStopStationClearlyIndicated::SetSelected(StopMarkDrawType)");
            Logger.DebugFormat("type:[{0}]", type.GetStringValue());

            // 文字列検索
            int result = FindString(type.GetStringValue());

            // 文字列検索結果判定
            if (result >= 0)
            {
                // 選択インデックス設定
                SelectedIndex = result;
            }

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= ComboBoxStopStationClearlyIndicated::SetSelected(StopMarkDrawType)");
        }
        #endregion
    }
}
