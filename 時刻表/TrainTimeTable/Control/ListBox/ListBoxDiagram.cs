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

        /// <summary>
        /// TimetablePropertyオブジェクト
        /// </summary>
        private RouteFileProperty m_RouteFileProperty = null;

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="property"></param>
        public ListBoxDiagram(RouteFileProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> ListBoxDiagram::ListBoxDiagram(RouteFileProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // 設定
            m_RouteFileProperty = property;

            // 更新
            Update(property.DiagramSequences, property.Diagrams);

            // ロギング
            Logger.Debug("<<<<= ListBoxDiagram::ListBoxDiagram(RouteFileProperty)");
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
                m_RouteFileProperty.DiagramSequences.Find(d => d.Name == property.Name).Seq = seq++;
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

        /// <summary>
        /// DiagramProperty取得
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public DiagramProperty GetProperty(string name)
        {
            // ロギング
            Logger.Debug("=>>>> ListBoxDiagram::GetProperty(string)");
            Logger.DebugFormat("name:[{0}]", name);

            // 結果オブジェクト取得
            DiagramProperty result = null;

            // 要素を繰り返す
            foreach (DiagramProperty item in Items)
            {
                // 名前判定
                if (item.Name == name)
                {
                    // 一致
                    result = item;
                    break;
                }
            }

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= ListBoxDiagram::GetProperty(string)");

            // 返却
            return result;
        }

        #region 更新
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="index"></param>
        /// <param name="property"></param>
        public void Update(DiagramProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> ListBoxDiagram::Update(DiagramProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // DiagramPropertyオブジェクト取得
            DiagramProperty target = GetProperty(property.Name);

            // 更新
            target.Copy(property);

            // 表示更新
            RefreshItems();

            // ロギング
            Logger.Debug("<<<<= ListBoxDiagram::Update(DiagramProperty)");
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="sequences"></param>
        /// <param name="properties"></param>
        public void Update(DiagramSequenceProperties sequences, DiagramProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> ListBoxDiagram::Update(DiagramSequenceProperties, DiagramProperties)");
            Logger.DebugFormat("sequences :[{0}]", sequences);
            Logger.DebugFormat("properties:[{0}]", properties);

            // アイテムクリア
            Items.Clear();

            // シーケンスコピー
            m_RouteFileProperty.DiagramSequences.Copy(sequences);

            // シーケンス分繰り返す
            foreach (var sequence in m_RouteFileProperty.DiagramSequences.OrderBy(d => d.Seq))
            {
                // 登録オブジェクト取得
                DiagramProperty property = properties.Find(d => d.Name == sequence.Name);

                // 登録
                Items.Add(property);
            }

            // シーケンス番号更新
            UpdateSeq();

            // 選択インデックス設定
            if (Items.Count > 0)
            {
                SelectedIndex = 0;
            }

            // ロギング
            Logger.Debug("<<<<= ListBoxDiagram::Update(DiagramSequenceProperties, DiagramProperties)");
        }
        #endregion

        /// <summary>
        /// ダイアグラム名変更
        /// </summary>
        /// <param name="oldName"></param>
        /// <param name="newName"></param>
        public void ChangeDiagramName(string oldName, string newName)
        {
            // ロギング
            Logger.Debug("=>>>> ListBoxDiagram::ChangeDiagramName(string, string)");
            Logger.DebugFormat("oldName:[{0}]", oldName);
            Logger.DebugFormat("newName:[{0}]", newName);

            // DiagramPropertyオブジェクト取得(旧)
            DiagramProperty property = GetProperty(oldName);

            // ダイアグラム名変更
            property.ChangeDiagramName(newName);

            // ロギング
            Logger.Debug("<<<<= RouteFileProperty::ChangeDiagramName(string, string)");
        }
        #endregion
    }
}
