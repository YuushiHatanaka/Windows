using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Common.IO
{
    /// <summary>
    /// FileLibraryクラス
    /// </summary>
    public class FileLibrary
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        private static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        [DllImport("shlwapi.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr PathCombine(
                [Out] StringBuilder lpszDest,
                string lpszDir,
                string lpszFile);

        [DllImport("shlwapi.dll", CharSet = CharSet.Auto)]
        private static extern bool PathRelativePathTo(
                [Out] StringBuilder pszPath,
                [In] string pszFrom,
                [In] System.IO.FileAttributes dwAttrFrom,
                [In] string pszTo,
                [In] System.IO.FileAttributes dwAttrTo);

        /// <summary>
        /// ファイル作成(日時更新)
        /// </summary>
        /// <param name="path"></param>
        public static void Touch(string path)
        {
            // ロギング
            Logger.Debug("=>>>> FileLibrary::Touch(string)");
            Logger.DebugFormat("path:[{0}]", path);

            // 存在判定
            if (!File.Exists(path))
            {
                // ロギング
                Logger.DebugFormat("ファイルなし:[{0}]", path);

                // ファイル作成
                File.Create(path).Close();
            }
            else
            {
                // ロギング
                Logger.DebugFormat("ファイルあり:[{0}]", path);

                // 更新日時の設定
                File.SetLastWriteTime(path, DateTime.Now);
                
                // アクセス日時の設定
                File.SetLastAccessTime(path, DateTime.Now);
            }

            // ロギング
            Logger.Debug("<<<<= FileLibrary::Touch(string)");
        }

        /// <summary>
        /// ファイル削除
        /// </summary>
        /// <param name="path"></param>
        public static void Remove(string path)
        {
            // ロギング
            Logger.Debug("=>>>> FileLibrary::Remove(string)");
            Logger.DebugFormat("path:[{0}]", path);

            // 存在判定
            if (File.Exists(path))
            {
                // ロギング
                Logger.DebugFormat("ファイルあり:[{0}]", path);

                // ファイル削除
                File.Delete(path);
            }

            // ロギング
            Logger.Debug("<<<<= FileLibrary::Remove(string)");
        }

        /// <summary>
        /// 相対パスから絶対パスを取得します。
        /// </summary>
        /// <param name="basePath">基準とするパス。</param>
        /// <param name="relativePath">相対パス</param>
        /// <returns>絶対パス</returns>
        public static string GetAbsolutePath(string basePath, string relativePath)
        {
            StringBuilder sb = new StringBuilder();
            IntPtr res = PathCombine(sb, basePath, relativePath);
            if (res == IntPtr.Zero)
            {
                throw new Exception("絶対パスの取得に失敗しました。");
            }
            return sb.ToString();
        }

        /// <summary>
        /// 絶対パスから相対パスを取得します。
        /// </summary>
        /// <param name="basePath">基準とするフォルダのパス。</param>
        /// <param name="absolutePath">絶対パス</param>
        /// <returns>相対パス</returns>
        public static string GetRelativePath(string basePath, string absolutePath)
        {
            StringBuilder sb = new StringBuilder();
            bool res = PathRelativePathTo(sb,
                basePath, System.IO.FileAttributes.Directory,
                absolutePath, System.IO.FileAttributes.Normal);
            if (!res)
            {
                throw new Exception("相対パスの取得に失敗しました。");
            }
            return sb.ToString();
        }
    }
}
