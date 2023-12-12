using log4net;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace TrainTimeTable
{
    /// <summary>
    /// Programクラス
    /// </summary>
    internal static class Program
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        private static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            // ロギング
            Logger.Debug("=>>>> Main()");

            // フォントコレクションを作成
            PrivateFontCollection fontCollection = new PrivateFontCollection();

            // Mutex名を決める
            string mutexName = Assembly.GetExecutingAssembly().GetName().Name;

            // Mutexオブジェクトを作成する
            Mutex mutex = new Mutex(false, mutexName);

            // ロギング
            Logger.DebugFormat("mutexName:{0}", mutexName);
            Logger.DebugFormat("mutex    :{0}", mutex);

            bool hasHandle = false;
            try
            {
                try
                {
                    //ミューテックスの所有権を要求する
                    hasHandle = mutex.WaitOne(0, false);

                    // ロギング
                    Logger.DebugFormat("hasHandle:{0}", hasHandle);
                }
                //.NET Framework 2.0以降の場合
                catch (AbandonedMutexException ex)
                {
                    // ロギング
                    Logger.Warn(ex.Message);
                    Logger.Warn(ex.StackTrace);

                    //  別のアプリケーションがミューテックスを解放しないで終了した時
                    hasHandle = true;
                }

                // ロギング
                Logger.DebugFormat("hasHandle:{0}", hasHandle);

                // ミューテックスを得られたか調べる
                if (hasHandle == false)
                {
                    Logger.WarnFormat("アプリ二重起動エラー:[{0}][{1}]", Environment.MachineName, Environment.UserName);

                    // 得られなかった場合は、すでに起動していると判断して終了
                    MessageBox.Show("このアプリは二重起動はできません", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                // ロギング
                Logger.InfoFormat("アプリ起動:[{0}][{1}]", Environment.MachineName, Environment.UserName);

#if DELETE
                // フォントファイル一覧を取得
                //string[] fontFiles =  Directory.GetFiles(@".\Font", "*.otf");
                List<string> fontFiles =  new List<string>();
                fontFiles.AddRange(Directory.GetFiles(@".\Font", "*.ttf"));
                //fontFiles.AddRange(Directory.GetFiles(@".\Font", "*.otf"));
                //fontFiles.AddRange(Directory.GetFiles(@".\Font", "*.woff"));

                // フォントファイル一覧を繰り返す
                foreach (var fontFilePath  in fontFiles)
                {
                    // TrueTypeフォントとして読み込む
                    fontCollection.AddFontFile(fontFilePath);

                    // ロギング
                    Logger.InfoFormat("フォントインストール:[{0}]", fontFilePath);
                }
#endif
                // はじめからMainメソッドにあったコードを実行
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FormMain());
            }
            catch (Exception ex)
            {
                // ロギング
                Logger.FatalFormat("アプリ例外:[{0}][{1}]", Environment.MachineName, Environment.UserName);
                Logger.Fatal(ex.Message);
                Logger.Fatal(ex.StackTrace);

                // 例外メッセージ表示
                MessageBox.Show(ex.Message, "例外", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                // フォントを解放
                fontCollection.Dispose();

                // ロギング
                Logger.DebugFormat("hasHandle:{0}", hasHandle);

                // ミューテックス取得判定
                if (hasHandle)
                {
                    // ミューテックスを解放する
                    mutex.ReleaseMutex();
                }

                // ミューテックスをクローズする
                mutex.Close();

                // ロギング
                Logger.InfoFormat("アプリ終了:[{0}][{1}]", Environment.MachineName, Environment.UserName);

                // ロギング
                Logger.Debug("<<<<= Main()");
            }
        }
    }
}
