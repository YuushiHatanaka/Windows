using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using TrainTimeTable.Property;

namespace TrainTimeTable.Control
{
    /// <summary>
    /// TreeViewRouteクラス
    /// </summary>
    public class TreeViewRoute : TreeView
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        protected ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region ノード
        /// <summary>
        /// ROOTノード
        /// </summary>
        private TreeNodeRoot m_RootNode = null;

        /// <summary>
        /// 駅ノード
        /// </summary>
        private TreeNodeStation m_StationNode = new TreeNodeStation("駅");

        /// <summary>
        /// 列車種別ノード
        /// </summary>
        private TreeNodeTrainType m_TrainType = new TreeNodeTrainType("列車種別");

        /// <summary>
        /// ダイヤノード
        /// </summary>
        private TreeNodeDiagram m_DiagramNode = new TreeNodeDiagram("ダイヤ");

        /// <summary>
        /// コメントノード
        /// </summary>
        private TreeNodeComment m_CommentNode = new TreeNodeComment("コメント");
        #endregion

        #region event delegate
        public delegate void NodeRootMouseClickEventHandler(object sender, TreeNodeRoot e);
        public delegate void NodeStationMouseClickEventHandler(object sender, TreeNodeStation e);
        public delegate void NodeTrainTypeoMouseClickEventHandler(object sender, TreeNodeTrainType e);
        public delegate void NodeDiaMouseDoubleClickEventHandler(object sender, TreeNodeDiagram e);
        public delegate void NodeCommentMouseClickEventHandler(object sender, TreeNodeComment e);
        public delegate void NodeOutboundTimetableMouseClickEventHandle(object sender, TreeNodeOutboundTimetable e);
        public delegate void NodeInboundTimeTableMouseClickEventHandle(object sender, TreeNodeInboundTimeTable e);
        public delegate void NodeDiagramMouseClickEventHandle(object sender, TreeNodeDiagramDraw e);
        public delegate void NodeStationTimetableMouseClickEventHandle(object sender, TreeNodeStationTimetable e);
        #endregion

        #region event
        /// <summary>
        /// 接続 event
        /// </summary>
        public event NodeRootMouseClickEventHandler OnNodeRootMouseClick = delegate { };
        public event NodeStationMouseClickEventHandler OnNodeStationMouseClick = delegate { };
        public event NodeTrainTypeoMouseClickEventHandler OnNodeTrainTypeMouseClick = delegate { };
        public event NodeDiaMouseDoubleClickEventHandler OnNodeDiaMouseDoubleClick = delegate { };
        public event NodeCommentMouseClickEventHandler OnNodeCommentMouseClick = delegate { };
        public event NodeOutboundTimetableMouseClickEventHandle OnNodeOutboundTimetableMouseClick = delegate { };
        public event NodeInboundTimeTableMouseClickEventHandle OnNodeInboundTimetableMouseClick = delegate { };
        public event NodeDiagramMouseClickEventHandle OnNodeDiagramMouseClick = delegate { };
        public event NodeStationTimetableMouseClickEventHandle OnNodeStationTimetableMouseClick = delegate { };
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TreeViewRoute()
        {
            // ロギング
            Logger.Debug("=>>>> TreeViewRoute::TreeViewRoute()");

            // コントコール設定
            NodeMouseClick += TreeViewRoute_NodeMouseClick;
            NodeMouseDoubleClick += TreeViewRoute_NodeMouseDoubleClick;
            BeforeCollapse += TreeViewRoute_BeforeCollapse;
            m_RootNode = new TreeNodeRoot();
            m_RootNode.Nodes.Add(m_StationNode);
            m_RootNode.Nodes.Add(m_TrainType);
            m_RootNode.Nodes.Add(m_DiagramNode);
            m_RootNode.Nodes.Add(m_CommentNode);
            Nodes.Add(m_RootNode);
            ExpandAll();

            // ロギング
            Logger.Debug("<<<<= TreeViewRoute::TreeViewRoute()");
        }
        #endregion

        #region TreeViewRoutイベント
        /// <summary>
        /// TreeViewRoute_BeforeCollapse
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewRoute_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> TreeViewRoute::TreeViewRoute_BeforeCollapse(object, TreeViewCancelEventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 折りたたみキャンセル
            e.Cancel = true;

            // ロギング
            Logger.Debug("<<<<= TreeViewRoute::TreeViewRoute_BeforeCollapse(object, TreeViewCancelEventArgs)");
        }

