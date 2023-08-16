using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using log4net;
using System.Reflection;

namespace Common.IO
{
    /// <summary>
    /// DirectoryLibraryクラス
    /// </summary>
    public class DirectoryLibrary
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        private static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        /// <summary>
        /// ディレクトリ作成
        /// </summary>
        /// <param name="path"></param>
        public static void Create(string path)
        {
            // ロギング
            Logger.Debug("=>>>> DirectoryLibrary::Create(string)");
            Logger.DebugFormat("path:[{0}]", path);

            // ディレクトリ存在判定
            if (!Directory.Exists(path))
            {
                // ロギング
                Logger.DebugFormat("ディレクトリなし:[{0}]", path);

                // ディレクトリ作成
                Directory.CreateDirectory(path);
            }

            // ロギング
            Logger.Debug("<<<<= DirectoryLibrary::Create(string)");
        }

        /// <summary>
        /// ディレクトリ内ファイル削除
        /// </summary>
        /// <param name="path"></param>
        public static void RemoveFiles(string path)
        {
            // ロギング
            Logger.Debug("=>>>> DirectoryLibrary::RemoveFiles(string)");
            Logger.DebugFormat("path:[{0}]", path);

            // DirectoryInfoオブジェクト生成
            DirectoryInfo directoryInfo = new DirectoryInfo(path);

            // ファイル一覧取得
            FileInfo[] files = directoryInfo.GetFiles();

            // ファイル一覧を繰り返す
            foreach (FileInfo file in files)
            {
                // ロギング
                Logger.DebugFormat("削除ファイル:[{0}]", file.Name);

                // ファイル削除
                file.Delete();
            }

            // ロギング
            Logger.Debug("<<<<= DirectoryLibrary::RemoveFiles(string)");
        }
    }
}
