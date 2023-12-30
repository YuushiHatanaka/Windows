using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TrainTimeTable.Common
{
    /// <summary>
    /// StationDrawRowInfomationクラス
    /// </summary>
    public class StationDrawRowInfomation
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        private static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        /// <summary>
        /// 駅名
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 発着種別
        /// </summary>
        public DepartureArrivalType DepartureArrivalType { get; set; } = DepartureArrivalType.None;

        /// <summary>
        /// 距離文字列
        /// </summary>
        public string Distance {get; set; } = string.Empty;

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public StationDrawRowInfomation()
        {
            // ロギング
            Logger.Debug("=>>>> StationDrawRowInfomation::StationDrawRowInfomation()");

            // ロギング
            Logger.Debug("<<<<= StationDrawRowInfomation::StationDrawRowInfomation()");
        }
        #endregion

        #region 文字列化
        /// <summary>
        /// 文字列化
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            // 文字列化返却
            return ToString(0);
        }

        /// <summary>
        /// 文字列化
        /// </summary>
        /// <param name="indent"></param>
        /// <returns></returns>
        public string ToString(int indent)
        {
            // 結果オブジェクト生成
            StringBuilder result = new StringBuilder();

            // インデント生成
            string indentstr = new string('　', indent);

            // 文字列追加
            result.AppendLine(indentstr + string.Format("＜駅描画情報＞"));
            result.AppendLine(indentstr + string.Format("　駅名    :[{0}] ", Name));
            result.AppendLine(indentstr + string.Format("　発着種別:[{0}] ", DepartureArrivalType.GetStringValue()));

            // 返却
            return result.ToString();
        }
        #endregion
    }
}
