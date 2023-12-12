using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrainTimeTable.Component;
using TrainTimeTable.Property;
using static System.Net.Mime.MediaTypeNames;

namespace TrainTimeTable.Control
{
    /// <summary>
    /// TreeNodeStationクラス
    /// </summary>
    public class TreeNodeStation : TreeNode
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
        /// <param name="text"></param>
        public TreeNodeStation()
            : this("駅")
        {
            // ロギング
            Logger.Debug("=>>>> TreeNodeStation::TreeNodeStation()");

            // ロギング
            Logger.Debug("<<<<= TreeNodeStation::TreeNodeStation()");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="text"></param>
        public TreeNodeStation(string text)
            : base(text)
        {
            // ロギング
            Logger.Debug("=>>>> TreeNodeStation::TreeNodeStation(string)");
            Logger.DebugFormat("text:[{0}]", text);

            // ロギング
            Logger.Debug("<<<<= TreeNodeStation::TreeNodeStation(string)");
        }
        #endregion

        #region 更新
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="properties"></param>
        public void Update(StationProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> TreeNodeStation::Update(StationProperties)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // TODO:未実装

            // ロギング
            Logger.Debug("<<<<= TreeNodeStation::Update(StationProperties)");
        }
        #endregion
    }
}