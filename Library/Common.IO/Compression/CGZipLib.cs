using log4net;
using System.IO;
using System.IO.Compression;
using System.Reflection;

namespace Common.IO.Compression
{
    /// <summary>
    /// Gzip圧縮クラス
    /// </summary>
    public class CGZipLib
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        protected static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region 圧縮
        /// <summary>
        /// 圧縮
        /// </summary>
        /// <param name="srcName"></param>
        /// <param name="desName"></param>
        public static void Compress(string srcName, string desName)
        {
            // ロギング
            Logger.Debug("=>>>> CGZipLib::Compress(string, string)");
            Logger.DebugFormat("srcName:[{0}]", srcName);
            Logger.DebugFormat("desName:[{0}]", desName);

            // 圧縮
            Compress(srcName, desName, new byte[1024]);

            // ロギング
            Logger.Debug("<<<<= CGZipLib::Compress(string, string)");
        }

        /// <summary>
        /// 圧縮
        /// </summary>
        /// <param name="srcName"></param>
        /// <param name="desName"></param>
        /// <param name="buf"></param>
        public static void Compress(string srcName, string desName, byte[] buf)
        {
            // ロギング
            Logger.Debug("=>>>> CGZipLib::Compress(string, string, byte[])");
            Logger.DebugFormat("srcName:[{0}]", srcName);
            Logger.DebugFormat("desName:[{0}]", desName);
            Logger.DebugFormat("buf    :[{0}]", buf);

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

            // ロギング
            Logger.Debug("<<<<= CGZipLib::Compress(string, string, byte[])");
        }

        /// <summary>
        /// 圧縮
        /// </summary>
        /// <param name="srcName"></param>
        /// <param name="desName"></param>
        public static void Compress(byte[] srcName, string desName)
        {
            // ロギング
            Logger.Debug("=>>>> CGZipLib::Compress(byte[], string)");
            Logger.DebugFormat("srcName:[{0}]", srcName);
            Logger.DebugFormat("desName:[{0}]", desName);

            // 圧縮
            Compress(srcName, desName, new byte[1024]);

            // ロギング
            Logger.Debug("<<<<= CGZipLib::Compress(byte[], string)");
        }

        /// <summary>
        /// 圧縮
        /// </summary>
        /// <param name="srcName"></param>
        /// <param name="desName"></param>
        /// <param name="buf"></param>
        public static void Compress(byte[] srcName, string desName, byte[] buf)
        {
            // ロギング
            Logger.Debug("=>>>> CGZipLib::Compress(byte[], string, byte[])");
            Logger.DebugFormat("srcName:[{0}]", srcName);
            Logger.DebugFormat("desName:[{0}]", desName);

            // 圧縮
            Compress(new MemoryStream(srcName, false), desName, buf);

            // ロギング
            Logger.Debug("<<<<= CGZipLib::Compress(byte[], string, byte[])");
        }

        /// <summary>
        /// 圧縮
        /// </summary>
        /// <param name="srcName"></param>
        /// <param name="desName"></param>
        public static void Compress(MemoryStream srcName, string desName)
        {
            // ロギング
            Logger.Debug("=>>>> CGZipLib::Compress(MemoryStream, string)");
            Logger.DebugFormat("srcName:[{0}]", srcName);
            Logger.DebugFormat("desName:[{0}]", desName);

            // 圧縮
            Compress(srcName, desName, new byte[1024]);

            // ロギング
            Logger.Debug("<<<<= CGZipLib::Compress(MemoryStream, string)");
        }

        /// <summary>
        /// 圧縮
        /// </summary>
        /// <param name="srcName"></param>
        /// <param name="desName"></param>
        /// <param name="buf"></param>
        public static void Compress(MemoryStream srcName, string desName, byte[] buf)
        {
            // ロギング
            Logger.Debug("=>>>> CGZipLib::Compress(MemoryStream, string, byte[])");
            Logger.DebugFormat("srcName:[{0}]", srcName);
            Logger.DebugFormat("desName:[{0}]", desName);

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

            // ロギング
            Logger.Debug("<<<<= CGZipLib::Compress(MemoryStream, string, byte[])");
        }
        #endregion

