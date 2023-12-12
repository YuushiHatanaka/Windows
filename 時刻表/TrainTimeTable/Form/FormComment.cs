using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrainTimeTable.EventArgs;
using TrainTimeTable.Property;

namespace TrainTimeTable
{
    public partial class FormComment : Form
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        private static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region 更新 Event
        /// <summary>
        /// 更新 event delegate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void UpdateEventHandler(object sender, CommentUpdateEventArgs e);

        /// <summary>
        /// 更新 event
        /// </summary>
        public event UpdateEventHandler OnUpdate = delegate { };
        #endregion

        #region コメント文字列
        /// <summary>
        /// コメント文字列
        /// </summary>
        private StringBuilder m_Comment = new StringBuilder();
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="comment"></param>
        public FormComment(StringBuilder comment)
        {
            // ロギング
            Logger.Debug("=>>>> FormComment::FormComment(StringBuilder)");
            Logger.DebugFormat("comment:[{0}]", comment.ToString());

            // コンポーネント初期化
            InitializeComponent();

            // 設定
            m_Comment.Append(comment.ToString());

            // ロギング
            Logger.Debug("<<<<= FormComment::FormComment(StringBuilder)");
        }
        #endregion

        #region イベント
        #region FormCommentイベント
        /// <summary>
        /// FormComment_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormComment_Load(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormComment::FormComment_Load(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // コントコール設定
            textBoxComment.Dock = DockStyle.Fill;
            textBoxComment.MouseClick += OnMouseClick;

            // 変換
            PropertyToControl();

            // ロギング
            Logger.Debug("<<<<= FormComment::FormComment_Load(object, EventArgs)");
        }

        #region textBoxCommentイベント
        /// <summary>
        /// textBoxComment_TextChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void textBoxComment_TextChanged(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormComment::textBoxComment_TextChanged(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 設定
            ControlToProperty();

            // イベント呼出
            OnUpdate(this, new CommentUpdateEventArgs() { Comment = m_Comment });

            // ロギング
            Logger.Debug("<<<<= FormComment::textBoxComment_TextChanged(object, EventArgs)");
        }
        #endregion

        #region Mouseイベント
        /// <summary>
        /// OnMouseClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FormComment::OnMouseClick(object, MouseEventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // フォームを前面に表示する
            BringToFront();

            // ロギング
            Logger.Debug("<<<<= FormComment::OnMouseClick(object, MouseEventArgs)");
        }
        #endregion
        #endregion
        #endregion

        #region publicメソッド
        /// <summary>
        /// 更新通知
        /// </summary>
        /// <param name="property"></param>
        public void UpdateNotification(RouteFileProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> FormComment::UpdateNotification(TimetableProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // 更新
            Update(property.Comment);

            // ロギング
            Logger.Debug("<<<<= FormComment::UpdateNotification(TimetableProperty)");
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="comment"></param>
        public void Update(StringBuilder comment)
        {
            // ロギング
            Logger.Debug("=>>>> FormComment::Update(StringBuilder)");
            Logger.DebugFormat("comment:[{0}]", comment.ToString());

            // 設定
            textBoxComment.Text = comment.ToString();

            // 変換
            PropertyToControl();

            // ロギング
            Logger.Debug("<<<<= FormComment::Update(StringBuilder)");
        }
        #endregion

        #region privateメソッド
        /// <summary>
        /// プロパティ⇒コントコール変換
        /// </summary>
        private void PropertyToControl()
        {
            // ロギング
            Logger.Debug("=>>>> FormComment::PropertyToControl()");

            // 設定
            textBoxComment.Text = m_Comment.ToString();

            // ロギング
            Logger.Debug("<<<<= FormComment::PropertyToControl()");
        }

        /// <summary>
        /// コントコール⇒プロパティ変換
        /// </summary>
        /// <returns></returns>
        private void ControlToProperty()
        {
            // ロギング
            Logger.Debug("=>>>> FormComment::ControlToProperty()");

            // 設定
            m_Comment.Clear();
            m_Comment.Append(textBoxComment.Text);

            // ロギング
            Logger.Debug("<<<<= FormComment::ControlToProperty()");
        }
        #endregion
    }
}
