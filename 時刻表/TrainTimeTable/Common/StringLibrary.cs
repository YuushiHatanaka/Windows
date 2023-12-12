using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainTimeTable.Common
{
    /// <summary>
    /// StringLibraryクラス
    /// </summary>
    public class StringLibrary
    {
        /// <summary>
        /// 縦文字作成
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string VerticalText(string text)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var c in text)
            {
                if (c.ToString() == "ー")
                {
                    sb.AppendLine("｜");
                }
                else
                {
                    sb.AppendLine(c.ToString());
                }
            }
            return sb.ToString();
        }
    }
}
