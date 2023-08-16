using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Text;

namespace Common.Drawing
{
    /// <summary>
    /// FontLibraryクラス
    /// </summary>
    public class FontLibrary
    {
        /// <summary>
        /// システムフォント名一覧取得
        /// </summary>
        /// <returns></returns>
        public static List<string> GetSystemFont()
        {
            List<string> result = new List<string>();
            InstalledFontCollection fonts = new InstalledFontCollection();
            FontFamily[] ffArray = fonts.Families;
            foreach (FontFamily ff in ffArray)
            {
                result.Add(ff.Name);
            }
            return result;
        }
    }
}
