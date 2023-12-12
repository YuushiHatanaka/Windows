using log4net;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common.Font
{
    /// <summary>
    /// FontLibraryクラス
    /// </summary>
    public class FontLibrary
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        protected static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region インストールフォント一覧取得
        /// <summary>
        /// インストールフォント一覧取得
        /// </summary>
        public static List<FontFamily> GetInstalledFontList()
        {
            // ロギング
            Logger.Debug("=>>>> FontLibrary::GetInstalledFontList()");

            // InstalledFontCollectionオブジェクトの取得
            InstalledFontCollection installedFontCollection =　new InstalledFontCollection();

            //インストールされているすべてのフォントファミリアを取得
            FontFamily[] result = installedFontCollection.Families;

            // ロギング
            Logger.DebugFormat("result:[{0}]", result.ToString());
            Logger.Debug("<<<<= FontLibrary::GetFont()");

            // 返却
            return result.ToList();
        }
        #endregion

        #region Fontオブジェクト取得
        /// <summary>
        /// Fontオブジェクト取得
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static System.Drawing.Font GetFont(string fileName, float size)
        {
            // ロギング
            Logger.Debug("=>>>> FontLibrary::GetFont(string, float)");
            Logger.DebugFormat("fileName:[{0}]", fileName);
            Logger.DebugFormat("size    :[{0}]", size);

            // PrivateFontCollectionをインスタンス化する
            PrivateFontCollection pfc = new PrivateFontCollection();

            // ttfファイルを追加する
            pfc.AddFontFile(fileName);

            // フォントのインスタンスを作成する
            System.Drawing.Font result = new System.Drawing.Font(pfc.Families[0], size);

            // ロギング
            Logger.DebugFormat("result:[{0}]", result.ToString());
            Logger.Debug("<<<<= FontLibrary::GetFont(string, float)");

            // 返却
            return result;
        }

        /// <summary>
        /// Fontオブジェクト取得
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="size"></param>
        /// <param name="style"></param>
        /// <returns></returns>
        public static System.Drawing.Font GetFont(string fileName, float size, FontStyle style)
        {
            // ロギング
            Logger.Debug("=>>>> FontLibrary::GetFont(string, float, FontStyle)");
            Logger.DebugFormat("fileName:[{0}]", fileName);
            Logger.DebugFormat("size    :[{0}]", size);
            Logger.DebugFormat("style   :[{0}]", style);

            // PrivateFontCollectionオブジェクト生成
            PrivateFontCollection privateFontCollection = new PrivateFontCollection();

            // ttfファイルを追加する
            privateFontCollection.AddFontFile(fileName);

            // フォントのインスタンスを作成する
            System.Drawing.Font result = new System.Drawing.Font(privateFontCollection.Families[0], size, style);

            // ロギング
            Logger.DebugFormat("result:[{0}]", result.ToString());
            Logger.Debug("<<<<= FontLibrary::GetFont(string, float, FontStyle)");

            // 返却
            return result;
        }
        #endregion
    }
}
