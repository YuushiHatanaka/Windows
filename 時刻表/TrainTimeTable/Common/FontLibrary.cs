using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainTimeTable.Common
{
    /// <summary>
    /// FontLibraryクラス
    /// </summary>
    public class FontLibrary
    {
        /// <summary>
        /// 使用可否
        /// </summary>
        /// <param name="fontName"></param>
        /// <returns></returns>
        public static bool Usability(string fontName)
        {
            // 使用できるFontFamily配列を取得
            FontFamily[] fontFamily = FontFamily.Families;

            // 条件に一致するフォント名一覧を取得
            List<FontFamily> result = fontFamily.Where(el => el.Name == fontName).ToList();

            // 条件返却
            return result.Any();
        }
    }
}
