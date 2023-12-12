using log4net;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrainTimeTable.Property;

namespace TrainTimeTable.Control
{
    /// <summary>
    /// ListBoxDiagramクラス
    /// </summary>
    public class ListBoxDiagram : ListBox
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
        /// <param name="properties"></param>
        public ListBoxDiagram(DiagramProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> ListBoxDiagram::ListBoxDiagram(DiagramProperties)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // 更新
            Update(properties);

            // ロギング
            Logger.Debug("<<<<= ListBoxDiagram::ListBoxDiagram(DiagramProperties)");
        }
        #endregion

        #region publicメソッド
        /// <summary>
        /// 存在判定
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool Exists(string name)
        {
            // ロギング
            Logger.Debug("=>>>> ListBoxDiagram::Exists()");
            Logger.DebugFormat("name:[{0}]", name);

            // 要素を繰り返す
            foreach (DiagramProperty property in Items)
            {
                // 一致判定
                if (property.Name == name)
                {
                    // ロギング
                    Logger.DebugFormat("result:[{0}][存在あり]", name);
                    Logger.Debug("<<<<= ListBoxDiagram::ToArray()");

                    // 存在あり
                    return true;
                }
            }

            // ロギング
            Logger.DebugFormat("result:[{0}][存在なし]", name);
            Logger.Debug("<<<<= ListBoxDiagram::ToArray()");

            // 存在なし
            return false;
        }

        /// <summary>
        /// シーケンス番号更新
        /// </summary>
        public void UpdateSeq()
        {
            // ロギング
            Logger.Debug("=>>>> ListBoxDiagram::UpdateSeq()");

            // 初期化
            int seq = 1;

            // 要素を繰り返す
            foreach (DiagramProperty property in Items)
            {
                // 順序設定
                property.Seq = seq++;
            }

            // ロギング
            Logger.Debug("<<<<= ListBoxDiagram::UpdateSeq()");
        }

        /// <summary>
        /// 配列化
        /// </summary>
        /// <returns></returns>
        public DiagramProperties ToArray()
        {
            // ロギング
            Logger.Debug("=>>>> ListBoxDiagram::ToArray()");

            // 結果オブジェクト生成
            DiagramProperties result = new DiagramProperties();

            // 要素を繰り返す
            foreach (DiagramProperty property in Items)
            {
                // 結果オブジェクトに登録
                result.Add(property);
            }

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= ListBoxDiagram::ToArray()");

            // 返却
            return result;
        }

        /// <summary>
        /// 入替
        /// </summary>
        /// <param name="oldIndex"></param>
        /// <param name="newIndex"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void ChangeOrder(int oldIndex, int newIndex)
        {
            // ロギング
            Logger.Debug("=>>>> ListBoxDiagram::ChangeOrder(int, int)");
            Logger.DebugFormat("oldIndex:[{0}]", oldIndex);
            Logger.DebugFormat("newIndex:[{0}]", newIndex);

            if (newIndex > Items.Count - 1)
            {
                // ロギング
                Logger.FatalFormat("引数エラー:[newIndex={0}][Count-1=[{1}]", newIndex, Items.Count - 1);
                Logger.Fatal("<<<<= ListBoxDiagram::ChangeOrder(int, int)");

                // 例外
                throw new ArgumentOutOfRangeException(nameof(newIndex));
            }

            if (oldIndex == newIndex)
            {
                // ロギング
                Logger.WarnFormat("パラメータ同一のためインデックス変更なし:[{0}][{1}]", oldIndex, newIndex);
                Logger.Fatal("<<<<= ListBoxDiagram::ChangeOrder(int, int)");

                // 終了
                return;
            }

            // 入れ替え
            DiagramProperty oldProperty = (DiagramProperty)Items[oldIndex];
            DiagramProperty newProperty = (DiagramProperty)Items[newIndex];

            Items[oldIndex] = newProperty;
            Items[newIndex] = oldProperty;

            // ロギング
            Logger.Debug("<<<<= ListBoxDiagram::ChangeOrder(int, int)");
        }

        #region 更新
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="index"></param>
        /// <param name="property"></param>
        public void Update(int index, DiagramProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> ListBoxDiagram::Update(int, DiagramProperty)");
            Logger.DebugFormat("index   :[{0}]", index);
            Logger.DebugFormat("property:[{0}]", property);

            // 更新
            ((DiagramProperty)Items[index]).Copy(property);
            RefreshItem(index);

            // ロギング
            Logger.Debug("<<<<= ListBoxDiagram::Update(int, DiagramProperty)");
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="properties"></param>
        public void Update(DiagramProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> ListBoxDiagram::Update(DiagramProperties)");
            Logger.DebugFormat("properties:[{0}]", properties);

            Items.Clear();
            foreach (DiagramProperty property in properties)
            {
                Items.Add(property);
            }

            // シーケンス番号更新
            UpdateSeq();

            if (Items.Count > 0)
            {
                SelectedIndex = 0;
            }

            // ロギング
            Logger.Debug("<<<<= ListBoxDiagram::Update(DiagramProperties)");
        }
        #endregion
        #endregion
    }
}
