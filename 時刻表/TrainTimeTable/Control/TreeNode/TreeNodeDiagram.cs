using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using TrainTimeTable.Component;
using TrainTimeTable.Property;

namespace TrainTimeTable.Control
{
    /// <summary>
    /// TreeNodeDiagramクラス
    /// </summary>
    public class TreeNodeDiagram : TreeNode
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        protected ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TreeNodeDiagram()
            : this("ダイヤ")
        {
            // ロギング
            Logger.Debug("=>>>> TreeNodeDia::TreeNodeDia()");

            // ロギング
            Logger.Debug("<<<<= TreeNodeDia::TreeNodeDia()");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="text"></param>
        public TreeNodeDiagram(string text)
            : base(text)
        {
            // ロギング
            Logger.Debug("=>>>> TreeNodeDia::TreeNodeDia(string)");
            Logger.DebugFormat("text:[{0}]", text);

            // ロギング
            Logger.Debug("<<<<= TreeNodeDia::TreeNodeDia(string)");
        }
        #endregion

        #region 更新
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="sequences"></param>
        /// <param name="properties"></param>
        public void Update(DiagramSequenceProperties sequences, DiagramProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> TreeNodeDia::Update(DiagramSequenceProperties, DiagramProperties)");
            Logger.DebugFormat("sequences :[{0}]", sequences);
            Logger.DebugFormat("properties:[{0}]", properties);

            // ノードクリア
            Nodes.Clear();

            // シーケンス分繰り返す
            foreach (var sequence in sequences.OrderBy(d => d.Seq))
            {
                // 登録オブジェクト取得
                DiagramProperty property = properties.Find(d => d.Name == sequence.Name);

                // TreeNodeDiagramDetailオブジェクト生成
                TreeNodeDiagramDetail treeNode = new TreeNodeDiagramDetail(property);

                // 登録
                Nodes.Add(treeNode);
            }

            // 全て展開状態にする
            ExpandAll();

            // ロギング
            Logger.Debug("<<<<= TreeNodeDia::Update(DiagramSequenceProperties, DiagramProperties)");
        }
        #endregion

        #region 削除
        /// <summary>
        /// 削除
        /// </summary>
        /// <param name="properties"></param>
        public void Remove(DiagramProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> TreeNodeDia::Remove(DiagramProperties)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // 削除対象ノード
            List<TreeNodeDiagramDetail> removeNodes = new List<TreeNodeDiagramDetail>();

            // ノードを繰り返す
            foreach (DiagramProperty property in properties)
            {
                // 登録されているノードを繰り返す
                foreach (TreeNodeDiagramDetail treeNode in Nodes)
                {
                    // ダイアグラム名判定
                    if (treeNode.Property.Name == property.Name)
                    {
                        // 削除ノード登録
                        removeNodes.Add(treeNode);
                    }
                }
            }

            // ノード削除
            foreach (TreeNodeDiagramDetail treeNode in removeNodes)
            {
                // ノード削除
                Nodes.Remove(treeNode);
            }

            // ロギング
            Logger.Debug("<<<<= TreeNodeDia::Remove(DiagramProperties)");
        }
        #endregion
    }
}