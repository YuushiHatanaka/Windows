using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Text;
using log4net.Repository.Hierarchy;

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

        /// <summary>
        /// 文字Pixelサイズ取得
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="font"></param>
        /// <returns></returns>
        public static Size GetStringPixelSize(string txt, Font font)
        {
            // 結果オブジェクト生成
            Size result = new Size();

            // 仮想の描画オブジェクトを作成
            using (Graphics graphics = Graphics.FromImage(new Bitmap(1, 1)))
            {
                // テキストのサイズを計算
                SizeF textSize = graphics.MeasureString(txt, font);

                // ピクセル幅を取得
                result.Width = (int)Math.Ceiling(textSize.Width);

                // ピクセル高さを取得
                result.Height = (int)Math.Ceiling(textSize.Height);
            }

            // 返却
            return result;
        }
    }
}
