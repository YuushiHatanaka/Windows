using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainTimeTable.Common
{
    public class StationDrawRowInfomation
    {
        public string Name { get; set; } = string.Empty;
        public DepartureArrivalType DepartureArrivalType { get; set; } = DepartureArrivalType.None;

        public string Distance {get; set; } = string.Empty;

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
