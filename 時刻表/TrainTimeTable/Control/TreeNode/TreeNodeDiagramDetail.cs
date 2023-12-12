using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Linq;
using TrainTimeTable.Property;

namespace TrainTimeTable.Control
{
    /// <summary>
    /// TreeNodeDiagramDetailクラス
    /// </summary>
    public class TreeNodeDiagramDetail : TreeNode
    {
        private DiagramProperty m_Property;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="text"></param>
        public TreeNodeDiagramDetail(string text)
            : base(text)
        {
            // TODO:暫定
            m_Property = new DiagramProperty();
            Nodes.Add(new TreeNodeOutboundTimetable());
            Nodes.Add(new TreeNodeInboundTimeTable());
            Nodes.Add(new TreeNodeDiagramDraw());
            Nodes.Add(new TreeNodeStationTimetable());
        }

        public TreeNodeDiagramDetail(DiagramProperty property)
            : this(property.Name)
        {
            m_Property.Copy(property);
        }
    }
}
