using System.IO;
using System.IO.Compression;

namespace Common.IO.Compression
{
    /// <summary>
    /// Gzip圧縮クラス
    /// </summary>
    public class CGZipLib
    {
        #region 圧縮
        /// <summary>
        /// 圧縮
        /// </summary>
        /// <param name="srcName"></param>
        /// <param name="desName"></param>
        static void Compress(string srcName, string desName)
        {
            // 圧縮
            CGZipLib.Compress(srcName, desName, new byte[1024]);
        }

        /// <summary>
        /// 圧縮
        /// </summary>
        /// <param name="srcName"></param>
        /// <param name="desName"></param>
        /// <param name="buf"></param>
        static void Compress(string srcName, string desName, byte[] buf)
        {
            int num;

            // 入力ストリーム
            FileStream inStream = new FileStream(srcName, FileMode.Open, FileAccess.Read);

            // 出力ストリーム
            FileStream outStream = new FileStream(desName, FileMode.Create);

            // 圧縮ストリーム
            GZipStream compStream = new GZipStream(outStream, CompressionMode.Compress);

            // 圧縮
            using (inStream)
            {
                using (outStream)
                {
                    using (compStream)
                    {
                        while ((num = inStream.Read(buf, 0, buf.Length)) > 0)
                        {
                            compStream.Write(buf, 0, num);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 圧縮
        /// </summary>
        /// <param name="srcName"></param>
        /// <param name="desName"></param>
        static void Compress(byte[] srcName, string desName)
        {
            // 圧縮
            CGZipLib.Compress(srcName, desName, new byte[1024]);
        }

        /// <summary>
        /// 圧縮
        /// </summary>
        /// <param name="srcName"></param>
        /// <param name="desName"></param>
        /// <param name="buf"></param>
        static void Compress(byte[] srcName, string desName, byte[] buf)
        {
            // 圧縮
            CGZipLib.Compress(new MemoryStream(srcName, false), desName, buf);
        }

        /// <summary>
        /// 圧縮
        /// </summary>
        /// <param name="srcName"></param>
        /// <param name="desName"></param>
        static void Compress(MemoryStream srcName, string desName)
        {
            // 圧縮
            CGZipLib.Compress(srcName, desName, new byte[1024]);
        }

        /// <summary>
        /// 圧縮
        /// </summary>
        /// <param name="srcName"></param>
        /// <param name="desName"></param>
        /// <param name="buf"></param>
        static void Compress(MemoryStream srcName, string desName, byte[] buf)
        {
            int num;

            // 入力ストリーム
            MemoryStream inStream = srcName;

            // 出力ストリーム
            FileStream outStream = new FileStream(desName, FileMode.Create);

            // 圧縮ストリーム
            GZipStream compStream = new GZipStream(outStream, CompressionMode.Compress);

            // 圧縮
            using (inStream)
            {
                using (outStream)
                {
                    using (compStream)
                    {
                        while ((num = inStream.Read(buf, 0, buf.Length)) > 0)
                        {
                            compStream.Write(buf, 0, num);
                        }
                    }
                }
            }
        }
        #endregion

        #region 解凍
        /// <summary>
        /// 解凍
        /// </summary>
        /// <param name="srcName"></param>
        /// <param name="desName"></param>
        static void Decompress(string srcName, string desName)
        {
            // 解凍
            CGZipLib.Decompress(srcName, desName, new byte[1024]);
        }

        /// <summary>
        /// 解凍
        /// </summary>
        /// <param name="srcName"></param>
        /// <param name="desName"></param>
        /// <param name="buf"></param>
        static void Decompress(string srcName, string desName, byte[] buf)
        {
            int num;

            // 入力ストリーム
            FileStream inStream = new FileStream(srcName, FileMode.Open, FileAccess.Read);

            // 出力ストリーム
            FileStream outStream = new FileStream(desName, FileMode.Create);

            // 解凍ストリーム
            GZipStream decompStream = new GZipStream(inStream, CompressionMode.Decompress);

            // 解凍
            using (inStream)
            {
                using (outStream)
                {
                    using (decompStream)
                    {
                        while ((num = decompStream.Read(buf, 0, buf.Length)) > 0)
                        {
                            outStream.Write(buf, 0, num);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 解凍
        /// </summary>
        /// <param name="srcName"></param>
        /// <param name="desName"></param>
        static void Decompress(byte[] srcName, string desName)
        {
            // 解凍
            CGZipLib.Decompress(srcName, desName, new byte[1024]);
        }

        /// <summary>
        /// 解凍
        /// </summary>
        /// <param name="srcName"></param>
        /// <param name="desName"></param>
        /// <param name="buf"></param>
        static void Decompress(byte[] srcName, string desName, byte[] buf)
        {
            // 解凍
            CGZipLib.Decompress(new MemoryStream(srcName, false), desName, buf);
        }

        /// <summary>
        /// 解凍
        /// </summary>
        /// <param name="srcName"></param>
        /// <param name="desName"></param>
        static void Decompress(MemoryStream srcName, string desName)
        {
            // 解凍
            CGZipLib.Decompress(srcName, desName, new byte[1024]);
        }

        /// <summary>
        /// 解凍
        /// </summary>
        /// <param name="srcName"></param>
        /// <param name="desName"></param>
        /// <param name="buf"></param>
        static void Decompress(MemoryStream srcName, string desName, byte[] buf)
        {
            int num;

            // 出力ストリーム
            FileStream outStream = new FileStream(desName, FileMode.Create);

            // 解凍ストリーム
            GZipStream decompStream = new GZipStream(srcName, CompressionMode.Decompress);

            // 解凍
            using (srcName)
            {
                using (outStream)
                {
                    using (decompStream)
                    {
                        while ((num = decompStream.Read(buf, 0, buf.Length)) > 0)
                        {
                            outStream.Write(buf, 0, num);
                        }
                    }
                }
            }
        }
        #endregion
    }
}