        /// <summary>
        /// TreeViewRoute_NodeMouseClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewRoute_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> TreeViewRoute::TreeViewRoute_NodeMouseClick(object, TreeNodeMouseClickEventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 種別で分岐
            switch (e.Node.GetType())
            {
                case Type @_ when @_ == typeof(TreeNodeStation):
                    // イベント呼出
                    OnNodeStationMouseClick(this, (TreeNodeStation)e.Node);
                    break;
                case Type @_ when @_ == typeof(TreeNodeTrainType):
                    // イベント呼出
                    OnNodeTrainTypeMouseClick(this, (TreeNodeTrainType)e.Node);
                    break;
                case Type @_ when @_ == typeof(TreeNodeComment):
                    // イベント呼出
                    OnNodeCommentMouseClick(this, (TreeNodeComment)e.Node);
                    break;
                case Type @_ when @_ == typeof(TreeNodeOutboundTimetable):
                    // イベント呼出
                    OnNodeOutboundTimetableMouseClick(this, (TreeNodeOutboundTimetable)e.Node);
                    break;
                case Type @_ when @_ == typeof(TreeNodeInboundTimeTable):
                    // イベント呼出
                    OnNodeInboundTimetableMouseClick(this, (TreeNodeInboundTimeTable)e.Node);
                    break;
                case Type @_ when @_ == typeof(TreeNodeDiagramDraw):
                    // イベント呼出
                    OnNodeDiagramMouseClick(this, (TreeNodeDiagramDraw)e.Node);
                    break;
                case Type @_ when @_ == typeof(TreeNodeStationTimetable):
                    // イベント呼出
                    OnNodeStationTimetableMouseClick(this, (TreeNodeStationTimetable)e.Node);
                    break;
                default:
                    break;
            }

            // ロギング
            Logger.Debug("<<<<= TreeViewRoute::TreeViewRoute_NodeMouseClick(object, TreeNodeMouseClickEventArgs)");
        }

        /// <summary>
        /// TreeViewRoute_NodeMouseDoubleClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewRoute_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> TreeViewRoute::TreeViewRoute_NodeMouseDoubleClick(object, TreeNodeMouseClickEventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 種別で分岐
            switch (e.Node.GetType())
            {
                case Type @_ when @_ == typeof(TreeNodeRoot):
                    // イベント呼出
                    OnNodeRootMouseClick(this, (TreeNodeRoot)e.Node);
                    break;
                case Type @_ when @_ == typeof(TreeNodeDiagram):
                    {
                        // TreeNodeDiaオブジェクト取得
                        TreeNodeDiagram node = (TreeNodeDiagram)e.Node;

                        node.Expand();

                        // イベント呼出
                        OnNodeDiaMouseDoubleClick(this, node);
                    }
                    break;
                default:
                    break;
            }

            // ロギング
            Logger.Debug("<<<<= TreeViewRoute::TreeViewRoute_NodeMouseDoubleClick(object, TreeNodeMouseClickEventArgs)");
        }
        #endregion

        #region 更新
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="property"></param>
        public void Update(RouteFileProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> TreeViewRoute::Update(RouteFileProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // ルートノード更新
            m_RootNode.Update(property);

            // 駅ノード更新
            m_StationNode.Update(property.Stations);

            // 列車種別ノード更新
            m_TrainType.Update(property.TrainTypes);

            // ダイヤグラムノード
            m_DiagramNode.Update(property.DiagramSequences, property.Diagrams);

            // コメントノード
            m_CommentNode.Update(property.Comment);

            // ロギング
            Logger.Debug("<<<<= TreeViewRoute::Update(RouteFileProperty)");
        }
        #endregion

        #region ダイアグラム削除
        /// <summary>
        /// ダイアグラム削除
        /// </summary>
        /// <param name="properties"></param>
        public void RemoveDiagram(DiagramProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> TreeViewRoute::RemoveDiagram(DiagramProperties)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // ダイヤグラムノード
            m_DiagramNode.Remove(properties);

            // ロギング
            Logger.Debug("<<<<= TreeViewRoute::RemoveDiagram(DiagramProperties)");
        }
        #endregion

        #region 路線名設定
        /// <summary>
        /// 路線名設定
        /// </summary>
        /// <param name="name"></param>
        public void SetRouteName(string name)
        {
            // ロギング
            Logger.Debug("=>>>> TreeViewRoute::SetRouteName(string)");
            Logger.DebugFormat("name:[{0}]", name);

            // ルートノードに路線名設定
            m_RootNode.SetRouteName(name);

            // ロギング
            Logger.Debug("<<<<= TreeViewRoute::SetRouteName(string)");
        }
        #endregion
    }
}
