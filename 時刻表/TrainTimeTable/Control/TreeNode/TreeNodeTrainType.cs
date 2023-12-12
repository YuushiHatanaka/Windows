using log4net;
using System;
using System.Reflection;
using System.Windows.Forms;
using TrainTimeTable.Component;
using TrainTimeTable.Property;
using static System.Net.Mime.MediaTypeNames;

namespace TrainTimeTable.Control
{
    /// <summary>
    /// TreeNodeTrainTypeクラス
    /// </summary>
    public class TreeNodeTrainType : TreeNode
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
        public TreeNodeTrainType()
            : this("列車種別")
        {
            // ロギング
            Logger.Debug("=>>>> TreeNodeTrainType::TreeNodeTrainType()");

            // ロギング
            Logger.Debug("<<<<= TreeNodeTrainType::TreeNodeTrainType()");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="text"></param>
        public TreeNodeTrainType(string text)
            : base(text)
        {
            // ロギング
            Logger.Debug("=>>>> TreeNodeTrainType::TreeNodeTrainType(string)");
            Logger.DebugFormat("text:[{0}]", text);

            // ロギング
            Logger.Debug("<<<<= TreeNodeTrainType::TreeNodeTrainType(string)");
        }
        #endregion

        #region 更新
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="properties"></param>
        public void Update(TrainTypeProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> TreeNodeTrainType::Update(TrainTypeProperties)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // TODO:未実装

            // ロギング
            Logger.Debug("<<<<= TreeNodeTrainType::Update(TrainTypeProperties)");
        }
        #endregion
    }
}