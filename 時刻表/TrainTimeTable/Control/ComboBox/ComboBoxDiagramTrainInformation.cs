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

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ComboBoxDiagramTrainInformation()
            : base()
        {
            // ロギング
            Logger.Debug("=>>>> ComboBoxDiagramTrainInformation::ComboBoxDiagramTrainInformation()");

            // 設定
            DropDownStyle = ComboBoxStyle.DropDownList;

            // 更新開始
            BeginUpdate();

            // 初期化
            Initialization();

            // 更新終了
            EndUpdate();

            // ロギング
            Logger.Debug("<<<<= ComboBoxDiagramTrainInformation::ComboBoxDiagramTrainInformation()");
        }
        #endregion

        #region privateメソッド
        /// <summary>
        ///  初期化
        /// </summary>
        private void Initialization()
        {
            // ロギング
            Logger.Debug("=>>>> ComboBoxDiagramTrainInformation::Initialization()");

            // リストを全てクリア
            Items.Clear();

            // DiagramTrainInformationを繰り返す
            for (DiagramTrainInformation i = DiagramTrainInformation.DisplayIfItIsTheFirstTrain; i <= DiagramTrainInformation.DoNotShow; i++)
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
            Logger.Debug("<<<<= ComboBoxDiagramTrainInformation::Initialization()");
        }
        #endregion

        #region publicメソッド
        /// <summary>
        /// 選択要素取得
        /// </summary>
        /// <returns></returns>
        public DiagramTrainInformation GetSelected()
        {
            // ロギング
            Logger.Debug("=>>>> ComboBoxDiagramTrainInformation::GetSelected()");

            // 結果初期化
            DiagramTrainInformation result = DiagramTrainInformation.None;

            // 選択インデックス判定
            if (SelectedIndex >= 0)
            {
                switch (Items[SelectedIndex].ToString())
                {
                    case "始発なら表示":
                        // 結果設定
                        result = DiagramTrainInformation.DisplayIfItIsTheFirstTrain;
                        break;
                    case "常に表示":
                        // 結果設定
                        result = DiagramTrainInformation.AlwaysVisible;
                        break;
                    case "表示しない":
                        // 結果設定
                        result = DiagramTrainInformation.DoNotShow;
                        break;
                    default:
                        // 結果設定
                        result = DiagramTrainInformation.None;
                        break;
                }
            }

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= ComboBoxDiagramTrainInformation::GetSelected()");

            // 返却
            return result;
        }

        /// <summary>
        /// 選択設定
        /// </summary>
        /// <param name="info"></param>
        public void SetSelected(DiagramTrainInformation info)
        {
            // ロギング
            Logger.Debug("=>>>> ComboBoxDiagramTrainInformation::SetSelected(DiagramTrainInformation)");
            Logger.DebugFormat("info:[{0}]", info.GetStringValue());

            // 文字列検索
            int result = FindString(info.GetStringValue());

            // 文字列検索結果判定
            if (result >= 0)
            {
                // 選択インデックス設定
                SelectedIndex = result;
            }

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= ComboBoxDiagramTrainInformation::DiagramTrainInformation(DashStyle)");
        }
        #endregion
    }
}
