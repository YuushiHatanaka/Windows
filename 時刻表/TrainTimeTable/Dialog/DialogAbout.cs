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
using TrainTimeTable.Common;
using static System.Collections.Specialized.BitVector32;

namespace TrainTimeTable.Dialog
{
    /// <summary>
    /// DialogAboutクラス
    /// </summary>
    public partial class DialogAbout : Form
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        private static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="parent"></param>
        public DialogAbout(Form parent)
        {
            // ロギング
            Logger.Debug("=>>>> DialogAbout::DialogAbout(Form)");
            Logger.DebugFormat("parent:[{0}]", parent);

            // コンポーネント初期化
            InitializeComponent();

            // 設定
            Icon = parent.Icon;

            // ロギング
            Logger.Debug("<<<<= DialogAbout::DialogAbout(Form)");
        }
        #endregion

        #region イベント
        #region DialogAboutイベント
        /// <summary>
        /// DialogAbout_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DialogAbout_Load(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> DialogAbout::DialogAbout_Load(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // タイトル設定
            SetTitle();

            // アイコン設定
            SetIcon();

            // プロパティ⇒コントコール変換
            PropertiesToControl();

            // ロギング
            Logger.Debug("<<<<= DialogAbout::DialogAbout_Load(object, EventArgs)");
        }
        #endregion
        #endregion

        #region privateメソッド
        #region タイトル設定
        /// <summary>
        /// タイトル設定
        /// </summary>
        private void SetTitle()
        {
            // ロギング
            Logger.Debug("=>>>> DialogAbout::SetTitle()");

            // 設定
            Text = string.Format("{0} - Ver.{1}", AssemblyLibrary.GetTitle(), AssemblyLibrary.GetVersion());

            // ロギング
            Logger.DebugFormat("Text:[{0}]", Text);
            Logger.Debug("<<<<= DialogAbout::SetTitle()");
        }
        #endregion

        #region アイコン設定
        /// <summary>
        /// アイコン設定
        /// </summary>
        private void SetIcon()
        {
            // ロギング
            Logger.Debug("=>>>> DialogAbout::SetIcon()");

            // IconからBitmapを生成
            Bitmap bitmap = Icon.ToBitmap();

            // BitmapからPictureを生成
            pictureBoxIcon.Image = bitmap;

            // ロギング
            Logger.Debug("<<<<= DialogAbout::SetIcon()");
        }
        #endregion

        #region プロパティ⇒コントコール変換
        /// <summary>
        /// プロパティ⇒コントコール変換
        /// </summary>
        private void PropertiesToControl()
        {
            // ロギング
            Logger.Debug("=>>>> DialogAbout::PropertiesToControl()");

            // 変換
            labelApplicationName.Text = AssemblyLibrary.GetTitle();
            labelVersion.Text = "Ver." + AssemblyLibrary.GetVersion();
            labelCopyRight.Text = AssemblyLibrary.GetCopyright();

            // ロギング
            Logger.Debug("<<<<= DialogAbout::PropertiesToControl()");
        }
        #endregion
        #endregion
    }
}
