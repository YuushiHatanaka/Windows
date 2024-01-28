using log4net;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TrainTimeTable.Common
{
    /// <summary>
    /// GridDataLocationクラス
    /// </summary>
    [Serializable]
    public class DataGridLocation
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        private readonly static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        /// <summary>
        /// 列
        /// </summary>
        public int Column { get; set; } = -1;

        /// <summary>
        /// 行
        /// </summary>
        public int Row { get; set; } = -1;

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DataGridLocation()
        {
            // ロギング
            Logger.Debug("=>>>> DataGridLocation::DataGridLocation()");

            // ロギング
            Logger.Debug("<<<<= DataGridLocation::DataGridLocation()");
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
            result.AppendLine(indentstr + string.Format("位置情報:[Column={0}][Row={1}] ", Column, Row));

            // 返却
            return result.ToString();
        }
        #endregion
    }
}