        #region 解凍
        /// <summary>
        /// 解凍
        /// </summary>
        /// <param name="srcName"></param>
        /// <param name="desName"></param>
        public static void Decompress(string srcName, string desName)
        {
            // ロギング
            Logger.Debug("=>>>> CGZipLib::Decompress(string, string)");
            Logger.DebugFormat("srcName:[{0}]", srcName);
            Logger.DebugFormat("desName:[{0}]", desName);

            // 解凍
            Decompress(srcName, desName, new byte[1024]);

            // ロギング
            Logger.Debug("<<<<= CGZipLib::Decompress(string, string)");
        }

        /// <summary>
        /// 解凍
        /// </summary>
        /// <param name="srcName"></param>
        /// <param name="desName"></param>
        /// <param name="buf"></param>
        public static void Decompress(string srcName, string desName, byte[] buf)
        {
            // ロギング
            Logger.Debug("=>>>> CGZipLib::Decompress(string, string, byte[])");
            Logger.DebugFormat("srcName:[{0}]", srcName);
            Logger.DebugFormat("desName:[{0}]", desName);
            Logger.DebugFormat("buf    :[{0}]", buf);

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

            // ロギング
            Logger.Debug("<<<<= CGZipLib::Decompress(string, string, byte[])");
        }

        /// <summary>
        /// 解凍
        /// </summary>
        /// <param name="srcName"></param>
        /// <param name="desName"></param>
        public static void Decompress(byte[] srcName, string desName)
        {
            // ロギング
            Logger.Debug("=>>>> CGZipLib::Decompress(byte[], string)");
            Logger.DebugFormat("srcName:[{0}]", srcName);
            Logger.DebugFormat("desName:[{0}]", desName);

            // 解凍
            Decompress(srcName, desName, new byte[1024]);

            // ロギング
            Logger.Debug("<<<<= CGZipLib::Decompress(byte[], string)");
        }

        /// <summary>
        /// 解凍
        /// </summary>
        /// <param name="srcName"></param>
        /// <param name="desName"></param>
        /// <param name="buf"></param>
        public static void Decompress(byte[] srcName, string desName, byte[] buf)
        {
            // ロギング
            Logger.Debug("=>>>> CGZipLib::Decompress(byte[], string, byte[])");
            Logger.DebugFormat("srcName:[{0}]", srcName);
            Logger.DebugFormat("desName:[{0}]", desName);
            Logger.DebugFormat("buf    :[{0}]", buf);

            // 解凍
            Decompress(new MemoryStream(srcName, false), desName, buf);

            // ロギング
            Logger.Debug("<<<<= CGZipLib::Decompress(byte[], string, byte[])");
        }

        /// <summary>
        /// 解凍
        /// </summary>
        /// <param name="srcName"></param>
        /// <param name="desName"></param>
        public static void Decompress(MemoryStream srcName, string desName)
        {
            // ロギング
            Logger.Debug("=>>>> CGZipLib::Decompress(MemoryStream, string)");
            Logger.DebugFormat("srcName:[{0}]", srcName);
            Logger.DebugFormat("desName:[{0}]", desName);

            // 解凍
            Decompress(srcName, desName, new byte[1024]);

            // ロギング
            Logger.Debug("<<<<= CGZipLib::Decompress(MemoryStream, string)");
        }

        /// <summary>
        /// 解凍
        /// </summary>
        /// <param name="srcName"></param>
        /// <param name="desName"></param>
        /// <param name="buf"></param>
        public static void Decompress(MemoryStream srcName, string desName, byte[] buf)
        {
            // ロギング
            Logger.Debug("=>>>> CGZipLib::Decompress(MemoryStream, string, byte[])");
            Logger.DebugFormat("srcName:[{0}]", srcName);
            Logger.DebugFormat("desName:[{0}]", desName);

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

            // ロギング
            Logger.Debug("<<<<= CGZipLib::Decompress(MemoryStream, string, byte[])");
        }
        #endregion
    }
}
