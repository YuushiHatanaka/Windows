using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrainTimeTable.Common;

namespace TrainTimeTable.Control
{
    /// <summary>
    /// ProgressBarInfomationクラス
    /// </summary>
    public class ProgressBarInfomation
    {
        /// <summary>
        /// プログレスバー表示位置
        /// </summary>
        public int Position = 0;

        /// <summary>
        /// 表示メッセージ
        /// </summary>
        public string Message = string.Empty;

        /// <summary>
        /// 処理結果
        /// </summary>
        public DialogResult Result = DialogResult.None;

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
            result.AppendLine(indentstr + string.Format("＜プログレスバー表示情報＞"));
            result.AppendLine(indentstr + string.Format("　プログレスバー表示位置:[{0}] ", Position));
            result.AppendLine(indentstr + string.Format("　表示メッセージ        :[{0}] ", Message));
            result.AppendLine(indentstr + string.Format("　処理結果              :[{0}] ", Result));

            // 返却
            return result.ToString();
        }
        #endregion
    }
}
