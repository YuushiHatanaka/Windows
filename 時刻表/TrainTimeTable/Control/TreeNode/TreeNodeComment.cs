using log4net;
using System;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace TrainTimeTable.Control
{
    /// <summary>
    /// TreeNodeCommentクラス
    /// </summary>
    public class TreeNodeComment : TreeNode
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
        public TreeNodeComment()
            : this("コメント")
        {
            // ロギング
            Logger.Debug("=>>>> TreeNodeComment::TreeNodeComment()");

            // ロギング
            Logger.Debug("<<<<= TreeNodeComment::TreeNodeComment()");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="text"></param>
        public TreeNodeComment(string text)
            : base(text)
        {
            // ロギング
            Logger.Debug("=>>>> TreeNodeComment::TreeNodeComment(string)");
            Logger.DebugFormat("text:[{0}]", text);

            // ロギング
            Logger.Debug("<<<<= TreeNodeComment::TreeNodeComment(string)");
        }
        #endregion

        #region 更新
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="comment"></param>
        public void Update(StringBuilder comment)
        {
            // ロギング
            Logger.Debug("=>>>> TreeNodeComment::Update(StringBuilder)");
            Logger.DebugFormat("comment:[{0}]", comment.ToString());

            // TODO:未実装

            // ロギング
            Logger.Debug("<<<<= TreeNodeComment::Update(StringBuilder)");
        }
        #endregion
    }
}