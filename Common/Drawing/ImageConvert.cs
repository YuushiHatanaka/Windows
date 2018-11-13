using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.Drawing;

namespace Common.Drawing
{
    /// <summary>
    /// 画像処理クラス
    /// </summary>
    public class ImageConvert
    {
        /// <summary>
        /// バイト配列をImageオブジェクトに変換
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Image ByteArrayToImage(byte[] b)
        {
            Trace.WriteLine("=>>>> ImageConvert::ByteArrayToImage(byte[])");

            if (b.Length > 0)
            {
                ImageConverter imgconv = new ImageConverter();
                Image img = (Image)imgconv.ConvertFrom(b);
                Trace.WriteLine("<<<<= ImageConvert::ByteArrayToImage(byte[])");
                return img;
            }

            Trace.WriteLine("<<<<= ImageConvert::ByteArrayToImage(byte[]) = null");
            return null;
        }

        /// <summary>
        /// Imageオブジェクトをバイト配列に変換
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public static byte[] ImageToByteArray(Image img)
        {
            Trace.WriteLine("=>>>> ImageConvert::ImageToByteArray(Image)");

            if (img != null)
            {
                ImageConverter imgconv = new ImageConverter();
                byte[] b = (byte[])imgconv.ConvertTo(img, typeof(byte[]));
                Trace.WriteLine("<<<<= ImageConvert::ImageToByteArray(Image)");
                return b;
            }

            Trace.WriteLine("<<<<= ImageConvert::ImageToByteArray(Image) = null");
            return null;
        }
    }
}
