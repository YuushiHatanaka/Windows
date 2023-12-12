using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrainTimeTable.Component
{
    /// <summary>
    /// DictionaryFormRouteクラス
    /// </summary>
    [Serializable]
    public class DictionaryFormRoute : Dictionary<string, FormRoute>
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        private static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DictionaryFormRoute()
        {
            // ロギング
            Logger.Debug("=>>>> DictionaryFormRoute::DictionaryFormRoute()");

            // ロギング
            Logger.Debug("<<<<= DictionaryFormRoute::DictionaryFormRoute()");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="properties"></param>
        public DictionaryFormRoute(DictionaryFormRoute properties)
        {
            // ロギング
            Logger.Debug("=>>>> DictionaryFormRoute::DictionaryFormRoute(DictionaryFormRoute)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // コピー
            Copy(properties);

            // ロギング
            Logger.Debug("<<<<= DictionaryFormRoute::DictionaryFormRoute(DictionaryFormRoute)");
        }
        #endregion

        #region コピー
        /// <summary>
        /// コピー
        /// </summary>
        /// <param name="properties"></param>
        public void Copy(DictionaryFormRoute properties)
        {
            // ロギング
            Logger.Debug("=>>>> DictionaryFormRoute::Copy(DictionaryFormRoute)");
            Logger.DebugFormat("property:[{0}]", properties);

            // クリア
            Clear();

            // 要素を繰り返す
            foreach (var key in properties.Keys)
            {
                // 登録
                Add(key, properties[key]);
            }

            // ロギング
            Logger.Debug("<<<<= DictionaryFormRoute::Copy(DictionaryFormRoute)");
        }
        #endregion

        #region 比較
        /// <summary>
        /// 比較
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        public bool Compare(DictionaryFormRoute properties)
        {
            // ロギング
            Logger.Debug("=>>>> DictionaryFormRoute::Compare(DictionaryFormRoute)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // リストを繰り返す
            foreach (var property in properties)
            {
                // キー登録判定
                if (!ContainsKey(property.Key))
                {
                    // ロギング
                    Logger.DebugFormat("key:[{0}][キー登録なし]", property.Key);
                    Logger.Debug("<<<<= DictionaryFormRoute::Compare(DictionaryFormRoute)");

                    // 不一致
                    return false;
                }

                // 内容判定
                if (!this[property.Key].Compare(property.Value))
                {
                    // ロギング
                    Logger.DebugFormat("Property:[{0}][{1}][不一致]", this[property.Key].ToString(), property.Value.ToString());
                    Logger.Debug("<<<<= DictionaryFormRoute::Compare(DictionaryFormRoute)");

                    // 不一致
                    return false;
                }
            }

            // ロギング
            Logger.Debug("result:[一致]");
            Logger.Debug("<<<<= DictionaryFormRoute::Compare(DictionaryFormRoute)");

            // 一致
            return true;
        }
        #endregion

        #region publicメソッド
        /// <summary>
        /// 存在判定
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool IsExist(string fileName)
        {
            // ロギング
            Logger.Debug("=>>>> DictionaryFormRoute::IsExist(string)");
            Logger.DebugFormat("fileName:[{0}]", fileName);

            // 登録判定
            if (ContainsKey(fileName))
            {
                // ロギング
                Logger.DebugFormat("登録あり:[{0}]", fileName);
                Logger.Debug("<<<<= DictionaryFormRoute::IsExist(string)");

                // 登録あり
                return true;
            }

            // ロギング
            Logger.DebugFormat("登録なし:[{0}]", fileName);
            Logger.Debug("<<<<= DictionaryFormRoute::IsExist(string)");

            // 登録なし
            return false;
        }

        /// <summary>
        /// 上書き確認
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool OverwriteConfirmation(string fileName)
        {
            // ロギング
            Logger.Debug("=>>>> DictionaryFormRoute::OverwriteConfirmation(string)");
            Logger.DebugFormat("fileName:[{0}]", fileName);

            // データベースファイル存在判定
            if (System.IO.File.Exists(fileName))
            {
                // メッセージ表示
                DialogResult result = MessageBox.Show(
                    "同名ファイルが存在します。上書きしますか？",
                    "上書き確認",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question);

                // メッセージ判定
                if (result != DialogResult.OK)
                {
                    // ロギング
                    Logger.DebugFormat("result:{0}", result);
                    Logger.Debug("<<<<= DictionaryFormRoute::OverwriteConfirmation(string)");

                    // 上書きしない
                    return false;
                }

                // ファイルを削除
                System.IO.File.Delete(fileName);
            }

            // ロギング
            Logger.Debug("<<<<= DictionaryFormRoute::OverwriteConfirmation(string)");

            // 上書きする
            return true;
        }

        /// <summary>
        /// FormRoute作成
        /// </summary>
        /// <param name="mdiParent"></param>
        /// <param name="eventHandler"></param>
        /// <returns></returns>
        public FormRoute Create(Form parent)
        {
            // ロギング
            Logger.Debug("<<<<= DictionaryFormRoute::Create(Form)");
            Logger.DebugFormat("parent:{0}", parent);

            // 新しいフォーム作成
            FormRoute form = new FormRoute();
            form.MdiParent = parent;
            form.BringToFront();
            form.WindowState = FormWindowState.Maximized;
            form.FormClosed += EventFormClosed;

            // ロギング
            Logger.DebugFormat("form:{0}", form);
            Logger.Debug("<<<<= DictionaryFormRoute::Create(Form)");

            // 返却
            return form;
        }

        /// <summary>
        /// 登録
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="form"></param>
        /// <returns></returns>
        public void Regston(string fileName, FormRoute form)
        {
            // ロギング
            Logger.Debug("=>>>> DictionaryFormRoute::Regston(string, FormRoute)");
            Logger.DebugFormat("fileName:[{0}]", fileName);
            Logger.DebugFormat("form    :[{0}]", form);

            // 設定
            form.FileName = fileName;

            // 登録
            Add(fileName, form);

            // ロギング
            Logger.Debug("<<<<= DictionaryFormRoute::Regston(string, FormRoute)");
        }

        #region 文字列化
        /// <summary>
        /// 文字列化
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            // 結果オブジェクト生成
            StringBuilder result = new StringBuilder();

            // リストを繰り返す
            foreach (var property in this)
            {
                // 文字列追加
                result.AppendLine(string.Format("キー:[{0}],値:[{1}]", property.Key, property.Value.FileName));
            }

            // 返却
            return result.ToString();
        }
        #endregion
        #endregion

        #region privateメソッド
        /// <summary>
        /// FormRouteクローズ時イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EventFormClosed(object sender, FormClosedEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> DictionaryFormRoute::EventFormClosed(object, FormClosedEventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // オブジェクト変換
            FormRoute form = sender as FormRoute;

            // フォーム名が設定されているか？
            if (form.FileName != null && form.FileName != string.Empty)
            {
                // 登録解除
                Remove(form.FileName);
            }

            // ロギング
            Logger.Debug("<<<<= DictionaryFormRoute::EventFormClosed(object, FormClosedEventArgs)");
        }
        #endregion
    }
}
