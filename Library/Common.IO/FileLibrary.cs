using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
    }
}
