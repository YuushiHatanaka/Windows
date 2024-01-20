using log4net;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using TrainTimeTable.Common;
using TrainTimeTable.Property;

namespace TrainTimeTable.File.Core
{
    /// <summary>
    /// FileCoreクラス
    /// </summary>
    public class FileCore
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        protected static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region ファイル名
        /// <summary>
        /// ファイル名
        /// </summary>
        public string FileName { get; set; }
        #endregion

        #region RouteFilePropertyオブジェクト
        /// <summary>
        /// RouteFilePropertyオブジェクト
        /// </summary>
        protected RouteFileProperty m_RouteFileProperty = new RouteFileProperty();
        #endregion

        #region 処理中セクション
        /// <summary>
        /// 処理中セクション
        /// </summary>
        protected string m_CurrentSection = string.Empty;
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fileName"></param>
        public FileCore(string fileName)
        {
            // ロギング
            Logger.Debug("=>>>> FileCore::FileCore(string)");
            Logger.DebugFormat("fileName:[{0}]", fileName);

            // 設定
            FileName = fileName;

            // フォントデフォルト設定
            m_RouteFileProperty.Fonts = FontProperties.GetDefault();

            // 色デフォルト設定
            m_RouteFileProperty.Colors = ColorProperties.GetDefault();

            // ロギング
            Logger.Debug("<<<<= FileCore::FileCore(string)");
        }
        #endregion

        #region 読込
        /// <summary>
        /// 読込
        /// </summary>
        public virtual RouteFileProperty Load()
        {
            // ロギング
            Logger.Debug("=>>>> FileCore::Load()");

            // 全行読込
            List<string> lines = System.IO.File.ReadLines(FileName, Encoding.GetEncoding("shift_jis")).ToList();

            // 文字列
            StringBuilder sb = new StringBuilder();

            // 事前処理
            PreProcessing();

            // 1行ずつ処理する
            int lineNo = 0;
            foreach (var line in lines)
            {
                // 行数加算
                lineNo++;

                // 文字列長判定
                if (line.Length == 0)
                {
                    // 行を処理しない
                    continue;
                }

                // 制御文字変換
                string line_value = Regex.Replace(line, "^[ \\t]+", "");

                // 最終文字判定
                if (line_value.Substring(line_value.Length - 1, 1) == @"\")
                {
                    // 文字列追加
                    sb.Append(line_value.Replace(@"\", ""));
                    continue;
                }

                // 文字列追加
                sb.Append(line_value);

                // 1行処理
                if (!ProcessOneLineAtTime(sb.ToString()))
                {
                    // 例外
                    throw new ApplicationException(string.Format("読込処理に失敗しました:[{0}][{1}行目]", FileName, lineNo));
                }

                // 文字列クリア
                sb.Clear();
            }

            // 事後処理
            PostProcess();

            // ロギング
            Logger.DebugFormat("result:[{0}]", m_RouteFileProperty);
            Logger.Debug("<<<<= FileCore::Load()");

            // 返却
            return m_RouteFileProperty;
        }

        /// <summary>
        /// 事前処理
        /// </summary>
        private void PreProcessing()
        {
            // ロギング
            Logger.Debug("=>>>> FileCore::PreProcessing()");

            // ロギング
            Logger.Debug("<<<<= FileCore::PreProcessing()");
        }

        /// <summary>
        ///  事後処理
        /// </summary>
        private void PostProcess()
        {
            // ロギング
            Logger.Debug("=>>>> FileCore::PostProcess()");

            // 次駅設定
            m_RouteFileProperty.Stations.SetNextStation();

            // 起点駅設定
            m_RouteFileProperty.Stations[0].StartingStation = true;
            m_RouteFileProperty.Stations[0].TimeFormat = TimeFormat.InboundArrivalTime;

            // 終点駅設定
            m_RouteFileProperty.Stations[m_RouteFileProperty.Stations.Count - 1].TerminalStation = true;

            // 最終駅は下り着駅にする
            m_RouteFileProperty.Stations[m_RouteFileProperty.Stations.Count - 1].TimeFormat = TimeFormat.OutboundArrivalTime;

            // 列車種別(データなし削除)
            m_RouteFileProperty.TrainTypesRemoveNoData();

            // 発着駅設定
            m_RouteFileProperty.Diagrams.DepartureArrivalStationSetting();

            // ロギング
            Logger.Debug("<<<<= FileCore::PostProcess()");
        }

        /// <summary>
        /// 1行処理
        /// </summary>
        /// <param name="line"></param>
        protected virtual bool ProcessOneLineAtTime(string line)
        {
            // ロギング
            Logger.Debug("=>>>> FileCore::ProcessOneLineAtTime(string)");
            Logger.DebugFormat("line:[{0}]", line);

            // ロギング
            Logger.Debug("<<<<= FileCore::ProcessOneLineAtTime(string)");

            // 正常終了
            return true;
        }
        #endregion
    }
}
