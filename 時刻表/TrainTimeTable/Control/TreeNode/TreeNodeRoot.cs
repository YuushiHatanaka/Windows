using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrainTimeTable.Property;
using static System.Net.Mime.MediaTypeNames;

namespace TrainTimeTable.Control
{
    /// <summary>
    /// TreeNodeRootクラス
    /// </summary>
    public class TreeNodeRoot : TreeNode
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        protected ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="text"></param>
        public TreeNodeRoot()
            : this("路線")
        {
            // ロギング
            Logger.Debug("=>>>> TreeNodeRoot::TreeNodeRoot()");

            // ロギング
            Logger.Debug("<<<<= TreeNodeRoot::TreeNodeRoot()");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="text"></param>
        public TreeNodeRoot(string text)
        {
            // ロギング
            Logger.Debug("=>>>> TreeNodeRoot::TreeNodeRoot(string)");
            Logger.DebugFormat("text:[{0}]", text);

            // 路線名設定
            SetRouteName(text);

            // ロギング
            Logger.Debug("<<<<= TreeNodeRoot::TreeNodeRoot(string)");
        }

        /// <summary>
        /// 路線名設定
        /// </summary>
        /// <param name="name"></param>
        public void SetRouteName(string name)
        {
            // ロギング
            Logger.Debug("=>>>> TreeNodeRoot::SetRouteName(string)");
            Logger.DebugFormat("name:[{0}]", name);

            if (name == string.Empty)
            {
                Text = "路線";
            }
            else
            {
                Text = name;
            }

            // ロギング
            Logger.Debug("<<<<= TreeNodeRoot::SetRouteName(string)");
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="property"></param>
        public void Update(RouteFileProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> TreeNodeRoot::Update(RouteFileProperty)");
            Logger.DebugFormat("RouteProperty:[{0}]", property);

            // 路線名設定
            SetRouteName(property.Route.Name);

            // TODO:未実装

            // ロギング
            Logger.Debug("<<<<= TreeNodeRoot::Update(RouteFileProperty)");
        }
    }
}
