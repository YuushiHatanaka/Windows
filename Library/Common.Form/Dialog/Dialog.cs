using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Common.Dialog
{
    /// <summary>
    /// ダイアログベースクラス
    /// </summary>
    public partial class Dialog : Form
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Dialog()
            : base()
        {
            // コンポーネント初期化
            InitializeComponent();
        }
    }
}
