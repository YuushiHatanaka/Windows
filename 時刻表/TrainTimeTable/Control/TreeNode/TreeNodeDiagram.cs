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
        /// <param name="properties"></param>
        public void Update(DiagramProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> TreeNodeDia::Update(DiagramProperties)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // ノードクリア
            Nodes.Clear();

            // ノードを繰り返す
            foreach (DiagramProperty property in properties)
            {
                // TreeNode登録
                TreeNodeDiagramDetail treeNode = new TreeNodeDiagramDetail(property);
                Nodes.Add(treeNode);

                // 全て展開状態にする
                ExpandAll();
            }

            // ロギング
            Logger.Debug("<<<<= TreeNodeDia::Update(DiagramProperties)");
        }
        #endregion

        /// <summary>
        /// 削除
        /// </summary>
        /// <param name="properties"></param>
        public void Remove(DiagramProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> TreeNodeDia::Remove(DiagramProperties)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // ノードを繰り返す
            foreach (DiagramProperty property in properties)
            {
                // 登録されているノードを繰り返す
                foreach (TreeNodeDiagramDetail treeNode in Nodes)
                {
                    // ダイアグラム名判定
                    if (treeNode.Property.Name == property.Name)
                    {
                        // ノード削除
                        Nodes.Remove(treeNode);
                    }
                }
            }

            // ロギング
            Logger.Debug("<<<<= TreeNodeDia::Remove(DiagramProperties)");
        }
    }
}