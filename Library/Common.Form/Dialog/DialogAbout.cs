using Common.Forms;
using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using static System.Collections.Specialized.BitVector32;

namespace Common.Dialog
{
    /// <summary>
    /// バージョン情報表示クラス
    /// </summary>
    public partial class DialogAbout : Dialog
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        private static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        /// <summary>
        /// アプリケーション名
        /// </summary>
        private string m_ApplicationName = string.Empty;

        /// <summary>
        /// バージョン
        /// </summary>
        private string m_Version = string.Empty;

        /// <summary>
        /// Copyright
        /// </summary>
        private string m_Copyright = string.Empty;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="icon"></param>
        /// <param name="name"></param>
        /// <param name="version"></param>
        /// <param name="copyright"></param>
        public DialogAbout(Icon icon, string name, string version, string copyright)
            : base()
        {
            // ロギング
            Logger.Debug("=>>>> DialogAbout::DialogAbout(Icon, string, string, string)");
            Logger.DebugFormat("icon     :[{0}]", icon);
            Logger.DebugFormat("name     :[{0}]", name);
            Logger.DebugFormat("version  :[{0}]", version);
            Logger.DebugFormat("copyright:[{0}]", copyright);

            // 設定
            Icon = icon;
            m_ApplicationName = name;
            m_Version = version;
            m_Copyright = copyright;

            // コンポーネント初期化
            InitializeComponent();

            // タイトル設定
            SetTitle();

            // 変換
            PropertyToControl();

            // ロギング
            Logger.Debug("<<<<= DialogAbout::DialogAbout(Icon, string, string, string)");
        }

        /// <summary>
        /// DialogAbout_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DialogAbout_Load(object sender, EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> DialogAbout::DialogAbout_Load(Icon, string)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // アイコン設定
            SetIcon();

            // ロギング
            Logger.Debug("<<<<= DialogAbout::DialogAbout_Load(Icon, string)");
        }

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

        #region タイトル設定
        /// <summary>
        /// タイトル設定
        /// </summary>
        private void SetTitle()
        {
            // ロギング
            Logger.Debug("=>>>> DialogAbout::SetTitle()");

            // 設定
            Text = string.Format("{0} - Ver.{1}", m_ApplicationName, m_Version);

            // ロギング
            Logger.DebugFormat("Text:[{0}]", Text);
            Logger.Debug("<<<<= DialogAbout::SetTitle()");
        }

        /// <summary>
        /// タイトル設定
        /// </summary>
        /// <param name="title"></param>
        /// <exception cref="NotImplementedException"></exception>

        private void SetTitle(string title)
        {
            // ロギング
            Logger.Debug("=>>>> DialogAbout::SetTitle(string)");
            Logger.DebugFormat("title:[{0}]", title);

            // 設定
            Text = title;

            // ロギング
            Logger.DebugFormat("Text:[{0}]", Text);
            Logger.Debug("<<<<= DialogAbout::SetTitle(string)");
        }
        #endregion

        /// <summary>
        /// プロパティ⇒コントコール変換
        /// </summary>
        private void PropertyToControl()
        {
            // ロギング
            Logger.Debug("=>>>> DialogAbout::PropertyToControl()");

            // 変換
            labelApplicationName.Text = m_ApplicationName;
            labelVersion.Text = "Ver." + m_Version;
            labelCopyRight.Text = m_Copyright;

            // ロギング
            Logger.Debug("<<<<= DialogAbout::PropertyToControl()");
        }
    }
}
