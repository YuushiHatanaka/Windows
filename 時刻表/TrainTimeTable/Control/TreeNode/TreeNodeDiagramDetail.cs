using log4net;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using System.Xml.Linq;
using TrainTimeTable.Property;
using static System.Net.Mime.MediaTypeNames;

namespace TrainTimeTable.Control
{
    /// <summary>
    /// TreeNodeDiagramDetailクラス
    /// </summary>
    public class TreeNodeDiagramDetail : TreeNode
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        protected ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        /// <summary>
        /// DiagramPropertyオブジェクト
        /// </summary>
        private DiagramProperty m_Property;

        /// <summary>
        /// ダイアグラムProperty
        /// </summary>
        public DiagramProperty Property
        {
            get { return m_Property; }
        }

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="text"></param>
        public TreeNodeDiagramDetail(string text)
            : base(text)
        {
            // ロギング
            Logger.Debug("=>>>> TreeNodeDiagramDetail::TreeNodeDiagramDetail(string)");
            Logger.DebugFormat("text:[{0}]", text);

            // 設定
            m_Property = new DiagramProperty();
            Nodes.Add(new TreeNodeOutboundTimetable());
            Nodes.Add(new TreeNodeInboundTimeTable());
            Nodes.Add(new TreeNodeDiagramDraw());
            Nodes.Add(new TreeNodeStationTimetable());

            // ロギング
            Logger.Debug("<<<<= TreeNodeDiagramDetail::TreeNodeDiagramDetail(string)");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="property"></param>
        public TreeNodeDiagramDetail(DiagramProperty property)
            : this(property.Name)
        {
            // ロギング
            Logger.Debug("=>>>> TreeNodeDiagramDetail::TreeNodeDiagramDetail(DiagramProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // コピー
            m_Property.Copy(property);

            // ロギング
            Logger.Debug("<<<<= TreeNodeDiagramDetail::TreeNodeDiagramDetail(DiagramProperty)");
        }
        #endregion
    }
}
